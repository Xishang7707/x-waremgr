using System;
using System.Collections.Generic;
using System.Text;

namespace models.db_models
{
    /// <summary>
    /// 用户
    /// </summary>
    public class t_user
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string user_name { get; set; }

        /// <summary>
        /// 真实名字
        /// </summary>
        public string real_name { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string idcard { get; set; }
        
        /// <summary>
        /// 工号
        /// </summary>
        public string job_number { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        public string log_pwd { get; set; }

        /// <summary>
        /// salt
        /// </summary>
        public string salt { get; set; }

        /// <summary>
        /// 职位id
        /// </summary>
        public int position_id { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime? reg_time { get; set; }

        /// <summary>
        /// 删除状态
        /// </summary>
        public int state { get; set; }

        /// <summary>
        /// 用户状态
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string avatar { get; set; }
    }
}
