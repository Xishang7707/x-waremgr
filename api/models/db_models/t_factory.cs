using System;
using System.Collections.Generic;
using System.Text;

namespace models.db_models
{
    /// <summary>
    /// 供应商
    /// </summary>
    public class t_factory
    {
        /// <summary>
        /// 供应商id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string factory_name { get; set; }

        /// <summary>
        /// 供应商电话
        /// </summary>
        public string factory_tel { get; set; }

        /// <summary>
        /// 供应商联系人名称
        /// </summary>
        public string factory_person_name { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime add_time { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 系统删除状态
        /// </summary>
        public int state { get; set; }
    }
}
