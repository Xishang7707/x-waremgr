using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.responses
{
    /// <summary>
    /// 搜索入库单结果
    /// </summary>
    public class SearchStockInResult
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string order_sn { get; set; }

        /// <summary>
        /// 申请时间
        /// </summary>
        public string add_time { get; set; }

        /// <summary>
        /// 审批状态
        /// </summary>
        public int apply_status { get; set; }

        /// <summary>
        /// 审批状态 文字
        /// </summary>
        public string apply_status_desc { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string depart_name { get; set; }

        /// <summary>
        /// 申请人
        /// </summary>
        public string applyer { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        public string job_number { get; set; }

        /// <summary>
        /// 审批到的步骤 第一步(0)、第二步(1)
        /// </summary>
        public int audit_step_index { get; set; }

        /// <summary>
        /// 是否可以审批操作
        /// </summary>
        public bool op_audit { get; set; }

        /// <summary>
        /// 审批列表
        /// </summary>
        public IEnumerable<ApplyProcess> audit_list { get; set; }
    }
}
