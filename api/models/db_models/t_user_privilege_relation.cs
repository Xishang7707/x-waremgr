using System;
using System.Collections.Generic;
using System.Text;

namespace models.db_models
{
    /// <summary>
    /// 用户拥有权限
    /// </summary>
    public class t_user_privilege_relation
    {
        public int id { get; set; }

        /// <summary>
        /// 权限唯一键
        /// </summary>
        public string privilege_key { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public int user_id { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime add_time { get; set; }

        /// <summary>
        /// 可用状态
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 删除状态
        /// </summary>
        public int state { get; set; }
    }
}
