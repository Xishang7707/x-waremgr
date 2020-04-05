using System;
using System.Collections.Generic;
using System.Text;

namespace models.db_models
{
    /// <summary>
    /// 项目
    /// </summary>
    public class t_project
    {
        /// <summary>
        /// 项目id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string project_name { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime add_time { get; set; }

        /// <summary>
        /// 备注说明
        /// </summary>
        public string remark { get; set; }
    }
}
