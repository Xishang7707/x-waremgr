using System.ComponentModel.DataAnnotations;

namespace api.requests
{
    /// <summary>
    /// 入库单详情
    /// </summary>
    public class StockInDetailModel
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string order_sn { get; set; }
    }
}
