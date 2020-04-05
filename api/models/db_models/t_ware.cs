using System;
using System.Collections.Generic;
using System.Text;

namespace models.db_models
{
    public class t_ware
    {
        public int id { get; set; }

        /// <summary>
        /// 仓库名
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        public string location { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime add_time { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 删除状态
        /// </summary>
        public int state { get; set; }
    }
}
