using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using auto.Configs;
using common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace auto
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            GConfig.Init(PConfig.AppSettingPath + "/appsettings.json");
            new ConfigurationBuilder().AddJsonFile(PConfig.AppSettingPath + "/appsettings.json").AddEnvironmentVariables().Build();
            return Host.CreateDefaultBuilder(args)
                   .ConfigureWebHostDefaults(webBuilder =>
                   {
                       webBuilder.UseUrls(GConfig.UrlConfig.AutoApiPort);
                       webBuilder.UseStartup<Startup>();
                   });
        }
    }
}
