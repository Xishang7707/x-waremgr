using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace web.externs
{
    public static class HttpRequestExterns
    {
        /// <summary>
        /// 获取token
        /// </summary>
        /// <param name="_req"></param>
        /// <returns></returns>
        public static string GetToken(this HttpRequest _req)
        {
            return _req.Headers["token"];
        }

        /// <summary>
        /// 获取lang
        /// </summary>
        /// <param name="_req"></param>
        /// <returns></returns>
        public static string GetLang(this HttpRequest _req)
        {
            return _req.Headers["lang"];
        }
    }
}
