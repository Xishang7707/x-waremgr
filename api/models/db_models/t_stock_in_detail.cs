using System;
using System.Collections.Generic;
using System.Text;

namespace models.db_models
{
    /// <summary>
    /// 入库详情
    /// </summary>
    public class t_stock_in_detail
    {
        public int id { get; set; }

        /// <summary>
        /// 入库单号
        /// </summary>
        public string order_sn { get; set; }

        /// <summary>
        /// 产品id
        /// </summary>
        //public int product_id { get; set; }

        /// <summary>
        /// 库存id
        /// </summary>
        public int stock_id { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal quantity { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public decimal price { get; set; }
    }
}
