using common.Consts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.requests
{
    /// <summary>
    /// 存放
    /// </summary>
    public class StockDepositModel
    {
        [Required(ErrorMessage = ErrorCodeConst.ERROR_1053)]
        public int id { get; set; }

        /// <summary>
        /// 仓库Id
        /// </summary>
        [Required(ErrorMessage = ErrorCodeConst.ERROR_1051)]
        public int ware_id { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal quantity { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        [Required(ErrorMessage = ErrorCodeConst.ERROR_1055)]
        [MaxLength(100, ErrorMessage = ErrorCodeConst.ERROR_1057)]
        public string location { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(100, ErrorMessage = ErrorCodeConst.ERROR_1049)]
        public string remark { get; set; }
    }
}
