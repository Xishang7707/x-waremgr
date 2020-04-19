using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Attributes;
using api.requests;
using api.Servers.LogServer.Interface;
using api.Servers.WareServer.Impl;
using api.Servers.WareServer.Interface;
using common.DB.Interface;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace api.Controllers
{
    [Route("api/[controller]")]
    public class WareController : BaseController
    {
        public WareController(IDbHelper _dbHelper, ILogServer _logServer) : base(_dbHelper, _logServer) { }

        /// <summary>
        /// @xis 添加仓库 2020-3-29 11:59:43
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("addware")]
        [Privilege("add_ware")]
        public async Task<IActionResult> AddWare([FromBody]AddWareModel model)
        {
            if (!ModelState.IsValid)
            {
                return GetModelErrorCode();
            }

            reqmodel<AddWareModel> reqmodel = await RequestPackingAsync(model);

            IWareServer wareServer = new WareServerImpl(g_dbHelper, g_logServer);
            return await wareServer.AddWareAsync(reqmodel);
        }

        /// <summary>
        /// @xis 删除仓库 2020-3-29 11:59:43
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("deleteware")]
        [Privilege("delete_ware")]
        public async Task<IActionResult> DeleteWare([FromBody]int id)
        {
            IWareServer wareServer = new WareServerImpl(g_dbHelper, g_logServer);
            return await wareServer.DeleteWareAsync(id);
        }

        /// <summary>
        /// @xis 获取所有仓库 2020-3-29 11:59:43
        /// </summary>
        /// <returns></returns>
        [HttpGet("getallwares")]
        public async Task<IActionResult> GetAllWares()
        {
            IWareServer wareServer = new WareServerImpl(g_dbHelper, g_logServer);
            return await wareServer.GetAllWares();
        }

        /// <summary>
        /// @xis 获取所有仓库 --下拉 2020-4-19 16:33:53
        /// </summary>
        /// <returns></returns>
        [HttpGet("getallwaresdrop")]
        public async Task<IActionResult> GetAllWaresDrop()
        {
            IWareServer wareServer = new WareServerImpl(g_dbHelper, g_logServer);
            return await wareServer.GetAllWaresDrop();
        }

        /// <summary>
        /// @xis 安置产品
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("depositstock")]
        public async Task<IActionResult> DepositStock([FromBody]StockDepositModel model)
        {
            if (!ModelState.IsValid)
            {
                return GetModelErrorCode();
            }
            reqmodel<StockDepositModel> reqmodel = await RequestPackingAsync(model);
            IWareServer wareServer = new WareServerImpl(g_dbHelper, g_logServer);
            return await wareServer.DepositStockAsync(reqmodel);
        }
    }
}
