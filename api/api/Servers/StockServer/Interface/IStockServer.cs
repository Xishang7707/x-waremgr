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
        /// @xis 查询入库单
        /// </summary>
        /// <param name="reqmodel"></param>
        /// <returns></returns>
        Task<Result> GetStockInList(reqmodel<QueryStockInModel> reqmodel);
    }
}
