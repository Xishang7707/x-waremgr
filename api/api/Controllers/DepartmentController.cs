using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Attributes;
using api.requests;
using api.responses;
using api.Servers.DepartmentServer.Impl;
using api.Servers.DepartmentServer.Interface;
using api.Servers.LogServer.Interface;
using api.Servers.UserServer.Impl;
using api.Servers.UserServer.Interface;
using common.DB.Interface;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace api.Controllers
{
    /// <summary>
    /// 部门
    /// </summary>
    [Route("api/[controller]")]
    public class DepartmentController : BaseController
    {
        public DepartmentController(IDbHelper _dbHelper, ILogServer _logServer) : base(_dbHelper, _logServer) { }

        /// <summary>
        /// @xis 添加部门 2020-3-28 12:36:50
        /// </summary>
        /// <returns></returns>
        [HttpPost("adddepartment")]
        [Privilege("add_department")]
        public async Task<IActionResult> AddDepartment([FromBody]AddDepartmentModel model)
        {
            if (!ModelState.IsValid)
            {
                return GetModelErrorCode();
            }
            reqmodel<AddDepartmentModel> reqmodel = await RequestPackingAsync(model);
            IDepartmentServer departmentServer = new DepartmentServerImpl(g_dbHelper, g_logServer);

            return await departmentServer.AddDepartment(reqmodel);
        }

        /// <summary>
        /// @xis 删除部门 2020-3-28 12:36:50
        /// </summary>
        /// <returns></returns>
        [HttpPost("deletedepartment")]
        [Privilege("delete_department")]
        public async Task<IActionResult> DeleteDepartment([FromBody]int id)
        {
            reqmodel<int> reqmodel = await RequestPackingAsync(id);
            IDepartmentServer departmentServer = new DepartmentServerImpl(g_dbHelper, g_logServer);

            return await departmentServer.DeleteDepartment(reqmodel);
        }

        /// <summary>
        /// @xis 获取部门列表型 2020-3-28 12:36:50
        /// </summary>
        /// <returns></returns>
        [HttpGet("getalldepartments")]
        public async Task<IActionResult> GetAllDepartments()
        {
            IDepartmentServer departmentServer = new DepartmentServerImpl(g_dbHelper, g_logServer);

            return await departmentServer.GetAllDepartments();
        }

        /// <summary>
        /// @xis 获取部门深度型 2020-3-28 12:36:50
        /// </summary>
        /// <returns></returns>
        [HttpGet("getalldepartmentdeps")]
        public async Task<IActionResult> GetAllDepartmentDeps()
        {
            IDepartmentServer departmentServer = new DepartmentServerImpl(g_dbHelper, g_logServer);

            return await departmentServer.GetAllDepartmentDeps();
        }
    }
}
