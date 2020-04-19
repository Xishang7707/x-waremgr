using api.requests;
using api.responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Servers.WareServer.Interface
{
    /// <summary>
    /// 仓库
    /// </summary>
    public interface IWareServer
    {
        /// <summary>
        /// @xis 添加仓库
        /// </summary>
        /// <param name="reqmodel"></param>
        /// <returns></returns>
        Task<Result> AddWareAsync(reqmodel<AddWareModel> reqmodel);

        /// <summary>
        /// @xis 删除仓库
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Result> DeleteWareAsync(int id);

        /// <summary>
        /// @xis 获取所有仓库
        /// </summary>
        /// <returns></returns>
        Task<Result> GetAllWares();

        /// <summary>
        /// @xis 获取所有仓库 --下拉
        /// </summary>
        /// <returns></returns>
        Task<Result> GetAllWaresDrop();

        /// <summary>
        /// @xis 放置货物
        /// </summary>
        /// <param name="reqmodel"></param>
        /// <returns></returns>
        Task<Result> DepositStockAsync(reqmodel<StockDepositModel> reqmodel);
    }
}
