using System;
using System.Collections.Generic;
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
        Progress = 0,

        /// <summary>
        /// 同意
        /// </summary>
        Agree = 1,

        /// <summary>
        /// 驳回
        /// </summary>
        Reject = 2,
    }
}
