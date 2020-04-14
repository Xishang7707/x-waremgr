using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace models.enums
{
    /// <summary>
    /// 审核状态
    /// </summary>
    public enum EnumAuditStatus
    {
        /// <summary>
        /// 驳回
        /// </summary>
        [Description("驳回")]
        Reject = 0,

        /// <summary>
        /// 同意
        /// </summary>
        [Description("同意")]
        Agree = 1
    }
}
