using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using common.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            string guid = Common.MakeGuid();
            string salt = Common.MakeMd5(guid);
            string pwd = Common.MakeMd5("123456", "1" + "33B23DEB1C8BDB705B4B7743DE05C630");
            bool res = "2CD5F3AB2D5EB5E7E6D280B1EA53B14D" == pwd;
            return new string[] { salt, pwd };
        }
    }
}
