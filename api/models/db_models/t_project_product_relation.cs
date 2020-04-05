using System;
using System.Collections.Generic;
using System.Text;

namespace models.db_models
{
    /// <summary>
    /// 项目的产品
    /// </summary>
    public class t_project_product_relation
    {
        /// <summary>
        /// id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 产品id
        /// </summary>
        public int product_id { get; set; }

        /// <summary>
        /// 项目id
        /// </summary>
        public int project_id { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal product_quality { get; set; }
    }
}
