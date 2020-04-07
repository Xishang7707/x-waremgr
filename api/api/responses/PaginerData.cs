using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.responses
{
    public class PaginerData<T>
    {
        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int page_total { get; set; }

        /// <summary>
        /// 数据量
        /// </summary>
        public int total { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        public int page_index { get; set; }

        /// <summary>
        /// 一页数据
        /// </summary>
        public int page_size { get; set; }
    }
}
