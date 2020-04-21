using api.requests;
using api.responses;
using api.Servers.LogServer.Interface;
using api.Servers.WareServer.Interface;
using common.Consts;
using common.DB.Interface;
using common.SqlMaker.Interface;
using models.db_models;
using models.enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace api.Servers.WareServer.Impl
{
    /// <summary>
    /// 仓库服务
    /// </summary>
    public class WareServerImpl : BaseServiceImpl, IWareServer
    {
        public WareServerImpl(IDbHelper dbHelper = null, ILogServer logServer = null) : base(dbHelper, logServer) { }

        public async Task<Result> AddWareAsync(reqmodel<AddWareModel> reqmodel)
        {
            Result result = new Result { code = ErrorCodeConst.ERROR_1030, status = ErrorCodeConst.ERROR_403 };

            string sql_insert = g_sqlMaker.Insert<t_ware>(i =>
            new
            {
                i.name,
                i.location,
                i.remark,
                i.state,
                i.status
            }).ToSQL();
            t_ware model = new t_ware
            {
                name = reqmodel.Data.name,
                location = reqmodel.Data.location,
                remark = reqmodel.Data.remark,
                state = (int)EnumState.Normal,
                status = (int)EnumStatus.Enable
            };
            bool insert_flag = await g_dbHelper.ExecAsync(sql_insert, model) > 0;
            if (!insert_flag)
            {
                result.code = ErrorCodeConst.ERROR_1018;
                return result;
            }

            result.code = ErrorCodeConst.ERROR_1019;
            result.status = ErrorCodeConst.ERROR_200;
            return result;
        }

        public async Task<Result> DeleteWareAsync(int id)
        {
            Result result = new Result { code = ErrorCodeConst.ERROR_1030, status = ErrorCodeConst.ERROR_403 };
            string sql_delete = g_sqlMaker.Delete<t_ware>().Where($"id", "=", "@id").And("state", "=", "@state").ToSQL();
            bool delete_flag = await g_dbHelper.ExecAsync(sql_delete, new { id, state = (int)EnumState.Normal }) > 0;
            if (!delete_flag)
            {
                result.code = ErrorCodeConst.ERROR_1030;
                return result;
            }

            result.code = ErrorCodeConst.ERROR_1029;
            result.status = ErrorCodeConst.ERROR_200;
            return result;
        }

        public async Task<Result> DepositStockAsync(reqmodel<StockDepositModel> reqmodel)
        {
            string modelname = @"WareServerImpl.DepositStockAsync";
            Result result = new Result { status = ErrorCodeConst.ERROR_403 };
            //待入库库存
            if (reqmodel.Data.id == 0)
            {
                result.code = ErrorCodeConst.ERROR_1053;
                return result;
            }
            string stock_pre_sql = g_sqlMaker.Select<t_stockin_pre>(s => new { s.id, s.quantity, s.stock_id, s.rv }).Where("id", "=", "@id").ToSQL();
            t_stockin_pre stock_pre = await g_dbHelper.QueryAsync<t_stockin_pre>(stock_pre_sql, new { id = reqmodel.Data.id });
            if (stock_pre == null || stock_pre.quantity <= 0)
            {
                result.code = ErrorCodeConst.ERROR_1053;
                return result;
            }
            if (reqmodel.Data.quantity <= 0)
            {
                result.code = ErrorCodeConst.ERROR_1054;
                return result;
            }
            if (reqmodel.Data.quantity > stock_pre.quantity)
            {
                result.code = ErrorCodeConst.ERROR_1056;
                return result;
            }
            //仓库
            if (reqmodel.Data.ware_id == 0)
            {
                result.code = ErrorCodeConst.ERROR_1050;
                return result;
            }
            string ware_exist_sql = g_sqlMaker.Select<t_ware>().Count().Where("id", "=", "@id").And("status", "=", "@status").And("state", "=", "@state").ToSQL();
            bool ware_exist_flag = await g_dbHelper.QueryAsync<int>(ware_exist_sql, new { id = reqmodel.Data.ware_id, status = (int)EnumStatus.Enable, state = (int)EnumState.Normal }) == 1;
            if (!ware_exist_flag)
            {
                result.code = ErrorCodeConst.ERROR_1050;
                return result;
            }

            t_stock_deposit stock_deposit = new t_stock_deposit
            {
                location = reqmodel.Data.location,
                quantity = reqmodel.Data.quantity,
                remark = reqmodel.Data.remark,
                stock_id = stock_pre.stock_id,
                ware_id = reqmodel.Data.ware_id
            };

            //设置存放位置和预入库数量
            g_dbHelper.Transaction();
            try
            {
                string deposit_sql = g_sqlMaker.Insert<t_stock_deposit>(i => new { i.ware_id, i.stock_id, i.quantity, i.remark, i.location }).ToSQL();
                bool insert_deposit_flag = await g_dbHelper.ExecAsync(deposit_sql, stock_deposit) > 0;
                if (!insert_deposit_flag)
                {
                    g_dbHelper.Rollback();
                    result.code = ErrorCodeConst.ERROR_1030;
                    return result;
                }

                stock_pre.quantity -= reqmodel.Data.quantity;
                bool stockin_pre_flag = false;
                if (stock_pre.quantity == 0)
                {
                    string stock_pre_oper_sql = g_sqlMaker.Delete<t_stockin_pre>().Where("id", "=", "@id").And("rv", "=", "@rv").ToSQL();
                    stockin_pre_flag = await g_dbHelper.ExecAsync(stock_pre_oper_sql, stock_pre) > 0;
                }
                else
                {
                    string stock_pre_oper_sql = g_sqlMaker.Update<t_stockin_pre>(u => new { u.quantity }).Where("id", "=", "@id").And("rv", "=", "@rv").ToSQL();
                    stockin_pre_flag = await g_dbHelper.ExecAsync(stock_pre_oper_sql, stock_pre) > 0;
                }
                if (!stockin_pre_flag)
                {
                    g_dbHelper.Rollback();
                    result.code = ErrorCodeConst.ERROR_1030;
                    return result;
                }

                g_dbHelper.Commit();
                g_logServer.Log(modelname, "安置产品成功", $"用户：{reqmodel.User.user_name}", EnumLogType.Info);
            }
            catch (Exception e)
            {
                g_dbHelper.Rollback();
                g_logServer.Log(modelname, "安置产品异常", $"用户：{reqmodel.User.user_name},信息：{JsonConvert.SerializeObject(e)}", EnumLogType.Error);
            }

            result.code = ErrorCodeConst.ERROR_1029;
            result.status = ErrorCodeConst.ERROR_200;

            return result;
        }

        public async Task<Result> GetAllWares()
        {
            List<t_ware> data_list = await GetAllWares(s => new { s.id, s.location, s.name, s.remark, s.status, s.add_time });
            List<WareResult> result_list = new List<WareResult>();
            foreach (var item in data_list)
            {
                result_list.Add(new WareResult
                {
                    id = item.id,
                    add_time = item.add_time,
                    location = item.location,
                    name = item.name,
                    remark = item.remark,
                    status = item.status,
                });
            }

            Result<IEnumerable<WareResult>> result = new Result<IEnumerable<WareResult>>
            {
                code = ErrorCodeConst.ERROR_200,
                status = ErrorCodeConst.ERROR_200,
                data = result_list
            };
            return result;
        }

        /// <summary>
        /// @lx 获取所有仓库信息
        /// </summary>
        /// <param name="selector">选择器</param>
        /// <param name="normal"></param>
        /// <returns></returns>
        public async Task<List<t_ware>> GetAllWares(Func<t_ware, dynamic> selector, bool normal = true)
        {
            IWhere<t_ware> sql_select = g_sqlMaker.Select(selector).Where();
            if (normal)
            {
                sql_select = sql_select.And("status", "=", "@status").And("state", "=", "@state");
            }
            List<t_ware> data_list = await g_dbHelper.QueryListAsync<t_ware>(sql_select.ToSQL(), new { status = (int)EnumStatus.Enable, state = (int)EnumState.Normal });
            return data_list;
        }

        public async Task<Result> GetAllWaresDrop()
        {
            List<t_ware> data_list = await GetAllWares(s => new { s.name, s.id });
            List<KVItemResult<int, string>> result_list = new List<KVItemResult<int, string>>();
            foreach (var item in data_list)
            {
                result_list.Add(new KVItemResult<int, string>
                {
                    key = item.id,
                    value = item.name
                });
            }

            Result<IEnumerable<KVItemResult<int, string>>> result = new Result<IEnumerable<KVItemResult<int, string>>>
            {
                code = ErrorCodeConst.ERROR_200,
                status = ErrorCodeConst.ERROR_200,
                data = result_list
            };
            return result;
        }
    }
}
