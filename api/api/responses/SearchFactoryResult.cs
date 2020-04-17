using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.responses
{
    /// <summary>
    /// 供货商搜索结果
    /// </summary>
    public class SearchFactoryResult
    {
        public int id { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public string factory_name { get; set; }

        /// <summary>
        /// 联系人名称
        /// </summary>
        public string factory_person_name { get; set; }

        /// <summary>
        /// 联系方式
        /// </summary>
        public string factory_tel { get; set; }
    }
}
