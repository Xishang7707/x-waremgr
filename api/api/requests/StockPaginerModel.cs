using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.requests
{
    /// <summary>
    /// 库存分页请求
    /// </summary>
    public class StockPaginerModel : PaginerModel
    {
        /// <summary>
        /// 产品名称
        /// </summary>
        public string name { get; set; }
    }
}
