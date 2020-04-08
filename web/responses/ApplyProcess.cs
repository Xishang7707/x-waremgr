using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.responses
{
    /// <summary>
    /// 审批流程进度
    /// </summary>
    public class ApplyProcess
    {
        /// <summary>
        /// 职位名称
        /// </summary>
        public string position_name { get; set; }

        /// <summary>
        /// 审批状态 EnumApplyStatus
        /// </summary>
        public int? audit_status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 审批者
        /// </summary>
        public string auditer { get; set; }

        /// <summary>
        /// 审批时间
        /// </summary>
        public string audit_time { get; set; }
    }
}
