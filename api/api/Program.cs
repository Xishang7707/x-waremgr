using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Configs;
using common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            GConfig.Init(PConfig.AppSettingPath);
            new ConfigurationBuilder().AddJsonFile(PConfig.AppSettingPath).AddEnvironmentVariables().Build();
            return Host.CreateDefaultBuilder(args)
                   .ConfigureWebHostDefaults(webBuilder =>
                   {
                       webBuilder.UseUrls(GConfig.UrlConfig.ApiPort);
                       webBuilder.UseStartup<Startup>();
                   });
        }
    }
}
