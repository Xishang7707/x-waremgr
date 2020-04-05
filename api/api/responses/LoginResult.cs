using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.responses
{
    /// <summary>
    /// 登录实体
    /// </summary>
    public class LoginResult
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public int user_id { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string real_name { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string user_name { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string avatar { get; set; }

        /// <summary>
        /// 职位id
        /// </summary>
        public int position_id { get; set; }

        /// <summary>
        /// 职位名称
        /// </summary>
        public string position_name { get; set; }

        /// <summary>
        /// 部门id
        /// </summary>
        public int department_id { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string department_name { get; set; }

        /// <summary>
        /// token
        /// </summary>
        public string token { get; set; }
    }
}
