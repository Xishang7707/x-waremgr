using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using api.responses;
using common.Consts;
using common.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : BaseController
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<string>> Get()
        {
            string guid = Common.MakeGuid();
            string salt = Common.MakeMd5(guid);
            string pwd = Common.MakeMd5("123456", "1" + "33B23DEB1C8BDB705B4B7743DE05C630");
            bool res = "2CD5F3AB2D5EB5E7E6D280B1EA53B14D" == pwd;
            return new string[] { salt, pwd };
        }

        public class SQLModel
        {
            public string guid { get; set; }
            public string str { get; set; }
        }
        /// <summary>
        /// 数据库加密
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        [HttpGet("sql")]
        [AllowAnonymous]
        public IActionResult SQL([FromQuery]SQLModel model)
        {
            return new Result<string>
            {
                code = ErrorCodeConst.ERROR_200,
                status = ErrorCodeConst.ERROR_200,
                data = Common.AESEncrypt(model.str, model.guid)
            };
        }
        
        /// <summary>
        /// 数据库解密
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        [HttpGet("sqldesc")]
        [AllowAnonymous]
        public IActionResult SQLDesc([FromQuery]SQLModel model)
        {
            return new Result<string>
            {
                code = ErrorCodeConst.ERROR_200,
                status = ErrorCodeConst.ERROR_200,
                data = Common.AESDecrypt(model.str, model.guid)
            };
        }
    }
}
