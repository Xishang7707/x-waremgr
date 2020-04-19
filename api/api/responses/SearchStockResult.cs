using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.responses
{
    /// <summary>
    /// 搜索的库存信息
    /// </summary>
    public class SearchStockResult
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 库存总数量
        /// </summary>
        public decimal quantity { get; set; }
    }

    /// <summary>
    /// 待入库库存列表
    /// </summary>
    public class SearchStockPrePaginerResult
    {
        public int id { get; set; }

        /// <summary>
        /// 库存id
        /// </summary>
        public int stock_id { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string product_name { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal quantity { get; set; }
    }
}
