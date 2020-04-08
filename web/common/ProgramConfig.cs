using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace web.common
{
    public class ProgramConfig
    {
        public ProgramConfig(IConfigurationRoot conf)
        {
            API_HOST = conf["api_host"].ToString();
        }
        public static string API_HOST;
    }
}
