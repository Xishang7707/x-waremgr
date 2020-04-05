using System;
using System.Collections.Generic;
using System.Text;

namespace models.db_models
{
    /// <summary>
    /// 入库
    /// </summary>
    public class t_stock_in
    {
        public int id { get; set; }

        /// <summary>
        /// 单号
        /// </summary>
        public string order_sn { get; set; }

        /// <summary>
        /// 入库申请人
        /// </summary>
        public int in_user_id { get; set; }

        /// <summary>
        /// 发起申请时间
        /// </summary>
        public DateTime? add_time { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 部门id
        /// </summary>
        public int department_id { get; set; }

        /// <summary>
        /// 职位id
        /// </summary>
        public int position_id { get; set; }

        /// <summary>
        /// 审批状态 0进行中 1通过 2失败
        /// </summary>
        public int apply_status { get; set; }

        /// <summary>
        /// 审批进度
        /// </summary>
        public int apply_process { get; set; }
    }
}
