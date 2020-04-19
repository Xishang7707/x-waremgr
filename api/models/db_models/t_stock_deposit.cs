using System;
using System.Collections.Generic;
using System.Text;

namespace models.db_models
{
    /// <summary>
    /// 货物安置
    /// </summary>
    public class t_stock_deposit
    {
        public int id { get; set; }

        /// <summary>
        /// 仓库id
        /// </summary>
        public int ware_id { get; set; }

        /// <summary>
        /// 库存id
        /// </summary>
        public int stock_id { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal quantity { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        public string location { get; set; }

        public dynamic rv { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime? add_time { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? update_time { get; set; }
    }
}
