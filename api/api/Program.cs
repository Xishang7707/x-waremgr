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
            Console.WriteLine("step1");
            GConfig.Init(PConfig.AppSettingPath + "/appsettings.json");
            Console.WriteLine("step1");
            new ConfigurationBuilder().SetBasePath(PConfig.AppSettingPath).AddJsonFile("appsettings.json").AddEnvironmentVariables().Build();
            return Host.CreateDefaultBuilder(args)
                   .ConfigureWebHostDefaults(webBuilder =>
                   {
                       webBuilder.UseUrls(GConfig.UrlConfig.ApiPort);
                       webBuilder.UseStartup<Startup>();
                   });
        }
    }
}
