using System.Threading.Tasks;
using api.requests;
using api.Servers.LogServer.Interface;
using api.Servers.StockServer.Impl;
using api.Servers.StockServer.Interface;
using common.DB.Interface;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace api.Controllers
{
    /// <summary>
    /// 库存
    /// </summary>
    [Route("api/[controller]")]
    public class StockController : BaseController
    {
        public StockController(IDbHelper _dbHelper, ILogServer _logServer) : base(_dbHelper, _logServer) { }
        /// <summary>
        /// @xis 入库申请 2020-4-1 22:07:35
        /// </summary>
        /// <returns></returns>
        [HttpPost("stockinapply")]
        public async Task<IActionResult> StockInApply([FromBody]StockInApplyModel model)
        {
            if (!ModelState.IsValid)
            {
                return GetModelErrorCode();
            }
            reqmodel<StockInApplyModel> reqmodel = await RequestPackingAsync(model);
            IStockServer stockServer = new StockServerImpl(g_dbHelper, g_logServer);
            return await stockServer.StockInApplyAsync(reqmodel);
        }

        /// <summary>
        /// @xis 入库审批 2020-4-2 22:09:53
        /// </summary>
        /// <returns></returns>
        [HttpPost("stockinaudit")]
        public async Task<IActionResult> StockInAudit([FromBody]AuditModel model)
        {
            if (!ModelState.IsValid)
            {
                return GetModelErrorCode();
            }
            reqmodel<AuditModel> reqmodel = await RequestPackingAsync(model);
            IStockServer stockServer = new StockServerImpl(g_dbHelper, g_logServer);
            return await stockServer.StockInAuditAsync(reqmodel);
        }

        /// <summary>
        /// @xis 搜索库存 2020-4-6 17:24:01
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet("searchstocktotal")]
        public async Task<IActionResult> SearchStockTotal([FromQuery]SearchStockModel model)
        {
            if (!ModelState.IsValid)
            {
                return GetModelErrorCode();
            }
            reqmodel<SearchStockModel> reqmodel = await RequestPackingAsync(model);
            IStockServer stockServer = new StockServerImpl(g_dbHelper, g_logServer);
            return await stockServer.SearchStockAsync(reqmodel);
        }

        /// <summary>
        /// @xis 搜索入库单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet("searchstockinpaginer")]
        public async Task<IActionResult> SearchStockInPaginer([FromQuery]SearchStockInModel model)
        {
            reqmodel<SearchStockInModel> reqmodel = await RequestPackingAsync(model);
            IStockServer stockServer = new StockServerImpl(g_dbHelper, g_logServer);
            return await stockServer.SearchStockInPaginerAsync(reqmodel);
        }

        /// <summary>
        /// @xis 入库单详情
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet("getstockindetail")]
        public async Task<IActionResult> GetStockInDetail([FromQuery]StockInDetailModel model)
        {
            if (!ModelState.IsValid)
            {
                return GetModelErrorCode();
            }
            reqmodel<StockInDetailModel> reqmodel = await RequestPackingAsync(model);
            IStockServer stockServer = new StockServerImpl(g_dbHelper, g_logServer);
            return await stockServer.GetStockInDetailAsync(reqmodel);
        }

        /// <summary>
        /// @xis 待入库列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet("searchstockprepaginer")]
        public async Task<IActionResult> SearchStockPrePaginer([FromQuery]SearchStockPreModel model)
        {
            reqmodel<SearchStockPreModel> reqmodel = await RequestPackingAsync(model);
            IStockServer stockServer = new StockServerImpl(g_dbHelper, g_logServer);
            return await stockServer.SearchStockPaginerAsync(reqmodel);
        }

        /// <summary>
        /// @xis 库存列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet("searchstockpaginer")]
        public async Task<IActionResult> SearchStockPaginer([FromQuery]StockPaginerModel model)
        {
            reqmodel<StockPaginerModel> reqmodel = await RequestPackingAsync(model);
            IStockServer stockServer = new StockServerImpl(g_dbHelper, g_logServer);
            return await stockServer.GetStockPaginerAsync(reqmodel);
        }


    }
}
