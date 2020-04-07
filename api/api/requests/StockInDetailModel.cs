using common.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
        [Required(ErrorMessage = ErrorCodeConst.ERROR_1048)]
        public string order_sn { get; set; }
    }
}
