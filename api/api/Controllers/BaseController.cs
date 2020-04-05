using api.Externs;
using api.requests;
using api.responses;
using common.Config;
using common.Consts;
using common.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Controllers
{
    public class BaseController : ControllerBase
    {
        private string _lang;
        /// <summary>
        /// @xis 获取语言 2020-2-19 20:27:41
        /// </summary>
        public string Lang
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_lang))
                {
                    return _lang;
                }

                _lang = Request.Headers[ConstFlag.LANG_FLAG];
                if (string.IsNullOrWhiteSpace(_lang) || !LanguageConfig.Languages.Any(a => a == _lang))
                {
                    _lang = ConstFlag.LANG_DEFAULT;
                }
                return _lang;
            }
        }

        private string _ip;
        /// <summary>
        /// @xis 获取ip 2020-2-19 20:28:07
        /// </summary>
        public string Ip
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_ip))
                {
                    return _ip;
                }
                _ip = Request.Headers["X-Forwarded-For"].FirstOrDefault();
                if (string.IsNullOrWhiteSpace(_ip))
                {
                    _ip = HttpContext.Connection.RemoteIpAddress.ToString();
                }
                return _ip;
            }
        }

        /// <summary>
        /// @xis 验证错误码转文字 2020-2-19 20:06:50
        /// </summary>
        /// <returns></returns>
        public string GetErrorCode(string code)
        {
            return LanguageConfig.GetErrorCode(code, Lang);
        }

        /// <summary>
        /// @xis 获取模型验证错误信息 2020-2-19 20:12:09
        /// </summary>
        /// <returns></returns>
        public Result GetModelErrorCode()
        {
            Result res = new Result { status = ErrorCodeConst.ERROR_403 };
            res.code = ModelState.Values.FirstOrDefault()?.Errors?.FirstOrDefault()?.ErrorMessage;
            return res;
        }

        /// <summary>
        /// 包装返回的请求
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<reqmodel> RequestPackingAsync()
        {
            reqmodel reqmodel = new reqmodel
            {
                Lang = Lang,
                Ip = Ip,
                Request = Request,
                User = await Request.GetUserAsync()
            };

            return reqmodel;
        }

        /// <summary>
        /// 包装返回的请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<reqmodel<T>> RequestPackingAsync<T>(T data)
        {
            reqmodel<T> reqmodel = new reqmodel<T>
            {
                Data = data,
                Lang = Lang,
                Ip = Ip,
                Request = Request,
                User = await Request.GetUserAsync()
            };

            return reqmodel;
        }
    }
}
