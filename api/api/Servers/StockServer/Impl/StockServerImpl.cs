﻿using api.requests;
using api.responses;
using api.Servers.FactoryServer.Impl;
using api.Servers.FactoryServer.Interface;
using api.Servers.ProductServer.Impl;
using api.Servers.ProductServer.Interface;
using api.Servers.StockServer.Interface;
using common;
using common.Consts;
using models.db_models;
using models.enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Servers.StockServer.Impl
{
    /// <summary>
    /// 库存服务
    /// </summary>
    public class StockServerImpl : BaseServiceImpl, IStockServer
    {
        /// <summary>
        /// 订单号生成
        /// </summary>
        /// <param name="prefix">前缀</param>
        /// <returns></returns>
        private string MakeOrderSn(string prefix)
        {
            return prefix + DateTime.Now.ToString("yyyyMMddHHmmssms") + (new Random(DateTime.Now.Millisecond).Next(0, 100000));
        }

        public Task<Result> GetStockInList(reqmodel<QueryStockInModel> reqmodel)
        {
            string sql_stock_in = g_sqlMaker.Select<t_stock_in>().Where("in_user_id", "=", "@user_id").ToSQL();
            return null;
        }

        /// <summary>
        /// @xis 入库申请
        /// </summary>
        /// <param name="reqmodel"></param>
        /// <returns></returns>
        public async Task<Result> StockInApplyAsync(reqmodel<StockInApplyModel> reqmodel)
        {
            const string modelname = "StockServerImpl.StockInApplyAsync";
            Result result = new Result { code = ErrorCodeConst.ERROR_1037, status = ErrorCodeConst.ERROR_403 };

            //入库单
            t_stock_in stock_in = new t_stock_in
            {
                apply_status = (int)EnumApplyStatus.Progress,
                in_user_id = reqmodel.User.user_id,
                department_id = reqmodel.User.department_id,
                position_id = reqmodel.User.position_id,
                status = (int)EnumStatus.Enable,
                order_sn = MakeOrderSn("IN"),
                apply_process = reqmodel.User.position_id,
                add_time = DateTime.Now
            };

            List<t_stock_in_detail> stock_detail_list = new List<t_stock_in_detail>();
            List<t_stock> stock_add_list = new List<t_stock>();
            List<t_stock> stock_update_list = new List<t_stock>();

            #region 数据验证
            if (reqmodel.Data.products.Count == 0)
            {
                result.code = ErrorCodeConst.ERROR_1042;
                return result;
            }

            #endregion

            List<t_stock> stock_info_list = await GetStockByIds(s => new { s.id, s.freeze_quantity, s.rv }, reqmodel.Data.products.Select(s => s.id));
            //检查库存
            foreach (var item in reqmodel.Data.products)
            {
                //更新库存
                t_stock es_stock = stock_info_list.FirstOrDefault(f => f.id == item.id);
                if (es_stock != null)
                {
                    //添加已有的详细信息
                    stock_detail_list.Add(new t_stock_in_detail
                    {
                        order_sn = stock_in.order_sn,
                        price = item.unit_price,
                        stock_id = item.id,
                        quantity = item.quantity
                    });
                    continue;
                }
                //添加新产品
                stock_add_list.Add(new t_stock
                {
                    order_sn = stock_in.order_sn,
                    add_time = DateTime.Now,
                    batch_number = item.batch_number,
                    expiration_date = item.expiration_date,
                    instructions = item.instructions,
                    material_number = item.material_number,
                    model_number = item.model_number,
                    package_size = item.package_size,
                    product_name = item.product_name,
                    report_card_url = item.report_card_url,
                    retest_date = item.retest_date,
                    spare_parts = item.spare_parts,
                    state = (int)EnumState.Normal,
                    status = (int)EnumStatus.Enable,
                    unit_price = item.unit_price,
                    util_name = item.unit_name,
                    quantity = item.quantity,//在详情处清0
                    freeze_quantity = 0,
                    factory_id = item.factory_id
                });
            }

            try
            {
                g_dbHelper.Transaction();
                bool insert_stock_in = await AddStockInAsync(stock_in) > 0;
                if (!insert_stock_in)
                {
                    g_dbHelper.Rollback();
                    result.code = ErrorCodeConst.ERROR_1030;
                    g_logServer.Log(modelname, "入库单申请失败", $"用户：{reqmodel.User.user_name},添加入库单失败", models.enums.EnumLogType.Info);
                    return result;
                }
                if (stock_add_list.Count > 0)
                {
                    foreach (var item in stock_add_list)
                    {
                        decimal quantity = item.quantity;
                        item.quantity = 0;
                        SVResult<int> add_result = await AddStock(item);
                        if (!add_result.state)
                        {
                            g_dbHelper.Rollback();
                            result.code = add_result.code;
                            g_logServer.Log(modelname, "入库单申请失败", $"用户：{reqmodel.User.user_name},添加新库存失败", models.enums.EnumLogType.Info);
                            return result;
                        }

                        stock_detail_list.Add(new t_stock_in_detail
                        {
                            order_sn = stock_in.order_sn,
                            price = item.unit_price,
                            quantity = quantity,
                            stock_id = add_result.data
                        });
                    }
                }

                //if (stock_info_list.Count > 0)
                //{
                //    bool update_stock = await UpdateStock(u => new { u.freeze_quantity }, stock_info_list);
                //    if (!update_stock)
                //    {
                //        g_dbHelper.Rollback();
                //        result.code = ErrorCodeConst.ERROR_1030;
                //        g_logServer.Log(modelname, "入库单申请失败", $"用户：{reqmodel.User.user_name},更新库存失败", models.enums.EnumLogType.Info);
                //        return result;
                //    }
                //}

                SVResult<int> insert_stock_dtl_flag = await AddStockDetails(stock_detail_list);
                if (!insert_stock_dtl_flag.state)
                {
                    g_dbHelper.Rollback();
                    result.code = insert_stock_dtl_flag.code;
                    g_logServer.Log(modelname, "入库单申请失败", $"用户：{reqmodel.User.user_name},更新入库单详情失败", models.enums.EnumLogType.Info);
                    return result;
                }

                g_dbHelper.Commit();
            }
            catch (Exception e)
            {
                g_dbHelper.Rollback();
                g_logServer.Log(modelname, "入库单申请失败", $"用户：{reqmodel.User.user_name},错误信息：{e.Message}", models.enums.EnumLogType.Error);
                return result;
            }

            result.code = ErrorCodeConst.ERROR_1036;
            result.status = ErrorCodeConst.ERROR_200;
            g_logServer.Log(modelname, "入库单申请成功", $"用户：{reqmodel.User.user_name},订单号：{stock_in.order_sn}", models.enums.EnumLogType.Info);
            return result;
        }

        /// <summary>
        /// @xis 入库审批
        /// </summary>
        /// <param name="reqmodel"></param>
        /// <returns></returns>
        public async Task<Result> StockInAuditAsync(reqmodel<AuditModel> reqmodel)
        {
            const string modelname = "StockServerImpl.StockInAuditAsync";
            Result result = new Result { code = ErrorCodeConst.ERROR_1037, status = ErrorCodeConst.ERROR_403 };

            t_stock_in stock_in = await GetStockInByOrderSn(s => new
            {
                s.apply_process,
                s.apply_status,
                s.department_id
            }, reqmodel.Data.order_sn);

            if (stock_in == null)
            {
                result.code = ErrorCodeConst.ERROR_1038;
                g_logServer.Log(modelname, "入库审批失败", $"用户：{reqmodel.User.user_name}，订单不存在", EnumLogType.Info);
                return result;
            }
            stock_in.order_sn = reqmodel.Data.order_sn;

            if (stock_in.apply_status == (int)EnumApplyStatus.Agree)
            {
                result.code = ErrorCodeConst.ERROR_1039;
                g_logServer.Log(modelname, "入库审批失败", $"用户：{reqmodel.User.user_name},订单号：{stock_in.order_sn}，订单被审核完成", EnumLogType.Info);
                return result;
            }
            if (stock_in.apply_status == (int)EnumApplyStatus.Reject)
            {
                result.code = ErrorCodeConst.ERROR_1041;
                g_logServer.Log(modelname, "入库审批失败", $"用户：{reqmodel.User.user_name},订单号：{stock_in.order_sn}，订单被驳回", EnumLogType.Info);
                return result;
            }
            int? next_applyer = GetNextApplyer(EnumOrderType.IN, stock_in.department_id, stock_in.apply_process);
            if (next_applyer == null)
            {
                result.code = ErrorCodeConst.ERROR_1030;
                g_logServer.Log(modelname, "入库审批失败", $"用户：{reqmodel.User.user_name},订单号：{stock_in.order_sn}，流程错误", EnumLogType.Info);
                return result;
            }

            if (next_applyer != reqmodel.User.position_id)
            {
                result.code = ErrorCodeConst.ERROR_1035;
                g_logServer.Log(modelname, "入库审批失败", $"用户：{reqmodel.User.user_name},订单号：{stock_in.order_sn}，无法进行审批", EnumLogType.Info);
                return result;
            }

            t_apply_log apply_log = new t_apply_log
            {
                apply_status = reqmodel.Data.act,
                order_sn = reqmodel.Data.order_sn,
                user_id = reqmodel.User.user_id,
                position_id = reqmodel.User.position_id
            };
            stock_in.apply_process = reqmodel.User.position_id;
            try
            {
                g_dbHelper.Transaction();
                //审批记录更新
                bool insert_apply_log_flag = await AddApplyLog(apply_log);
                if (!insert_apply_log_flag)
                {
                    g_dbHelper.Rollback();
                    result.code = ErrorCodeConst.ERROR_1030;
                    g_logServer.Log(modelname, "入库审批失败", $"用户：{reqmodel.User.user_name},订单号：{stock_in.order_sn}，添加审批记录失败", models.enums.EnumLogType.Info);
                    return result;
                }

                //审批状态更新
                bool update_stock_in_flag;
                if (reqmodel.Data.act == 1)
                {
                    stock_in.apply_status = (int)EnumApplyStatus.Agree;
                }
                else
                {
                    //驳回
                    stock_in.apply_status = (int)EnumApplyStatus.Reject;
                }

                //最后一步或者拒绝则更新审批状态
                EnumApplyStepFlag step_flag = GetApplyStepFlag(EnumOrderType.IN, stock_in.department_id, reqmodel.User.position_id);
                if (step_flag == EnumApplyStepFlag.End || stock_in.apply_status == (int)EnumApplyStatus.Reject)
                {
                    update_stock_in_flag = await UpdateStockInByOrderSn(u => new { u.apply_status, u.apply_process }, stock_in);
                }
                else
                {
                    update_stock_in_flag = await UpdateStockInByOrderSn(u => new { u.apply_process }, stock_in);
                }

                if (!update_stock_in_flag)
                {
                    g_dbHelper.Rollback();
                    result.code = ErrorCodeConst.ERROR_1030;
                    g_logServer.Log(modelname, "入库审批失败", $"用户：{reqmodel.User.user_name},订单号：{stock_in.order_sn}，更新审批状态失败", models.enums.EnumLogType.Info);
                    return result;
                }

                //同意并且是最后一步审批，更新库存数量
                if (step_flag == EnumApplyStepFlag.End && stock_in.apply_status == (int)EnumApplyStatus.Agree)
                {
                    bool update_quality_flag = await StockInApplySuccess(stock_in.order_sn);
                    if (!update_quality_flag)
                    {
                        g_dbHelper.Rollback();
                        result.code = ErrorCodeConst.ERROR_1030;
                        g_logServer.Log(modelname, "入库审批失败", $"用户：{reqmodel.User.user_name},订单号：{stock_in.order_sn}，更新库存数量失败", models.enums.EnumLogType.Info);
                        return result;
                    }
                }

                g_dbHelper.Commit();
            }
            catch (Exception ex)
            {
                g_dbHelper.Rollback();
                result.code = ErrorCodeConst.ERROR_1030;
                g_logServer.Log(modelname, "入库审批异常", $"用户：{reqmodel.User.user_name},订单号：{stock_in.order_sn},异常信息：{ex.Message}", models.enums.EnumLogType.Error);
                return result;
            }
            result.code = ErrorCodeConst.ERROR_1029;
            result.status = ErrorCodeConst.ERROR_200;
            g_logServer.Log(modelname, "入库审批成功", $"用户：{reqmodel.User.user_name},订单号：{stock_in.order_sn}", models.enums.EnumLogType.Info);
            return result;
        }

        /// <summary>
        /// @xis 获取下一个审批者
        /// </summary>
        /// <param name="_ot">订单类型</param>
        /// <param name="_depart">申请部门</param>
        /// <param name="_cur_position">当前已审批的职位</param>
        /// <returns>
        /// null：获取失败
        /// -1：已达末尾
        /// </returns>
        public int? GetNextApplyer(EnumOrderType _ot, int _depart, int _cur_position)
        {
            JObject process_json = CommonConfig.ProcessConfig[_ot.ToString()] as JObject;
            if (process_json == null)
            {
                return null;
            }

            List<int> process_list = JsonConvert.DeserializeObject<List<int>>(process_json[$"d_{_depart}"].ToString());//流程列表
            if (process_list == null)
            {
                return null;
            }

            int cur_index = process_list.IndexOf(_cur_position);
            //不存在或达末尾
            if (cur_index == -1 || cur_index + 1 >= process_list.Count)
            {
                return null;
            }

            return process_list[cur_index + 1];
        }

        /// <summary>
        /// @xis 审批进度 开始/进行中/末尾 EnumApplyStepFlag
        /// </summary>
        /// <returns></returns>
        public EnumApplyStepFlag GetApplyStepFlag(EnumOrderType _ot, int _depart, int _cur_position)
        {
            List<int> process_list = JsonConvert.DeserializeObject<List<int>>(CommonConfig.ProcessConfig[_ot.ToString()][$"d_{_depart}"].ToString());
            int cur_index = process_list.IndexOf(_cur_position);
            if (cur_index == 0)
            {
                return EnumApplyStepFlag.Start;
            }
            if (cur_index + 1 != process_list.Count)
            {
                return EnumApplyStepFlag.Progress;
            }

            return EnumApplyStepFlag.End;
        }

        /// <summary>
        /// @xis 添加入库单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> AddStockInAsync(t_stock_in model)
        {
            string sql_insert_stock_in = g_sqlMaker.Insert<t_stock_in>(i => new
            {
                i.in_user_id,
                i.order_sn,
                i.position_id,
                i.status,
                i.department_id,
                i.apply_status,
                i.apply_process
            }).ToSQL();
            return await g_dbHelper.ExecScalarAsync<int>(sql_insert_stock_in, model);
        }

        /// <summary>
        /// @xis 添加库存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<SVResult<int>> AddStock(t_stock model)
        {
            SVResult<int> res = new SVResult<int> { state = false };

            string sql_insert = g_sqlMaker.Insert<t_stock>(i => new
            {
                i.batch_number,
                i.expiration_date,
                i.factory_id,
                i.freeze_quantity,
                i.instructions,
                i.material_number,
                i.model_number,
                i.order_sn,
                i.package_size,
                i.product_name,
                i.quantity,
                i.remark,
                i.report_card_url,
                i.retest_date,
                i.spare_parts,
                i.state,
                i.status,
                i.unit_price,
                i.util_name
            }).ToSQL();
            //验证
            if (string.IsNullOrWhiteSpace(model.product_name))
            {
                res.code = ErrorCodeConst.ERROR_1044;
                return res;
            }

            if (model.product_name.Length > 30)
            {
                res.code = ErrorCodeConst.ERROR_1046;
                return res;
            }

            if (model.factory_id <= 0)
            {
                res.code = ErrorCodeConst.ERROR_1045;
                return res;
            }
            IFactoryServer factoryServer = new FactoryServerImpl(g_dbHelper, g_logServer);
            t_factory factory_model = await factoryServer.GetFactoryById(g => new { g.id }, model.factory_id);
            if (factory_model == null)
            {
                res.code = ErrorCodeConst.ERROR_1045;
                return res;
            }

            int id = await g_dbHelper.ExecScalarAsync<int>(sql_insert, model);
            if (id <= 0)
            {
                res.code = ErrorCodeConst.ERROR_1018;
                return res;
            }
            res.data = id;
            res.state = true;
            return res;
        }

        /// <summary>
        /// @xis 更新库存
        /// </summary>
        /// <param name="update"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<bool> UpdateStock(Func<t_stock, dynamic> update, t_stock data)
        {
            //更新库存
            string sql_update_stock_quelity_list = g_sqlMaker.Update(update).Where("id", "=", "@id").And("rv", "=", "@rv").ToSQL();
            return await g_dbHelper.ExecAsync(sql_update_stock_quelity_list, data) > 0;
        }

        /// <summary>
        /// @xis 更新库存
        /// </summary>
        /// <param name="update"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<bool> UpdateStock(Func<t_stock, dynamic> update, IEnumerable<t_stock> data)
        {
            //更新库存
            string sql_update_stock_quelity_list = g_sqlMaker.Update(update).Where("id", "=", "@id").And("rv", "=", "@rv").ToSQL();
            return await g_dbHelper.ExecAsync(sql_update_stock_quelity_list, data) == data.Count();
        }

        /// <summary>
        /// @xis 添加入库单详情
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<SVResult<int>> AddStockDetails(t_stock_in_detail model)
        {
            SVResult<int> res = new SVResult<int> { state = false };

            //添加的产品为冻结
            if (model.quantity <= 0)
            {
                res.code = ErrorCodeConst.ERROR_1043;
                return res;
            }

            string sql = g_sqlMaker.Insert<t_stock_in_detail>(i => new
            {
                i.order_sn,
                i.price,
                i.quantity,
                i.stock_id
            }).ToSQL();

            int id = await g_dbHelper.ExecScalarAsync<int>(sql, model);
            if (id <= 0)
            {
                res.code = ErrorCodeConst.ERROR_1018;
                return res;
            }

            res.state = true;
            res.data = id;
            return res;
        }

        /// <summary>
        /// @xis 添加入库单详情
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<SVResult<int>> AddStockDetails(IEnumerable<t_stock_in_detail> model)
        {
            SVResult<int> res = new SVResult<int> { state = false };
            if (model.Any(a => a.quantity <= 0))
            {
                res.code = ErrorCodeConst.ERROR_1043;
                return res;
            }
            string sql = g_sqlMaker.Insert<t_stock_in_detail>(i => new
            {
                i.order_sn,
                i.price,
                i.quantity,
                i.stock_id
            }).ToSQL();

            int count = await g_dbHelper.ExecAsync(sql, model);
            if (count != model.Count())
            {
                res.code = ErrorCodeConst.ERROR_1018;
                return res;
            }

            res.state = true;
            res.data = count;
            return res;
        }

        /// <summary>
        /// @xis 获取库存列表
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<List<t_stock>> GetStockByIds(Func<t_stock, dynamic> selector, IEnumerable<int> ids)
        {
            string sql_select_stock_info = g_sqlMaker.Select(selector).Where("id", "in", "@ids").ToSQL();
            return await g_dbHelper.QueryListAsync<t_stock>(sql_select_stock_info, new { ids });
        }

        /// <summary>
        /// @xis 获取入库单
        /// </summary>
        /// <param name="selector">数据列</param>
        /// <param name="order_sn">订单号</param>
        /// <returns></returns>
        public async Task<t_stock_in> GetStockInByOrderSn(Func<t_stock_in, dynamic> selector, string order_sn)
        {
            string sql_select_stock_info = g_sqlMaker.Select(selector).Where("order_sn", "=", "@order_sn").And("status", "=", "@status").ToSQL();
            return await g_dbHelper.QueryAsync<t_stock_in>(sql_select_stock_info, new { order_sn, status = (int)EnumStatus.Enable });
        }

        /// <summary>
        /// @xis 添加审批记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddApplyLog(t_apply_log model)
        {
            string sql = g_sqlMaker.Insert<t_apply_log>(i => new
            {
                i.order_sn,
                i.position_id,
                i.remark,
                i.user_id,
                i.apply_status
            }).ToSQL();
            return await g_dbHelper.ExecAsync(sql, model) > 0;
        }

        /// <summary>
        /// @xis 更新入库单
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateStockInByOrderSn(Func<t_stock_in, dynamic> selector, t_stock_in model)
        {
            string sql = g_sqlMaker.Update(selector).Where("order_sn", "=", "@order_sn").ToSQL();
            return await g_dbHelper.ExecAsync(sql, model) > 0;
        }

        /// <summary>
        /// @xis 获取入库单详情
        /// </summary>
        /// <param name="order_sn">订单号</param>
        /// <returns></returns>
        public async Task<List<t_stock_in_detail>> GetStockInDetailsByOrderSn(Func<t_stock_in_detail, dynamic> selector, string order_sn)
        {
            string sql = g_sqlMaker.Select(selector).Where("order_sn", "=", "@order_sn").ToSQL();
            return await g_dbHelper.QueryListAsync<t_stock_in_detail>(sql, new { order_sn });
        }

        /// <summary>
        /// @xis 入库单审核通过
        /// </summary>
        /// <param name="order_sn">订单号</param>
        /// <returns></returns>
        public async Task<bool> StockInApplySuccess(string order_sn)
        {
            List<t_stock_in_detail> detail_list = await GetStockInDetailsByOrderSn(s => new { s.stock_id, s.quantity }, order_sn);
            List<t_stock> stock_list = await GetStockByIds(s => new { s.id, s.quantity, s.rv }, detail_list.Select(s => s.stock_id));
            foreach (var item in stock_list)
            {
                t_stock_in_detail detail = detail_list.FirstOrDefault(f => f.stock_id == item.id);
                if (detail == null)
                {
                    return false;
                }

                item.quantity += detail.quantity;
            }

            return await UpdateStock(u => new { u.quantity }, stock_list);
        }
    }
}
