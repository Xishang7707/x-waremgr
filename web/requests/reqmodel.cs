using api.responses;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.requests
{
    /// <summary>
    /// 请求基类 包含请求的信息
    /// </summary>
    public class reqmodel
    {
        /// <summary>
        /// 语音
        /// </summary>
        public string Lang { get; set; }

        /// <summary>
        /// Ip
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// 请求
        /// </summary>
        public HttpRequest Request { get; set; }

        /// <summary>
        /// 登录用户
        /// </summary>
        public LoginResult User { get; set; }
    }

    /// <summary>
    /// 请求基类 包含请求的信息
    /// </summary>
    public class reqmodel<T> : reqmodel
    {
        /// <summary>
        /// 请求数据
        /// </summary>
        public T Data { get; set; }
    }
}
