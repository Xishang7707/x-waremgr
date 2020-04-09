using auto.Configs;
using common.RabbitMQ;
using common.Utils;
using models.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace auto.Servers
{
    /// <summary>
    /// 自动报备
    /// </summary>
    public class ReportFiddlerServer
    {
        private string modelname = "auto.Servers.ReportFiddlerServer";
        private DateTime report_time;
        private bool is_success = false;
        private Timer _time;
        public void Start()
        {
            _time = new Timer(async o =>
            {
                if (report_time != null && report_time.ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd") && is_success == true)
                {
                    return;
                }
                try
                {
                    await DoWork();
                    report_time = DateTime.Now;
                    is_success = true;
                }
                catch (Exception e)
                {
                    is_success = false;
                    RabbitServer.Instance.SendMessage(PConfig.QUEUE_LOG, new LogData
                    {
                        model = modelname,
                        make_time = DateTime.Now,
                        title = "自动报备失败",
                        type = (int)EnumLogType.Info,
                        data = e.Message,
                    });
                }
            }, null, 0, (int)TimeSpan.FromHours(1).TotalMilliseconds);
        }

        private async Task DoWork()
        {
            string url = $"http://www.hngczy.cn/jkreport/jkbb.aspx";
            var data = new
            {
                __VIEWSTATE = "/wEPDwUKMjA5NDY4NzUxMg9kFgICAw9kFgpmDw9kFgYeBVZhbHVlBRjor7fovpPlhaXlrablj7fmiJblt6Xlj7ceB09uRm9jdXMFOmlmKHRoaXMudmFsdWU9PSfor7fovpPlhaXlrablj7fmiJblt6Xlj7cnKSB7dGhpcy52YWx1ZT0nJ30eBk9uQmx1cgU5aWYodGhpcy52YWx1ZT09Jycpe3RoaXMudmFsdWU9J+ivt+i+k+WFpeWtpuWPt+aIluW3peWPtyd9ZAICDw8WAh4EVGV4dAUG6b6Z57+UZGQCAw8PFgQfAwUBMB4HVmlzaWJsZWhkZAIGDw8WAh8DBQoyMDE3MTczODA0ZGQCCQ8PZBYEHwEFDXRoaXMudmFsdWU9JycfAgVOaWYodGhpcy52YWx1ZT09Jycpe3RoaXMudmFsdWU9J+ivt+aMieagvOW8j+Whq+WGme+8jOWmgu+8mua5luWNl+ecgemVv+aymeW4gid9ZBgBBR5fX0NvbnRyb2xzUmVxdWlyZVBvc3RCYWNrS2V5X18WFAUCamsFAmprBQNuamsFA25qawUCaGIFAmhiBQNuaGIFA25oYgUDY2hiBQNjaGIFBG5jaGIFBG5jaGIFBmNkb3VidAUGY2RvdWJ0BQduY2RvdWJ0BQduY2RvdWJ0BQVkb3VidAUFZG91YnQFBm5kb3VidAUGbmRvdWJ0tschy+FPQ2cMI6vFO5mxwGRBMwfmQudHI2qzh5Mj0vM=",
                __VIEWSTATEGENERATOR = "03881B9E",
                __EVENTVALIDATION = "/wEdABSW0gA6wv8AtlFD0+0v5Y+Jw0SHfPflUTwQtFrIk5X6z1xJx+usTkI9Uo8SoPQbyjWBIL75NB/hLt08pUsRYtbsa87Sn49t24nCvaT5QfNVIg8Inq/DZ5eemuqKk4Wikv3I5KJk3khFhR5ShgkI3vmgcHoovqxvQXxdNhVWHlyhvbBPXhGdWr/YXTvdLGbmobNQEwTvGwMcXAdLhjt0ee/9Asr6CXcLaz0EkxoIy5D10NfO0pfSJOTJfgBjRxcILeNJDguWzpiP4oqVPlkWhrZTp7j9s2Yx68SbQvTaT+UtOze9j33HUQSIMh0RaTiNbnxDAxRdkTfkNWIL9VMwrBcMSNg71ROrJv5P6iMpPIye5pWTLspmtMLKLaC2V9h5QWkA9KmJEaleLbuhk1z2x0E4b8zY86tE6gSSIfYuTOmrZCoG7ksFFo7LyrkeG1oeVul731Az04/6vtG+iygGCIdw",
                txtXgh = "201717380440",
                txtJkinfo = "njk",
                txtaddress = "深圳",
                txtcomecs = "",
                txttraffic = "",
                txtHbinfo = "nhb",
                txtChbinfo = "nchb",
                txtCdoubtinfo = "ncdoubt",
                txtDoubtinfo = "ndoubt",
                txtBz = "",
                submitbb = "报   备"
            };

            Dictionary<string, object> dic = new Dictionary<string, object>
            {
                //{"Cache-Control","max-age=0" },
                //{"Accept","text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8" },
                //{"Accept-Encoding","gzip, deflate" },
                //{"Accept-Language","zh-CN,zh;q=0.8,en-US;q=0.6,en;q=0.5;q=0.4" },
                //{"User-Agent","Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.116 Safari/537.36 QBCore/4.0.1295.400 QQBrowser/9.0.2524.400 Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2875.116 Safari/537.36 NetType/WIFI MicroMessenger/7.0.5 WindowsWechat" },
                //{"Content-Type","application/x-www-form-urlencoded" },
                //{"Upgrade-Insecure-Requests","1" },
            };

            string res = await HttpUtils.HttpPostForm(url, data, dic);
            RabbitServer.Instance.SendMessage(PConfig.QUEUE_LOG, new LogData
            {
                model = modelname,
                make_time = DateTime.Now,
                title = "自动报备成功",
                type = (int)EnumLogType.Info,
                data = res,
            });
        }
    }
}
