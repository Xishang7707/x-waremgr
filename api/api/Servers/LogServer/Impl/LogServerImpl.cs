using api.Configs;
using api.Servers.LogServer.Interface;
using common.RabbitMQ;
using models.enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Servers.LogServer.Impl
{
    public class LogServerImpl : ILogServer
    {
        public void Log(string model, string title, string msg, EnumLogType type)
        {
            RabbitServer.Instance.SendMessage(PConfig.QUEUE_LOG, new LogData
            {
                data = msg,
                make_time = DateTime.Now,
                model = model,
                title = title,
                type = (int)type
            });
        }

        public void Log(string model, string title, dynamic msg, EnumLogType type)
        {
            Log(model, title, JsonConvert.SerializeObject(msg), type);
        }
    }
}
