﻿using System;
using System.Collections.Generic;
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

        /// <summary>
        /// 配置文件路径
        /// </summary>//"/xis-projects/x-waremgr/appsettings.json";//
        public static string AppSettingPath = RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? "/xis-projects/x-waremgr/appsettings.json" : @"D:\Visual Studio Projects\x-waremgr\api\api\appsettings.json";
    }
}
