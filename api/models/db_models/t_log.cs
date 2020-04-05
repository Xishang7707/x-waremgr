using System;
using System.Collections.Generic;
using System.Text;

namespace models.db_models
{
    /// <summary>
    /// 日志记录
    /// </summary>
    public class t_log
    {
        /// <summary>
        /// 日志id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 日志数据 json
        /// </summary>
        public string data { get; set; }

        /// <summary>
        /// 日志类型
        /// </summary>
        public int type { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime add_time { get; set; }
    }
}
