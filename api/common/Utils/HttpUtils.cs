using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace common.Utils
{
    public class HttpUtils
    {
        /// <summary>
        /// 实体转get参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_p"></param>
        /// <returns></returns>
        private static string ToQuerys<T>(T _p)
        {
            List<string> p_l = new List<string>();
            foreach (var item in _p.GetType().GetProperties())
            {
                p_l.Add($"{item.Name}={item.GetValue(_p)}");
            }
            return string.Join("&", p_l);
        }

        /// <summary>
        /// 实体转post参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_p"></param>
        /// <returns></returns>
        private static FormUrlEncodedContent ToForm<T>(T _p)
        {
            Dictionary<string, string> p_l = new Dictionary<string, string>();
            foreach (var item in _p.GetType().GetProperties())
            {
                p_l.Add($"{item.Name}", $"{item.GetValue(_p)}");
            }
            return new FormUrlEncodedContent(p_l);
        }

        public static async Task<T> HttpGet<T>(string _url, string _p = null, string _token = null, string _lang = null)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("token", _token);
            client.DefaultRequestHeaders.Add("lang", _lang);

            HttpResponseMessage res = await client.GetAsync($"{_url}?{_p}");

            T result = JsonConvert.DeserializeObject<T>(await res.Content.ReadAsStringAsync());
            return result;
        }

        public static async Task<_out> HttpPost<_in, _out>(string _url, _in _p, string _token, string _lang = null)
        {
            HttpWebRequest req = WebRequest.Create(_url) as HttpWebRequest;
            req.Method = "POST";
            req.Headers.Add("token", _token);
            req.Headers.Add("lang", _lang);
            req.ContentType = "application/json";

            string p = JsonConvert.SerializeObject(_p);

            await req.GetRequestStream().WriteAsync(Encoding.UTF8.GetBytes(p));

            HttpWebResponse res = (await req.GetResponseAsync()) as HttpWebResponse;

            _out result;
            using (Stream stream = res.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                result = JsonConvert.DeserializeObject<_out>(await reader.ReadToEndAsync());

            return result;
        }

        public static async Task<string> HttpPostForm(string _url, dynamic _p, Dictionary<string, object> headers = null)
        {
            HttpClient client = new HttpClient();
            FormUrlEncodedContent content = ToForm(_p);

            if (headers != null)
                foreach (var item in headers.Keys)
                {
                    content.Headers.Add(item, headers[item]?.ToString());
                }

            var response = await client.PostAsync(_url, content);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
