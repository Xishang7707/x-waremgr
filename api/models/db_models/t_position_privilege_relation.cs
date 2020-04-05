using System;
using System.Collections.Generic;
using System.Text;

namespace models.db_models
{
    /// <summary>
    /// 职位权限
    /// </summary>
    public class t_position_privilege_relation
    {
        /// <summary>
        /// 权限id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 权限唯一固定标识
        /// </summary>
        public string privilege_key { get; set; }

        /// <summary>
        /// 职位id
        /// </summary>
        public int position_id { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime add_time { get; set; }
    }
}
