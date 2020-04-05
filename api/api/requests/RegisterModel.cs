using common.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace api.requests
{
    /// <summary>
    /// 注册
    /// </summary>
    public class RegisterModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required(ErrorMessage = ErrorCodeConst.ERROR_1002)]
        [MaxLength(15, ErrorMessage = ErrorCodeConst.ERROR_1006)]
        [MinLength(5, ErrorMessage = ErrorCodeConst.ERROR_1006)]
        public string user_name { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Required(ErrorMessage = ErrorCodeConst.ERROR_1021)]
        public string real_name { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = ErrorCodeConst.ERROR_1003)]
        [MaxLength(18, ErrorMessage = ErrorCodeConst.ERROR_1007)]
        [MinLength(6, ErrorMessage = ErrorCodeConst.ERROR_1007)]
        public string log_pwd { get; set; }

        /// <summary>
        /// 职位id
        /// </summary>
        [Required(ErrorMessage = ErrorCodeConst.ERROR_1020)]
        public string position_id { get; set; }
    }
}
