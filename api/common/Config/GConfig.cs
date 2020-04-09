using common.Config;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace common
{
    public class GConfig
    {
        /// <summary>
        /// 数据库配置
        /// </summary>
        public static DBConfig DBConfig { get; set; }

        /// <summary>
        /// 缓存配置
        /// </summary>
        public static CacheConfig CacheConfig { get; set; }

        /// <summary>
        /// 配置路径
        /// </summary>
        public static UrlConfig UrlConfig { get; set; }

        /// <summary>
        /// 语言配置
        /// </summary>
        public static LanguageConfig LanguageConfig { get; set; }

        /// <summary>
        /// 公共配置
        /// </summary>
        public static CommonConfig CommonConfig { get; set; }

        /// <summary>
        /// 队列配置
        /// </summary>
        public static QueueConfig QueueConfig { get; set; }

        /// <summary>
        /// 初始化配置
        /// </summary>
        /// <param name="path"></param>
        public static void Init(string path)
        {
            StreamReader sr = new StreamReader(path);
            using JsonTextReader reader = new JsonTextReader(sr);
            JToken o = JToken.ReadFrom(reader);
            CommonConfig = new CommonConfig(o["CommonConfigs"] as JObject);
            LanguageConfig = new LanguageConfig(o["LanguageConfigs"] as JObject);

            DBConfig = new DBConfig(o["DBConfigs"] as JArray);
            CacheConfig = new CacheConfig(o["CacheConfigs"] as JObject);
            UrlConfig = new UrlConfig(o["UrlConfigs"] as JObject);
            QueueConfig = new QueueConfig(o["QueueConfigs"] as JObject);
        }
    }
}
