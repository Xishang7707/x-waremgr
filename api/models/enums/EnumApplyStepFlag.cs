using System;
using System.Collections.Generic;
using System.Text;

namespace models.enums
{
    /// <summary>
    /// 审批进度标记
    /// </summary>
    public enum EnumApplyStepFlag
    {
        /// <summary>
        /// 开始
        /// </summary>
        Start = 0,

        /// <summary>
        /// 进行中
        /// </summary>
        Progress = 1,

        /// <summary>
        /// 结束
        /// </summary>
        End = 2
    }
}
