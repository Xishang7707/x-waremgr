using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Attributes;
using api.requests;
using api.Servers.WareServer.Impl;
using api.Servers.WareServer.Interface;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace api.Controllers
{
    [Route("api/[controller]")]
    public class WareController : BaseController
    {
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

            IWareServer wareServer = new WareServerImpl();
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
            IWareServer wareServer = new WareServerImpl();
            return await wareServer.DeleteWareAsync(id);
        }

        /// <summary>
        /// @xis 获取所有仓库 2020-3-29 11:59:43
        /// </summary>
        /// <returns></returns>
        [HttpPost("getallwares")]
        public async Task<IActionResult> GetAllWares()
        {
            IWareServer wareServer = new WareServerImpl();
            return await wareServer.GetAllWares();
        }
    }
}
