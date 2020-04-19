using api.Externs;
using api.requests;
using api.responses;
using api.Servers.AuditServer.Impl;
using api.Servers.AuditServer.Interface;
using api.Servers.DepartmentServer.Impl;
using api.Servers.DepartmentServer.Interface;
using api.Servers.FactoryServer.Impl;
using api.Servers.FactoryServer.Interface;
using api.Servers.LogServer.Interface;
using api.Servers.PositionServer.Impl;
using api.Servers.PositionServer.Interface;
using api.Servers.StockServer.Interface;
using api.Servers.UserServer.Impl;
using api.Servers.UserServer.Interface;
using common.Consts;
using common.DB.Interface;
using common.SqlMaker.Interface;
using models.db_models;
using models.enums;
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
        public StockServerImpl(IDbHelper dbHelper = null, ILogServer logServer = null) : base(dbHelper, logServer) { }

        /// <summary>
        /// 订单号生成
        /// </summary>
        /// <param name="prefix">前缀</param>
        /// <returns></returns>
        private string MakeOrderSn(string prefix)
        {
            return prefix + DateTime.Now.ToString("yyyyMMddHHmmssms") + (new Random(DateTime.Now.Millisecond).Next(0, 100000));
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
                        unit_price = item.unit_price,
                        stock_id = item.id,
                        quantity = item.quantity,
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
                        unit_name = item.unit_name,
                        factory_id = item.factory_id
                    });
                    continue;
                }
                //添加新产品
                stock_add_list.Add(new t_stock
                {
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
                            stock_id = add_result.data,
                            batch_number = item.batch_number,
                            expiration_date = item.expiration_date,
                            factory_id = item.factory_id,
                            instructions = item.instructions,
                            material_number = item.material_number,
                            model_number = item.model_number,
                            package_size = item.package_size,
                            product_name = item.product_name,
                            report_card_url = item.report_card_url,
                            retest_date = item.retest_date,
                            spare_parts = item.spare_parts,
                            unit_name = item.util_name,
                            quantity = quantity,
                            unit_price = item.unit_price,
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
            IAuditServer auditServer = new AuditServerImpl(g_dbHelper, g_logServer);
            int? next_applyer = auditServer.GetNextApplyer(EnumOrderType.IN, stock_in.department_id, stock_in.apply_process);
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
                EnumApplyStepFlag step_flag = auditServer.GetApplyStepFlag(EnumOrderType.IN, stock_in.department_id, reqmodel.User.position_id);
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
            t_factory factory_model = await factoryServer.GetFactoryByIdEnable(g => new { g.id }, model.factory_id);
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
            string sql_update_stock_quantity_list = g_sqlMaker.Update(update).Where("id", "=", "@id").And("rv", "=", "@rv").ToSQL();
            return await g_dbHelper.ExecAsync(sql_update_stock_quantity_list, data) == data.Count();
        }

        /// <summary>
        /// @xis 增加待入库货物
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<bool> AddStockPre(Func<t_stockin_pre, dynamic> selector, IEnumerable<t_stockin_pre> data)
        {
            //增加待入库
            string sql_insert_stockin_pre_list = g_sqlMaker.Insert(selector).ToSQL();
            return await g_dbHelper.ExecAsync(sql_insert_stockin_pre_list, data) == data.Count();
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
                i.unit_price,
                i.quantity,
                i.stock_id,
                i.batch_number,
                i.expiration_date,
                i.factory_id,
                i.instructions,
                i.material_number,
                i.model_number,
                i.package_size,
                i.product_name,
                i.report_card_url,
                i.retest_date,
                i.spare_parts,
                i.unit_name
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
                i.unit_price,
                i.quantity,
                i.stock_id,
                i.batch_number,
                i.expiration_date,
                i.factory_id,
                i.instructions,
                i.material_number,
                i.model_number,
                i.package_size,
                i.product_name,
                i.report_card_url,
                i.retest_date,
                i.spare_parts,
                i.unit_name
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
            List<t_stockin_pre> pre_list = new List<t_stockin_pre>();//待入库
            foreach (var item in stock_list)
            {
                t_stock_in_detail detail = detail_list.FirstOrDefault(f => f.stock_id == item.id);
                if (detail == null)
                {
                    return false;
                }

                item.quantity += detail.quantity;

                pre_list.Add(new t_stockin_pre
                {
                    stock_id = detail.stock_id,
                    quantity = detail.quantity
                });
            }
            //更新库存数量
            bool stock_flag = await UpdateStock(u => new { u.quantity }, stock_list);
            if (!stock_flag)
            {
                return false;
            }
            //货物待入库
            bool pre_flag = await AddStockPre(a => new { a.stock_id, a.quantity }, pre_list);
            return pre_flag;
        }

        /// <summary>
        /// @xis 搜索库存
        /// </summary>
        /// <param name="reqmodel"></param>
        /// <returns></returns>
        public async Task<Result> SearchStockAsync(reqmodel<SearchStockModel> reqmodel)
        {
            Result<IEnumerable<SearchStockResult>> result = new Result<IEnumerable<SearchStockResult>> { code = ErrorCodeConst.ERROR_200, status = ErrorCodeConst.ERROR_200 };
            List<t_stock> stock_list = await GetStockHasByVagueName(s => new { s.id, s.product_name, s.quantity }, reqmodel.Data.name, reqmodel.Data.count);
            IEnumerable<SearchStockResult> result_list = stock_list.GroupBy(g => g.product_name).Select(s => new SearchStockResult { name = s.Key, quantity = stock_list.Where(w => w.product_name == s.Key).Sum(sm => sm.quantity) });
            result.data = result_list;
            return result;
        }

        /// <summary>
        /// @xis 模糊查询库存
        /// </summary>
        /// <param name="selector">列选择器</param>
        /// <param name="name">产品名称</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public async Task<List<t_stock>> GetStockHasByVagueName(Func<t_stock, dynamic> selector, string name, int count = 5)
        {
            string sql = g_sqlMaker.Select(selector)
                                   .Top(count)
                                   .Where("product_name", "like", "@name")
                                   .And("quantity", ">", "@quantity")
                                   .And("status", "=", "@status")
                                   .And("state", "=", "@state")
                                   .ToSQL();

            return await g_dbHelper.QueryListAsync<t_stock>(sql, new { name = $"%{name}%", quantity = 0, status = (int)EnumStatus.Enable, state = (int)EnumState.Normal });
        }

        /// <summary>
        /// @xis 获取入库单产品
        /// </summary>
        /// <param name="selector">列选择器</param>
        /// <param name="order_sn">订单号</param>
        /// <returns></returns>
        public async Task<List<t_stock>> GetStockByOrderSn(Func<t_stock, dynamic> selector, string order_sn)
        {
            string sql = g_sqlMaker.Select(selector)
                                   .Where("order_sn", "=", "@order_sn")
                                   .ToSQL();

            return await g_dbHelper.QueryListAsync<t_stock>(sql, new { order_sn });
        }

        /// <summary>
        /// @xis 获取入库单产品详情
        /// </summary>
        /// <param name="selector">列选择器</param>
        /// <param name="order_sn">订单号</param>
        /// <returns></returns>
        public async Task<List<t_stock_in_detail>> GetStockDetailsByOrderSn(Func<t_stock_in_detail, dynamic> selector, string order_sn)
        {
            string sql = g_sqlMaker.Select(selector)
                                   .Where("order_sn", "=", "@order_sn")
                                   .ToSQL();

            return await g_dbHelper.QueryListAsync<t_stock_in_detail>(sql, new { order_sn });
        }

        /// <summary>
        /// @xis 搜索入库单
        /// </summary>
        /// <param name="reqmodel"></param>
        /// <returns></returns>
        public async Task<Result> SearchStockInPaginerAsync(reqmodel<SearchStockInModel> reqmodel)
        {
            PaginerData<List<t_stock_in>> order_list = await GetStockHasByVagueOrderSn(s => new
            {
                s.in_user_id,
                s.order_sn,
                s.add_time,
                s.apply_process,
                s.apply_status,
                s.department_id,
                s.position_id
            }, reqmodel.Data.order_sn, reqmodel.User.position_id, reqmodel.Data.page_index, reqmodel.Data.page_size);

            Result<PaginerData<List<SearchStockInResult>>> result = new Result<PaginerData<List<SearchStockInResult>>> { status = ErrorCodeConst.ERROR_200, code = ErrorCodeConst.ERROR_200 };
            PaginerData<List<SearchStockInResult>> result_paginer = new PaginerData<List<SearchStockInResult>> { page_index = order_list.page_index, page_size = order_list.page_size, page_total = order_list.page_total, total = order_list.total, Data = new List<SearchStockInResult>() };
            IAuditServer auditServer = new AuditServerImpl(g_dbHelper, g_logServer);
            IUserServer userServer = new UserServerImpl(g_dbHelper, g_logServer);
            IDepartmentServer departmentServer = new DepartmentServerImpl(g_dbHelper, g_logServer);
            foreach (var item in order_list.Data)
            {
                t_user user_model = await userServer.GetUserById(s => new { s.real_name, s.job_number }, item.in_user_id);
                t_department depart_model = await departmentServer.GetDepartment(s => new { s.department_name }, item.department_id);
                result_paginer.Data.Add(new SearchStockInResult
                {
                    add_time = item.add_time.Value.ToString("yyyy-MM-dd hh:mm:ss"),
                    applyer = user_model.real_name,
                    apply_status = item.apply_status,
                    apply_status_desc = ((EnumApplyStatus)item.apply_status).GetDesc(),
                    job_number = user_model.job_number,
                    order_sn = item.order_sn,
                    depart_name = depart_model.department_name,
                    audit_list = await auditServer.GetApplyLogByOrderSnAsync(EnumOrderType.IN, item.order_sn, item.department_id, item.position_id),
                    audit_step_index = auditServer.GetApplyIndex(EnumOrderType.IN, item.department_id, item.position_id, item.apply_process),//获取审批到第几步
                    op_audit = (auditServer.GetNextApplyer(EnumOrderType.IN, item.department_id, item.apply_process) == reqmodel.User.position_id) && item.apply_status == (int)EnumApplyStatus.Progress
                });
            }
            result.data = result_paginer;
            return result;
        }

        /// <summary>
        /// @xis 模糊查询入库单
        /// </summary>
        /// <param name="selector">列选择器</param>
        /// <param name="order_sn">订单号</param>
        /// <param name="position_id">职位id</param>
        /// <param name="page_index">页码</param>
        /// <param name="page_size">数量</param>
        /// <returns></returns>
        public async Task<PaginerData<List<t_stock_in>>> GetStockHasByVagueOrderSn(Func<t_stock_in, dynamic> selector, string order_sn, int position_id, int page_index, int page_size = 15)
        {
            ISelect<t_stock_in> select = g_sqlMaker.Select(selector);
            ISelect<t_stock_in> select_count = g_sqlMaker.Select<t_stock_in>(null);
            IWhere<t_stock_in> where_data;
            IWhere<t_stock_in> where_count;
            if (!string.IsNullOrWhiteSpace(order_sn))
            {
                where_data = select.Where("order_sn", "like", "@order_sn");
                where_count = select_count.Count().Where("order_sn", "like", "@order_sn");
            }
            else
            {
                where_data = select.Where();
                where_count = select_count.Count().Where();
            }
            string sql_data = where_data
                                   .And("position_id", "in", "@position_ids")
                                   .And("status", "=", "@status")
                                   .OrderByDesc("add_time")
                                   .Pager(page_index, page_size)
                                   .ToSQL();

            string sql_count = where_count
                                   .And("status", "=", "@status")
                                   .ToSQL();

            IPositionServer positionServer = new PositionServerImpl(g_dbHelper, g_logServer);
            List<int> position_ids = (await positionServer.GetSubordinatePositions(s => new { s.id }, position_id)).Select(s => s.id).ToList();
            position_ids.Insert(0, position_id);
            PaginerData<List<t_stock_in>> paginer_data = new PaginerData<List<t_stock_in>>
            {
                Data = await g_dbHelper.QueryListAsync<t_stock_in>(sql_data, new { order_sn = $"%{order_sn}%", position_ids = position_ids, status = (int)EnumStatus.Enable, state = (int)EnumState.Normal }),
                page_index = page_index,
                page_size = page_size,
                total = await g_dbHelper.QueryAsync<int>(sql_count, new { order_sn = $"%{order_sn}%", status = (int)EnumStatus.Enable, state = (int)EnumState.Normal })
            };
            paginer_data.page_total = (paginer_data.total % page_size > 0 ? 1 : 0) + paginer_data.total / page_size;
            return paginer_data;
        }

        /// <summary>
        /// @xis 获取入库单详情
        /// </summary>
        /// <param name="reqmodel"></param>
        /// <returns></returns>
        public async Task<Result> GetStockInDetailAsync(reqmodel<StockInDetailModel> reqmodel)
        {
            Result<StockInDetailResult> result = new Result<StockInDetailResult> { code = ErrorCodeConst.ERROR_100, status = ErrorCodeConst.ERROR_403 };
            t_stock_in stock_in_model = await GetStockInByOrderSn(f => new
            {
                f.in_user_id,
                f.department_id,
                f.add_time,
                f.apply_status,
                f.position_id,
                f.order_sn
            }, reqmodel.Data.order_sn);

            if (stock_in_model == null)
            {
                result.code = ErrorCodeConst.ERROR_1038;
                return result;
            }

            IUserServer userServer = new UserServerImpl(g_dbHelper, g_logServer);
            IDepartmentServer departmentServer = new DepartmentServerImpl(g_dbHelper, g_logServer);
            IAuditServer auditServer = new AuditServerImpl(g_dbHelper, g_logServer);

            t_user user_model = await userServer.GetUserById(s => new { s.real_name, s.job_number }, stock_in_model.in_user_id);
            t_department depart_model = await departmentServer.GetDepartment(s => new { s.department_name }, stock_in_model.department_id);
            result.data = new StockInDetailResult
            {
                add_time = stock_in_model.add_time.Value.ToString("yyyy-MM-dd hh:mm:ss") ?? "",
                applyer = user_model.real_name,
                apply_status = stock_in_model.apply_status,
                apply_status_desc = ((EnumApplyStatus)stock_in_model.apply_status).GetDesc(),
                audit_step_index = auditServer.GetApplyIndex(EnumOrderType.IN, stock_in_model.department_id, stock_in_model.position_id, stock_in_model.apply_process),
                job_number = user_model.job_number,
                order_sn = stock_in_model.order_sn,
                depart_name = depart_model.department_name,
                audit_list = await auditServer.GetApplyedLogByOrderSnAsync(EnumOrderType.IN, stock_in_model.order_sn, stock_in_model.department_id, stock_in_model.position_id),
                products = new List<StockInProductResult>()
            };

            //获取入库的库存信息
            List<t_stock_in_detail> stock_detail_list = await GetStockDetailsByOrderSn(f => new t_stock_in_detail { }, stock_in_model.order_sn);
            IFactoryServer factoryServer = new FactoryServerImpl(g_dbHelper, g_logServer);

            foreach (var item in stock_detail_list)
            {
                result.data.products.Add(new StockInProductResult
                {
                    batch_number = item.batch_number,
                    expiration_date = item.expiration_date,
                    factory_id = item.factory_id,
                    instructions = item.instructions,
                    material_number = item.material_number,
                    model_number = item.model_number,
                    package_size = item.package_size,
                    product_name = item.product_name,
                    report_card_url = item.report_card_url,
                    retest_date = item.retest_date,
                    spare_parts = item.spare_parts,
                    unit_name = item.unit_name,
                    quantity = item.quantity,
                    unit_price = item.unit_price,
                    factory_name = (await factoryServer.GetFactoryById(f => new { f.factory_name }, item.factory_id)).factory_name
                });
            }
            result.code = ErrorCodeConst.ERROR_200;
            result.status = ErrorCodeConst.ERROR_200;
            return result;
        }
    }
}
