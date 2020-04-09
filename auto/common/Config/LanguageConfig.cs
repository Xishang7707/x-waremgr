using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace common.Config
{
    /// <summary>
    /// 语言
    /// </summary>
    public class LanguageConfig
    {
        public LanguageConfig(JObject config)
        {
            Languages = (config["languages"] as JArray).Select(s => s.ToString()).ToArray();

            //加载错误码信息
            string error_code_path = Path.GetFullPath(config["error_code"]["path"].ToString()).TrimEnd('\\', '/');
            string error_code_prefix = config["error_code"]["prefix"].ToString();
            foreach (var item in Languages)
            {
                using StreamReader file = File.OpenText($@"{error_code_path}/{error_code_prefix}{item}.json");
                using JsonTextReader reader = new JsonTextReader(file);
                JObject o = (JObject)JToken.ReadFrom(reader);
                Dictionary<string, string> error_dic = new Dictionary<string, string>();
                var pairs = o.GetEnumerator();

                while (pairs.MoveNext())
                {

                    error_dic.Add(pairs.Current.Key, pairs.Current.Value.ToString());
                }

                ErrorCodeConfig.Add(item, error_dic);
            }
        }

        /// <summary>
        /// 支持语言列表
        /// </summary>
        public static string[] Languages { get; private set; }

        /// <summary>
        /// 服务器错误码配置
        /// </summary>
        private static Dictionary<string, Dictionary<string, string>> ErrorCodeConfig { get; set; } = new Dictionary<string, Dictionary<string, string>>();

        /// <summary>
        /// 获取错误提示
        /// </summary>
        /// <param name="code">错误码</param>
        /// <param name="lang">语言类型</param>
        /// <returns></returns>
        public static string GetErrorCode(string code, string lang)
        {
            if (!ErrorCodeConfig.ContainsKey(lang) || !ErrorCodeConfig[lang].ContainsKey(code))
                return string.Empty;
            return ErrorCodeConfig[lang][code];
        }
    }
}
