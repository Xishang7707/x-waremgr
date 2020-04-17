using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Attributes;
using api.requests;
using api.Servers.FactoryServer.Impl;
using api.Servers.FactoryServer.Interface;
using api.Servers.LogServer.Interface;
using common.DB.Interface;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace api.Controllers
{
    [Route("api/[controller]")]
    public class FactoryController : BaseController
    {
        public FactoryController(IDbHelper _dbHelper, ILogServer _logServer) : base(_dbHelper, _logServer) { }

        /// <summary>
        /// @xis 添加供应商 2020-3-28 13:03:04
        /// </summary>
        /// <returns></returns>
        [HttpPost("addfactory")]
        [Privilege("add_factory")]
        public async Task<IActionResult> AddFactory([FromBody]AddFactoryModel model)
        {
            if (!ModelState.IsValid)
            {
                return GetModelErrorCode();
            }
            reqmodel<AddFactoryModel> reqmodel = await RequestPackingAsync(model);
            IFactoryServer factoryServer = new FactoryServerImpl(g_dbHelper, g_logServer);

            return await factoryServer.AddFactory(reqmodel);
        }

        /// <summary>
        /// @xis 删除供货商 2020-3-28 12:36:50
        /// </summary>
        /// <returns></returns>
        [HttpPost("deletefactory")]
        [Privilege("delete_factory")]
        public async Task<IActionResult> DeleteFactory([FromBody]int id)
        {
            reqmodel<int> reqmodel = await RequestPackingAsync(id);
            IFactoryServer factoryServer = new FactoryServerImpl(g_dbHelper, g_logServer);

            return await factoryServer.DeleteFactory(reqmodel);
        }

        /// <summary>
        /// @xis 搜索供货商-下拉 2020-3-28 12:36:50
        /// </summary>
        /// <returns></returns>
        [HttpGet("searchfactorydrop")]
        //[Privilege("searchfactorydrop")]
        public async Task<IActionResult> SearchFactoryDrop([FromQuery]SearchFactoryModel model)
        {
            reqmodel<SearchFactoryModel> reqmodel = await RequestPackingAsync(model);
            IFactoryServer factoryServer = new FactoryServerImpl(g_dbHelper, g_logServer);

            return await factoryServer.SearchFactoryDrop(reqmodel);
        }

        /// <summary>
        /// @xis 搜索供货商 2020-4-17 22:30:57
        /// </summary>
        /// <returns></returns>
        [HttpGet("searchfactorybypaginer")]
        //[Privilege("searchfactorydrop")]
        public async Task<IActionResult> SearchFactoryByPaginer([FromQuery]SearchFactoryModel model)
        {
            reqmodel<SearchFactoryModel> reqmodel = await RequestPackingAsync(model);
            IFactoryServer factoryServer = new FactoryServerImpl(g_dbHelper, g_logServer);

            return await factoryServer.SearchFactoryByPaginer(reqmodel);
        }
    }
}
