using api.Servers.LogServer.Interface;
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
            // TODO
        }

        public void Log(string model, string title, dynamic msg, EnumLogType type)
        {
            Log(model, title, JsonConvert.SerializeObject(msg), type);
        }
    }
}
