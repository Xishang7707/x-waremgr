using api.requests;
using api.responses;
using models.db_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Servers.DepartmentServer.Interface
{
    /// <summary>
    /// 部门
    /// </summary>
    public interface IDepartmentServer
    {
        /// <summary>
        /// @xis 增加部门 2020-3-28 10:53:12
        /// </summary>
        /// <param name="reqmodel"></param>
        /// <returns></returns>
        Task<Result> AddDepartment(reqmodel<AddDepartmentModel> reqmodel);

        /// <summary>
        /// @xis 删除部门
        /// </summary>
        /// <param name="reqmodel"></param>
        /// <returns></returns>
        Task<Result> DeleteDepartment(reqmodel<int> reqmodel);

        /// <summary>
        /// @xis 获取所有部门信息
        /// </summary>
        /// <returns></returns>
        Task<Result<IEnumerable<DepartmentResult>>> GetAllDepartmentDeps();

        /// <summary>
        /// @xis 获取所有部门信息
        /// </summary>
        /// <returns></returns>
        Task<Result<IEnumerable<DepartmentResult>>> GetAllDepartments();

        /// <summary>
        /// @xis 获取部门信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<t_department> GetDepartment(int id);
    }
}
