using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.requests
{
    /// <summary>
    /// 搜索入库单
    /// </summary>
    public class SearchStockInModel
    {
        private int _page_index = 1;
        private int _page_size = 15;

        /// <summary>
        /// 订单号
        /// </summary>
        public string order_sn { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        public int page_index { get => _page_index; set { _page_index = value <= 0 ? 1 : value; } }

        /// <summary>
        /// 数量
        /// </summary>
        public int page_size { get => _page_size; set { _page_size = value <= 0 ? 15 : value; } }
    }
}
