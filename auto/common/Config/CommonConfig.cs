using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;

namespace common
{
    /// <summary>
    /// 公共固定配置
    /// </summary>
    public class CommonConfig
    {
        public CommonConfig(JObject config)
        {
            GGUID = config["guid"].ToString();

            //加载流程信息
            string process_path = Path.GetFullPath(config["process_path"].ToString()).TrimEnd('\\', '/');

            using StreamReader file = File.OpenText(process_path);
            using JsonTextReader reader = new JsonTextReader(file);
            ProcessConfig = (JObject)JToken.ReadFrom(reader);
        }

        public static string GGUID { get; set; }

        /// <summary>
        /// 配置文件路径
        /// </summary>
        public static string AppSettingPath = @"D:\Visual Studio Projects\X-Music\appsettings.json";

        /// <summary>
        /// 流程配置
        /// </summary>
        public static JObject ProcessConfig { get; set; } = new JObject();
    }
}
