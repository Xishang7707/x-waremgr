using common.RabbitMQ;
using models.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Servers.LogServer.Interface
{
    public interface ILogServer
    {
        /// <summary>
        /// @xis 日志记录
        /// </summary>
        /// <param name="model">调用模块</param>
        /// <param name="title">标题</param>
        /// <param name="msg">数据 json</param>
        /// <param name="type">日志类型</param>
        Task Log(LogData data);

        /// <summary>
        /// @xis 日志记录
        /// </summary>
        /// <param name="model">调用模块</param>
        /// <param name="title">标题</param>
        /// <param name="msg">数据 json</param>
        /// <param name="type">日志类型</param>
        Task Log(string model, string title, dynamic msg, EnumLogType type);
    }
}
