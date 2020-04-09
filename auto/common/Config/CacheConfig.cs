using Newtonsoft.Json.Linq;

namespace common.Config
{
    /// <summary>
    /// 缓存配置
    /// </summary>
    public class CacheConfig
    {
        public CacheConfig(JObject config)
        {
            RedisHost = config["redis_host"].ToString();
            RedisDBNum = int.Parse(config["redis_db"].ToString());
            RedisPwd = config["redis_pwd"].ToString();
        }

        public string RedisHost { get; set; }
        public int RedisDBNum { get; set; }
        public string RedisPwd { get; set; }
    }

}
