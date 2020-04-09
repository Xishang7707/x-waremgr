using api.Servers.LogServer.Interface;
using common.RabbitMQ;
using models.db_models;
using models.enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Servers.LogServer.Impl
{
    public class LogServerImpl : BaseServiceImpl, ILogServer
    {
        public async Task Log(LogData data)
        {
            string sql_log = g_sqlMaker.Insert<t_log>(i => new
            {
                i.model,
                i.data,
                i.make_time,
                i.title,
                i.type
            }).ToSQL();
            try
            {
                await g_dbHelper.ExecAsync(sql_log, data);
            }catch(Exception e)
            {
                Console.WriteLine();
            }
        }

        public async Task Log(string model, string title, dynamic msg, EnumLogType type)
        {
            await Log(new LogData
            {
                data = msg,
                make_time = DateTime.Now,
                model = model,
                title = title,
                type = (int)type
            });
        }
    }
}
