using api.responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace web.common
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
            foreach (var item in typeof(T).GetProperties())
            {
                p_l.Add($"{item.Name}={item.GetValue(_p)}");
            }
            return string.Join("&", p_l);
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
    }
}
