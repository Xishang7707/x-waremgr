using api.requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.responses
{
    /// <summary>
    /// 入库单详情
    /// </summary>
    public class StockInDetailResult : SearchStockInResult
    {
        /// <summary>
        /// 产品
        /// </summary>
        public List<StockInProductResult> products { get; set; }
    }

    public class StockInProductResult : StockInProduct
    {
        /// <summary>
        /// 供货商名称
        /// </summary>
        public string factory_name { get; set; }
    }

}
