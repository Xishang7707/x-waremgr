using System;
using System.Collections.Generic;
using System.Text;

namespace models.db_models
{
    /// <summary>
    /// 职位
    /// </summary>
    public class t_position
    {
        /// <summary>
        /// 职位id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 部门id
        /// </summary>
        public int department_id { get; set; }

        /// <summary>
        /// 职位名称
        /// </summary>
        public string position_name { get; set; }

        /// <summary>
        /// 上级
        /// </summary>
        public string position_parent { get; set; }

        /// <summary>
        /// 删除状态
        /// </summary>
        public int state { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int status { get; set; }
    }
}
