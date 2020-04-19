using System;
using System.Collections.Generic;
using System.Text;

namespace models.db_models
{
    /// <summary>
    /// 待入库
    /// </summary>
    public class t_stockin_pre
    {
        public int id { get; set; }

        /// <summary>
        /// 库存id
        /// </summary>
        public int stock_id { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal quantity { get; set; }

        public dynamic rv { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? update_time { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime? add_time { get; set; }
    }
}
