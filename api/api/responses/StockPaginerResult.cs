using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.responses
{
    public class StockPaginerResult
    {
        /// <summary>
        /// 产品名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal quantility { get; set; }
    }
}
