using api.requests;
using api.responses;
using models.db_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Servers.StockServer.Interface
{
    /// <summary>
    /// 库存服务
    /// </summary>
    public interface IStockServer
    {
        /// <summary>
        /// @xis 入库申请
        /// </summary>
        /// <param name="reqmodel"></param>
        /// <returns></returns>
        Task<Result> StockInApplyAsync(reqmodel<StockInApplyModel> reqmodel);

        /// <summary>
        /// @xis 入库审批
        /// </summary>
        /// <param name="reqmodel"></param>
        /// <returns></returns>
        Task<Result> StockInAuditAsync(reqmodel<AuditModel> reqmodel);

        /// <summary>
        /// @xis 搜索库存
        /// </summary>
        /// <param name="reqmodel"></param>
        /// <returns></returns>
        Task<Result> SearchStockAsync(reqmodel<SearchStockModel> reqmodel);

        /// <summary>
        /// @xis 搜索入库单
        /// </summary>
        /// <param name="reqmodel"></param>
        /// <returns></returns>
        Task<Result> SearchStockInPaginerAsync(reqmodel<SearchStockInModel> reqmodel);

        /// <summary>
        /// @xis 获取入库单详情
        /// </summary>
        /// <param name="reqmodel"></param>
        /// <returns></returns>
        Task<Result> GetStockInDetailAsync(reqmodel<StockInDetailModel> reqmodel);

        /// <summary>
        /// @xis 待入库列表
        /// </summary>
        /// <param name="reqmodel"></param>
        /// <returns></returns>
        Task<Result> SearchStockPaginerAsync(reqmodel<SearchStockPreModel> reqmodel);
    }
}
