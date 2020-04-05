using api.responses;
using api.Servers.UserServer.Impl;
using common.Config;
using common.Redis;
using common.Utils;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.Externs
{
    public static class RequestExterns
    {
        public static string GetIp(this HttpRequest req)
        {
            string _ip = req.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(_ip))
            {
                _ip = req.HttpContext.Connection.RemoteIpAddress.ToString();
            }
            return _ip;
        }

        public static string GetLang(this HttpRequest req)
        {
            string _lang = req.Headers[ConstFlag.LANG_FLAG];
            if (string.IsNullOrWhiteSpace(_lang) || !LanguageConfig.Languages.Any(a => a == _lang))
            {
                _lang = ConstFlag.LANG_DEFAULT;
            }
            return _lang;
        }

        /// <summary>
        /// @xis 获取token 2020-2-21 00:00:10
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static string GetToken(this HttpRequest req)
        {
            string token = req.Headers[ConstFlag.TOKEN];
            return string.IsNullOrWhiteSpace(token) ? null : token;
        }

        /// <summary>
        /// @xis 获取登录用户 2020-2-21 00:00:10
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async static Task<LoginResult> GetUserAsync(this HttpRequest req)
        {
            string token = req.GetToken();
            if (token == null)
            {
                return null;
            }
            return await RedisHelper.Instance.GetStringKeyAsync<LoginResult>(UserServerImpl.RedisPrefix + token);
        }

        /// <summary>
        /// @xis 验证错误码转文字 2020-2-19 20:06:50
        /// </summary>
        /// <returns></returns>
        public static string GetErrorCode(this HttpRequest req, string code)
        {
            return LanguageConfig.GetErrorCode(code, GetLang(req));
        }

        /// <summary>
        /// @xis 向客户端写数据 2020-2-21 12:43:19
        /// </summary>
        /// <param name="res"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Task WriteBodyAsync(this HttpResponse res, object obj)
        {
            string str = JsonConvert.SerializeObject(obj);
            var bt = Encoding.Default.GetBytes(str);
            res.ContentType = "application/json";
            return res.Body.WriteAsync(bt, 0, bt.Length);
        }

        /// <summary>
        /// @xis 向客户端写数据 2020-2-21 12:43:19
        /// </summary>
        /// <param name="res"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Task WriteBodyAsync(this HttpResponse res, ResultBase obj)
        {
            if (!int.TryParse(obj.code, out int _))
            {
                obj.msg = obj.code;
                obj.code = "0";
            }
            else
            {
                obj.msg = LanguageConfig.GetErrorCode(obj.code, res.HttpContext.Request.GetLang());
            }

            res.HttpContext.Response.StatusCode = int.Parse(obj.status);
            return res.WriteBodyAsync(obj as object);
        }
    }
}
