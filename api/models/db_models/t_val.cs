using System;
using System.Collections.Generic;
using System.Text;

namespace models.db_models
{
    /// <summary>
    /// 站点配置
    /// </summary>
    public class t_val
    {
        public int id { get; set; }

        /// <summary>
        /// 配置键
        /// </summary>
        public string val_key { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string val_name { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string value { get; set; }

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
