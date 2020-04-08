using System;

namespace api.requests
{
    /// <summary>
    /// 入库单查询
    /// </summary>
    public class QueryStockInModel
    {
        /// <summary>
        /// 入库用户名查询
        /// </summary>
        public string user_name { get; set; }

        /// <summary>
        /// 审批状态
        /// </summary>
        public int? apply_status { get; set; }

        /// <summary>
        /// 起始时间
        /// </summary>
        public DateTime? start_time { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? end_time { get; set; }
    }
}
