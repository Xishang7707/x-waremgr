using System.ComponentModel.DataAnnotations;

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
        public string order_sn { get; set; }

        /// <summary>
        /// 是否同意
        /// </summary>
        public int act { get; set; }

        /// <summary>
        /// 原因
        /// </summary>
        public string remark { get; set; }
    }
}
