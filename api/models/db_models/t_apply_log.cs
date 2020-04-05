using System;
using System.Collections.Generic;
using System.Text;

namespace models.db_models
{
    /// <summary>
    /// 审批记录
    /// </summary>
    public class t_apply_log
    {
        public int id { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string order_sn { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public int user_id { get; set; }

        /// <summary>
        /// 审批状态
        /// </summary>
        public int apply_status { get; set; }

        /// <summary>
        /// 审批者职位
        /// </summary>
        public int position_id { get; set; }

        /// <summary>
        /// 审批拒绝原因
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime? add_time { get; set; }
    }
}
