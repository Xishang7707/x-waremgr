using common.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.requests
{
    /// <summary>
    /// 审批
    /// </summary>
    public class AuditModel
    {
        /// <summary>
        /// 订单号
        /// </summary>
        [Required(ErrorMessage = ErrorCodeConst.ERROR_1048)]
        public string order_sn { get; set; }

        /// <summary>
        /// 是否同意
        /// </summary>
        public int act { get; set; }

        /// <summary>
        /// 原因
        /// </summary>
        [MaxLength(100, ErrorMessage = ErrorCodeConst.ERROR_1049)]
        public string remark { get; set; }
    }
}
