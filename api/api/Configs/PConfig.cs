using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace api.Configs
{
    public class PConfig
    {

        /// <summary>
        /// GUID
        /// </summary>
        public static string GUID = @"20b14440eec146789b1bbb8dc1d1195b";

        private static string _app_setting_path;
        /// <summary>
        /// 配置文件路径
        /// </summary>//"/xis-projects/x-waremgr/appsettings.json";//
        public static string AppSettingPath
        {
            //RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? "/xis-projects/x-waremgr/appsettings.json" : @"D:\Visual Studio Projects\x-waremgr\api\api\appsettings.json";
            get
            {
                if (_app_setting_path == null) _app_setting_path = Directory.GetCurrentDirectory(); return _app_setting_path;
            }
        }
    }
}


