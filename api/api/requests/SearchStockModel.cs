using common.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.requests
{
    /// <summary>
    /// 搜索库存
    /// </summary>
    public class SearchStockModel
    {
        /// <summary>
        /// 产品名称
        /// </summary>
        [Required(ErrorMessage = ErrorCodeConst.ERROR_1044)]
        public string name { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int count { get; set; } = 7;
    }
}
