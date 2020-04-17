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
    public class SearchStockInModel : PaginerModel
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string order_sn { get; set; }
    }
}
