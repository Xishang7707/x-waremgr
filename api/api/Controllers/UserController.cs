using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Attributes;
using api.Externs;
using api.requests;
using api.responses;
using api.Servers.LogServer.Interface;
using api.Servers.UserServer.Impl;
using api.Servers.UserServer.Interface;
using common.DB.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace api.Controllers
{
    [Route("api/[controller]")]
    public class UserController : BaseController
    {
        public UserController(IDbHelper _dbHelper, ILogServer _logServer) : base(_dbHelper, _logServer) { }

        /// <summary>
        /// @xis 添加用户 2020-3-24 20:08:57
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("adduser")]
        [Privilege("add_user")]
        public async Task<IActionResult> AddUser([FromBody]RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return GetModelErrorCode();
            }
            reqmodel<RegisterModel> reqmodel = await RequestPackingAsync(model);
            IUserServer userServer = new UserServerImpl(g_dbHelper, g_logServer);

            return await userServer.AddUserAsync(reqmodel);
        }


        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            return new Result<string>
            {
                code = "200",
                status = "200",
                data = $"当前登录用户：{ (await Request.GetUserAsync()).user_name}"
            };
        }

        /// <summary>
        /// @xis 登录 2020-3-25 13:45:16
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return GetModelErrorCode();
            }
            reqmodel<LoginModel> reqmodel = await RequestPackingAsync(model);
            IUserServer userServer = new UserServerImpl(g_dbHelper, g_logServer);

            return await userServer.LoginAsync(reqmodel);
        }

        /// <summary>
        /// @xis 登出 2020-3-27 13:00:21
        /// </summary>
        /// <returns></returns>
        [HttpPost("loginout")]
        public async Task<IActionResult> LoginOut()
        {
            reqmodel reqmodel = await RequestPackingAsync();
            IUserServer userServer = new UserServerImpl(g_dbHelper, g_logServer);

            return await userServer.LoginOutAsync(reqmodel);
        }

        /// <summary>
        /// @xis 获取用户信息 2020-3-27 13:00:21
        /// </summary>
        /// <returns></returns>
        [HttpGet("getuserinfo")]
        public async Task<IActionResult> GetUserInfo()
        {
            reqmodel reqmodel = await RequestPackingAsync();
            IUserServer userServer = new UserServerImpl(g_dbHelper, g_logServer);

            return await userServer.GetUserInfoAsync(reqmodel);
        }

    }
}
