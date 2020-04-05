using common.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.requests
{
    /// <summary>
    /// 登录
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required(ErrorMessage =ErrorCodeConst.ERROR_1002)]
        public string user_name { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = ErrorCodeConst.ERROR_1003)]
        public string password { get; set; }
    }
}
