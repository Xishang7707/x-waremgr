using System;
using System.Collections.Generic;
using System.Text;

namespace models.db_models
{
    /// <summary>
    /// 部门
    /// </summary>
    public class t_department
    {
        /// <summary>
        /// 部门id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string department_name { get; set; }

        /// <summary>
        /// 上级部门
        /// </summary>
        public int? department_parent { get; set; }

        /// <summary>
        /// 删除状态
        /// </summary>
        public int state { get; set; }

        /// <summary>
        /// 部门状态
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime add_time { get; set; }
    }
}
