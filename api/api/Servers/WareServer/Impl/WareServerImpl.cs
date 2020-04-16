using api.requests;
using api.responses;
using api.Servers.LogServer.Interface;
using api.Servers.WareServer.Interface;
using common.Consts;
using common.DB.Interface;
using models.db_models;
using models.enums;
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

        public async Task<Result<IEnumerable<WareResult>>> GetAllWares()
        {
            string sql_select = g_sqlMaker.Select<t_ware>().Where("status", "=", "@status").And("state", "=", "@state").ToSQL();
            List<t_ware> data_list = await g_dbHelper.QueryListAsync<t_ware>(sql_select, new { status = (int)EnumStatus.Enable, state = (int)EnumState.Normal });
            List<WareResult> result_list = new List<WareResult>();
            foreach (var item in data_list)
            {
                result_list.Add(new WareResult(item));
            }

            Result<IEnumerable<WareResult>> result = new Result<IEnumerable<WareResult>>
            {
                code = ErrorCodeConst.ERROR_200,
                status = ErrorCodeConst.ERROR_200,
                data = result_list
            };
            return result;
        }

    }
}
