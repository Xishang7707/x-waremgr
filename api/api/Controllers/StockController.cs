using System.Threading.Tasks;
using api.requests;
using api.Servers.StockServer.Impl;
using api.Servers.StockServer.Interface;
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
            IStockServer stockServer = new StockServerImpl();
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
            IStockServer stockServer = new StockServerImpl();
            return await stockServer.StockInAuditAsync(reqmodel);
        }
    }
}
