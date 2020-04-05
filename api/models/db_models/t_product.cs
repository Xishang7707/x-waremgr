using System;
using System.Collections.Generic;
using System.Text;

namespace models.db_models
{
    /// <summary>
    /// 产品
    /// </summary>
    public class t_product
    {
        /// <summary>
        /// 产品id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string product_name { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime add_time { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 删除状态
        /// </summary>
        public int state { get; set; }
    }
}
