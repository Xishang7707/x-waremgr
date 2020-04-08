using api.responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using web.common;
using web.externs;

namespace web.PageModels
{
    /// <summary>
    /// 授权验证接口
    /// </summary>
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
                Endpoint endpoint = context.GetEndpoint();
                if (endpoint == null)
                {
                    context.Response.Redirect("/other/error_404", false);
                    return;
                }

                if (!endpoint.Metadata.Any(a => a is IAllowAnonymous))
                {
                    //--更换为权限验证
                    if (!(await VerifyUser(context)))
                    {
                        context.Response.Redirect("/login", false);
                        return;
                    }
                }
                await next.Invoke(context);
            }
            catch (Exception ex)
            {
                context.Response.Redirect("/other/error_500", false);
                return;
            }
        }

        /// <summary>
        /// @xis 验证用户
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task<bool> VerifyUser(HttpContext context)
        {
            try
            {
                //1.身份验证
                Result<LoginResult> res = await HttpUtils.HttpGet<Result<LoginResult>>($"{ProgramConfig.API_HOST}/api/user/getuserinfo", _token: context.Request.GetToken(), _lang: context.Request.GetLang());

                if (res == null || res.status != "200")
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// @xis 验证权限
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task<bool> VerifyPrivilege(HttpContext context)
        {
            //1.身份验证
            LoginResult user = await HttpUtils.HttpGet<LoginResult>("http://127.0.0.1:7001/api/user/getuserinfo", _token: context.Request.GetToken(), _lang: context.Request.GetLang());
            if (user == null)
            {
                return false;
            }
            return true;
        }
    }
}
