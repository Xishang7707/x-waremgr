using api.Externs;
using api.requests;
using api.responses;
using api.Servers.DepartmentServer.Impl;
using api.Servers.DepartmentServer.Interface;
using api.Servers.LogServer.Impl;
using api.Servers.LogServer.Interface;
using api.Servers.PositionServer.Impl;
using api.Servers.PositionServer.Interface;
using api.Servers.PrivilegeServer.Impl;
using api.Servers.PrivilegeServer.Interface;
using api.Servers.UserServer.Interface;
using common.Config;
using common.Consts;
using common.DB.Impl;
using common.DB.Interface;
using common.Redis;
using common.SqlMaker;
using common.SqlMaker.Interface;
using common.Utils;
using models.db_models;
using models.enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.Servers.UserServer.Impl
{
    /// <summary>
    /// @xis 用户服务类 2020-2-19 20:33:06
    /// </summary>
    public class UserServerImpl : BaseServiceImpl, IUserServer
    {
        public const string RedisPrefix = "api:";

        public UserServerImpl(IDbHelper dbHelper = null, ILogServer logServer = null) : base(dbHelper, logServer) { }

        #region 内部公共
        /// <summary>
        /// @xis 加密密码 2020-3-25 07:54:39
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pwd"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        private string EncPassword(int id, string pwd, string salt)
        {
            return Common.AESEncrypt(Common.MakeMd5(pwd, id.ToString()), Common.MakeMd5(salt + id + salt));
        }

        /// <summary>
        /// @xis 生成salt
        /// </summary>
        /// <returns></returns>
        private string MakeUserSalt()
        {
            return Common.MakeMd5(Common.MakeMd5(Common.MakeGuid()));
        }
        #endregion

        /// <summary>
        /// @xis 添加用户
        /// </summary>
        /// <param name="reqmodel"></param>
        /// <returns></returns>
        public async Task<Result> AddUserAsync(reqmodel<RegisterModel> reqmodel)
        {
            const string modelname = "UserServerImpl.AddUserAsync";
            Result result = new Result { status = ErrorCodeConst.ERROR_403, code = ErrorCodeConst.ERROR_100 };

            //检查用户名是否存在
            string sql_user_name_exist = g_sqlMaker.Select<t_user>(s => new { s.id })
                .Where("user_name", "=", "@user_name")
                .And("state", "=", (int)EnumState.Normal)
                .ToSQL();

            try
            {
                g_dbHelper.Transaction();
                //检查用户名是否存在
                t_user user = await g_dbHelper.QueryAsync<t_user>(sql_user_name_exist, new { reqmodel.Data.user_name });
                if (user != null && user.id != 0)
                {
                    g_dbHelper.Rollback();
                    g_logServer.Log(modelname, "添加用户失败", new { msg = $"用户名{reqmodel.Data.user_name}已存在" }, EnumLogType.Info);
                    result.code = ErrorCodeConst.ERROR_1005;
                    return result;
                }

                //职位
                IPositionServer positionServer = new PositionServerImpl(g_dbHelper, g_logServer);

                if (!int.TryParse(reqmodel.Data.position_id, out int position_id))
                {
                    g_dbHelper.Rollback();
                    g_logServer.Log(modelname, "添加用户失败", new { msg = $"parse position_id fail" }, EnumLogType.Info);
                    result.code = ErrorCodeConst.ERROR_1020;
                    return result;
                }

                if (!await positionServer.ExistPositionAsync(position_id))
                {
                    g_dbHelper.Rollback();
                    g_logServer.Log(modelname, "添加用户失败", new { msg = $"position_id not exist" }, EnumLogType.Info);
                    result.code = ErrorCodeConst.ERROR_1020;
                    return result;
                }

                user = new t_user
                {
                    user_name = reqmodel.Data.user_name,
                    real_name = reqmodel.Data.real_name,
                    position_id = position_id,
                    status = 1,
                    state = 1
                };

                string sql_user_insert = g_sqlMaker.Insert<t_user>(i => new { i.user_name, i.real_name, i.position_id, i.state, i.status }).ToSQL();
                user.id = await g_dbHelper.ExecScalarAsync<int>(sql_user_insert, user);
                if (user.id == 0)
                {
                    g_dbHelper.Rollback();
                    g_logServer.Log(modelname, "添加用户失败", new { msg = $"id=0" }, EnumLogType.Info);
                    result.code = ErrorCodeConst.ERROR_1018;
                    return result;
                }

                //设置密码
                user.salt = MakeUserSalt();
                user.log_pwd = EncPassword(user.id, reqmodel.Data.log_pwd, user.salt);

                string sql_user_update = g_sqlMaker.Update<t_user>(u => new
                {
                    u.salt,
                    u.log_pwd
                })
                .Where("id", "=", "@id")
                .And("state", "=", (int)EnumState.Normal)
                .ToSQL();
                if (await g_dbHelper.ExecAsync(sql_user_update, user) <= 0)
                {
                    g_dbHelper.Rollback();
                    g_logServer.Log(modelname, "添加用户失败", new { msg = $"update pwd fail" }, EnumLogType.Info);
                    result.code = ErrorCodeConst.ERROR_1018;
                    return result;
                }

                //权限
                IPrivilegeServer privilegeServer = new PrivilegeServerImpl(g_dbHelper, g_logServer);
                List<t_position_privilege_relation> privilege_list = await privilegeServer.GetPrivilegesByPositionIdAsync(user.id);
                if (!await privilegeServer.SetUserPrivileges(user.id, privilege_list.Select(s => s.privilege_key)))
                {
                    g_dbHelper.Rollback();
                    g_logServer.Log(modelname, "添加用户失败", new { msg = $"insert privilege fail" }, EnumLogType.Info);
                    result.code = ErrorCodeConst.ERROR_1018;
                    return result;
                }
                g_dbHelper.Commit();

                g_logServer.Log(modelname, "添加用户成功", new { msg = $"用户名:{reqmodel.Data.user_name}" }, EnumLogType.Info);
                result.code = ErrorCodeConst.ERROR_1019;
                result.status = ErrorCodeConst.ERROR_200;
                return result;
            }
            catch (Exception ex)
            {
                g_dbHelper.Rollback();
                g_logServer.Log(modelname, "添加用户异常", JsonConvert.SerializeObject(ex), EnumLogType.Error);
                result.code = ErrorCodeConst.ERROR_1018;
            }
            return result;
        }

        /// <summary>
        /// @xis 登录 2020-3-25 07:52:00
        /// </summary>
        /// <param name="reqmodel"></param>
        /// <returns></returns>
        public async Task<Result> LoginAsync(reqmodel<LoginModel> reqmodel)
        {
            const string modelname = "UserServerImpl.LoginAsync";
            Result<LoginResult> result = new Result<LoginResult> { status = ErrorCodeConst.ERROR_403, code = ErrorCodeConst.ERROR_100 };

            string sql_user_select = g_sqlMaker.Select<t_user>(s => new
            {
                s.id,
                s.user_name,
                s.real_name,
                s.salt,
                s.log_pwd,
                s.position_id
            })
                .Where($"user_name", "=", "@user_name")
                .And("state", "=", "@state")
                .ToSQL();

            t_user user = await g_dbHelper.QueryAsync<t_user>(sql_user_select, new { reqmodel.Data.user_name, state = (int)EnumState.Normal });
            if (user == null)
            {
                g_logServer.Log(modelname, "登录失败", new { msg = $"用户名:{reqmodel.Data.user_name}" }, EnumLogType.Debug);
                result.code = ErrorCodeConst.ERROR_1004;
                return result;
            }

            string pwd = EncPassword(user.id, reqmodel.Data.password, user.salt);
            if (user.log_pwd != pwd)
            {
                g_logServer.Log(modelname, "登录失败", new { msg = $"用户名:{reqmodel.Data.user_name}" }, EnumLogType.Debug);
                result.code = ErrorCodeConst.ERROR_1004;
                return result;
            }

            //获取职位信息
            IPositionServer positionServer = new PositionServerImpl(g_dbHelper, g_logServer);
            t_position position_model = await positionServer.GetPosition(s => new { s.id, s.position_name, s.department_id }, user.position_id);
            if (position_model == null)
            {
                g_logServer.Log(modelname, "登录失败", new { msg = $"用户名:{reqmodel.Data.user_name},获取职位信息失败" }, EnumLogType.Debug);
                result.code = ErrorCodeConst.ERROR_1022;
                return result;
            }

            //获取部门信息
            IDepartmentServer departmentServer = new DepartmentServerImpl(g_dbHelper, g_logServer);
            t_department depart_model = await departmentServer.GetDepartment(s => new { s.id, s.department_name }, position_model.department_id);
            if (depart_model == null)
            {
                g_logServer.Log(modelname, "登录失败", new { msg = $"用户名:{reqmodel.Data.user_name},获取部门信息失败" }, EnumLogType.Debug);
                result.code = ErrorCodeConst.ERROR_1022;
                return result;
            }

            //token
            string token = Common.AESEncrypt(Common.MakeMd5(Common.MakeGuid()), Common.MakeGuid());

            LoginResult login_info = new LoginResult
            {
                user_id = user.id,
                user_name = user.user_name,
                avatar = user.avatar,
                real_name = user.real_name,
                department_id = depart_model.id,
                department_name = depart_model.department_name,
                position_id = position_model.id,
                position_name = position_model.position_name,
                token = token
            };

            bool login_flag = await TokenRenewalAsync(token, login_info);
            if (!login_flag)
            {
                g_logServer.Log(modelname, "登录失败", new { msg = $"用户名:{reqmodel.Data.user_name},存Redis失败" }, EnumLogType.Debug);
                result.code = ErrorCodeConst.ERROR_1022;
                return result;
            }

            result.data = login_info;
            result.status = ErrorCodeConst.ERROR_200;
            result.code = ErrorCodeConst.ERROR_1008;
            return result;
        }

        public async Task<Result> LoginOutAsync(reqmodel reqmodel)
        {
            const string modelname = "UserServerImpl.LoginOutAsync";
            Result result = new Result { status = ErrorCodeConst.ERROR_403, code = ErrorCodeConst.ERROR_100 };

            bool loginout_flag = await RedisHelper.Instance.DeleteStringKeyAsync(RedisPrefix + reqmodel.User.token);
            if (!loginout_flag)
            {
                g_logServer.Log(modelname, "登出失败", new { msg = $"用户名:{reqmodel.User.user_name}" }, EnumLogType.Debug);
                result.code = ErrorCodeConst.ERROR_100;
                return result;
            }

            result.code = ErrorCodeConst.ERROR_200;
            result.status = ErrorCodeConst.ERROR_200;
            return result;
        }

        public async Task<Result> GetUserInfoAsync(reqmodel reqmodel)
        {
            //const string modelname = "UserServerImpl.GetUserInfoAsync";
            //ILogServer logServer = new LogServerImpl();
            Result<LoginResult> result = new Result<LoginResult>();

            result.code = ErrorCodeConst.ERROR_200;
            result.status = ErrorCodeConst.ERROR_200;
            result.data = reqmodel.User;
            return await Task.FromResult(result);
        }

        /// <summary>
        /// @xis token续期/记录 2020-3-29 10:17:54
        /// </summary>
        /// <param name="token"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> TokenRenewalAsync(string token, LoginResult user)
        {
            return await RedisHelper.Instance.SetStringKeyAsync(RedisPrefix + token, user, TimeSpan.FromSeconds(ConstFlag.UserExpireTime));
        }

        public async Task<t_user> GetUserById(Func<t_user, dynamic> selector, int id)
        {
            string sql = g_sqlMaker.Select(selector).Where("id", "=", "@id").ToSQL();
            return await g_dbHelper.QueryAsync<t_user>(sql, new { id });
        }
    }
}
