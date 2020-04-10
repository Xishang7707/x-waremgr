using api.Servers.LogServer.Interface;
using api.Servers.PrivilegeServer.Interface;
using common.DB.Interface;
using models.db_models;
using models.enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace api.Servers.PrivilegeServer.Impl
{
    public class PrivilegeServerImpl : BaseServiceImpl, IPrivilegeServer
    {
        public PrivilegeServerImpl(IDbHelper dbHelper = null, ILogServer logServer = null) : base(dbHelper, logServer) { }
        public async Task<List<t_position_privilege_relation>> GetPrivilegesByPositionIdAsync(int position_id)
        {
            string sql_select = g_sqlMaker.Select<t_position_privilege_relation>().Where("position_id", "=", "@position_id").And("status", "=", "@status").ToSQL();
            return await g_dbHelper.QueryListAsync<t_position_privilege_relation>(sql_select, new { position_id, status = (int)EnumState.Normal });
        }

        public async Task<bool> HasPrivilege(int user_id, string privilege_key)
        {
            string sql_exist = g_sqlMaker.Select<t_user_privilege_relation>(s => new { s.id }).Where("user_id", "=", "@user_id").And("privilege_key", "=", "@privilege_key").And("status", "=", "@status").ToSQL();
            t_user_privilege_relation model = new t_user_privilege_relation
            {
                user_id = user_id,
                privilege_key = privilege_key,
                state = (int)EnumState.Normal,
                status = (int)EnumStatus.Enable
            };
            return (await g_dbHelper.QueryListAsync<int>(sql_exist, model)).Count > 0;
        }

        public async Task<bool> SetUserPrivileges(int user_id, IEnumerable<string> privilege_keys)
        {
            string modelname = "PrivilegeServerImpl.SetUserPrivileges";
            string sql_delete = g_sqlMaker.Delete<t_user_privilege_relation>().Where("user_id", "=", "@user_id").ToSQL();
            g_logServer.Log(modelname, "设置用户权限SQL", $"SQL:{sql_delete},{user_id}", EnumLogType.Debug);
            await g_dbHelper.ExecAsync(sql_delete, new { user_id });

            string sql_insert = g_sqlMaker.Insert<t_user_privilege_relation>(i => new
            {
                i.user_id,
                i.privilege_key,
                i.status,
                i.state
            }).ToSQL();
            foreach (var item in privilege_keys)
            {
                if (await g_dbHelper.ExecAsync(sql_insert, new { user_id, privilege_key = item, status = (int)EnumStatus.Enable, state = (int)EnumState.Normal }) <= 0)
                    return false;
            }

            return true;
        }
    }
}
