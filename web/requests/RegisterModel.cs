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
        public string user_name { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string real_name { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string log_pwd { get; set; }

        /// <summary>
        /// 职位id
        /// </summary>
        public string position_id { get; set; }
    }
}
