using System;
using System.Collections.Generic;
using System.Text;

namespace models.db_models
{
    /// <summary>
    /// 权限
    /// </summary>
    public class t_privilege
    {
        /// <summary>
        /// 权限id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 权限名称
        /// </summary>
        public string privilege_name { get; set; }

        /// <summary>
        /// 权限唯一键
        /// </summary>
        public string privilege_key { get; set; }

        /// <summary>
        /// 备注说明
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime add_time { get; set; }
    }
}
