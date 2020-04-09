using Newtonsoft.Json.Linq;

namespace common.Config
{
    /// <summary>
    /// 路径配置
    /// </summary>
    public class UrlConfig
    {
        public UrlConfig() { }
        public UrlConfig(JObject config)
        {
            ApiPort = config["api_port"].ToString();
            ApiUrl = config["api_url"].ToString();
            HtPort = config["ht_port"].ToString();
            HtUrl = config["ht_url"].ToString();
            HtApiPort = config["ht_api_port"].ToString();
            HtApiUrl = config["ht_api_url"].ToString();
            AutoApiUrl = config["auto_api_url"].ToString();
            AutoApiPort = config["auto_api_port"].ToString();
        }

        /// <summary>
        /// API 端口
        /// </summary>
        public string ApiPort { get; set; }

        /// <summary>
        /// API地址
        /// </summary>
        public string ApiUrl { get; set; }

        /// <summary>
        /// 后台端口
        /// </summary>
        public string HtPort { get; set; }

        /// <summary>
        /// 后台地址
        /// </summary>
        public string HtUrl { get; set; }

        /// <summary>
        /// 后台api端口
        /// </summary>
        public string HtApiPort { get; set; }

        /// <summary>
        /// 后台api地址
        /// </summary>
        public string HtApiUrl { get; set; }

        /// <summary>
        /// 自动服务地址
        /// </summary>
        public string AutoApiPort { get; set; }

        /// <summary>
        /// 自动服务端口
        /// </summary>
        public string AutoApiUrl { get; set; }
    }
}
