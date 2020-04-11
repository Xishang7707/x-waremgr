using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace models.enums
{
    /// <summary>
    /// 申请状态
    /// </summary>
    public enum EnumApplyStatus
    {
        /// <summary>
        /// 进行中
        /// </summary>
        [Description("进行中")]
        Progress = 0,

        /// <summary>
        /// 同意
        /// </summary>
        [Description("同意")]
        Agree = 1,

        /// <summary>
        /// 驳回
        /// </summary>
        [Description("驳回")]
        Reject = 2,
    }
}
