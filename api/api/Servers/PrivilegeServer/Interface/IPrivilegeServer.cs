using models.db_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Servers.PrivilegeServer.Interface
{
    /// <summary>
    /// 权限
    /// </summary>
    public interface IPrivilegeServer
    {
        /// <summary>
        /// @xis 获取职位的权限 2020-3-24 13:35:57
        /// </summary>
        /// <param name="position_id">职位id</param>
        /// <returns></returns>
        Task<List<t_position_privilege_relation>> GetPrivilegesByPositionIdAsync(int position_id);

        /// <summary>
        /// @xis 设置用户权限 2020-3-24 13:50:38
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="privileges"></param>
        /// <returns></returns>
        Task<bool> SetUserPrivileges(int user_id, IEnumerable<string> privilege_keys);

        /// <summary>
        /// @xis 验证权限 2020-3-29 09:48:56
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="privilege_key"></param>
        /// <returns></returns>
        Task<bool> HasPrivilege(int user_id, string privilege_key);
    }
}
