using api.Externs;
using common.Config;
using common.Consts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace api.responses
{
    /// <summary>
    /// 响应基类
    /// </summary>
    public class ResultBase : ActionResult
    {
        /// <summary>
        /// 代码
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 服务器状态
        /// </summary>
        public string status { get; set; }

        /// <summary>
        /// 信息
        /// </summary>
        public string msg { get; set; }
    }

    public class Result : ResultBase
    {
        public override Task ExecuteResultAsync(ActionContext context)
        {
            return context.HttpContext.Response.WriteBodyAsync(this);
        }
    }

    public class Result<T> : Result
    {
        public T data { get; set; }
    }

    public class ExceptionResult : Result
    {
        public string ex { get; set; }
    }
}
