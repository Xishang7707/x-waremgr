using api.Servers.LogServer.Impl;
using api.Servers.LogServer.Interface;
using auto.Configs;
using common.RabbitMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace auto.Servers
{
    public class LogServer
    {
        private ILogServer logServer = new LogServerImpl();
        public void Start()
        {
            RabbitServer.Instance.CreateConsumer<LogData>(PConfig.QUEUE_LOG, o =>
            {
                logServer.Log(o);
                return true;
            });
        }
    }
}
