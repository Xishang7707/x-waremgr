using api.Attributes;
using api.Externs;
using api.responses;
using api.Servers.PrivilegeServer.Impl;
using api.Servers.PrivilegeServer.Interface;
using api.Servers.UserServer.Impl;
using api.Servers.UserServer.Interface;
using common.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.MiddleWare
{
    /// <summary>
    /// 授权中间件
    /// </summary>
    public class AuthMiddleware
    {
        private RequestDelegate next;
        public AuthMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                if (await Verify(context))
                {
                    await next.Invoke(context);
                }
            }
            catch (Exception ex)
            {
                ExceptionResult res = new ExceptionResult
                {
                    code = ErrorCodeConst.ERROR_500,
                    status = ErrorCodeConst.ERROR_403,
                    ex = JsonConvert.SerializeObject(ex)
                };
                await context.Response.WriteBodyAsync(res);
            }
        }

        /// <summary>
        /// @xis 验证
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task<bool> Verify(HttpContext context)
        {
            //1.身份验证
            LoginResult user = await context.Request.GetUserAsync();
            if (!await IDentityVerify(context, user))
            {
                Result res = new Result
                {
                    code = ErrorCodeConst.ERROR_1001,
                    status = ErrorCodeConst.ERROR_401
                };
                await context.Response.WriteBodyAsync(res);
                return false;
            }

            //不需要验证用户
            if (user == null || user.user_id == 1)
            {
                return true;
            }

            //token 续期
            IUserServer userServer = new UserServerImpl();
            await userServer.TokenRenewalAsync(user.token, user);

            //2.验证权限
            if (!await PrivilegeVerify(context, user))
            {
                Result res = new Result
                {
                    code = ErrorCodeConst.ERROR_1035,
                    status = ErrorCodeConst.ERROR_400
                };
                await context.Response.WriteBodyAsync(res);
                return false;
            }

            return true;
        }

        /// <summary>
        /// @xis 身份验证 2020-3-29 09:43:17
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task<bool> IDentityVerify(HttpContext context, LoginResult user)
        {

            if (context.GetEndpoint().Metadata.Any(a => a is IAllowAnonymous))
            {
                return true;
            }

            if (user == null)
            {
                return false;
            }

            return await Task.FromResult(true);
        }

        /// <summary>
        /// @xis 权限验证 2020-3-29 09:43:30
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task<bool> PrivilegeVerify(HttpContext context, LoginResult user)
        {
            if (context.GetEndpoint().Metadata.Any(a => a is PrivilegeAnyAttribute))
            {
                return true;
            }

            IEnumerable<PrivilegeAttribute> privilege_list = context.GetEndpoint().Metadata.Where(w => w is PrivilegeAttribute).Select(s => s as PrivilegeAttribute);
            if (privilege_list.Count() == 0)
            {
                return true;
            }
            IPrivilegeServer privilegeServer = new PrivilegeServerImpl();
            foreach (var item in privilege_list)
            {
                if (await privilegeServer.HasPrivilege(user.user_id, item.privilege_key))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
