using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.responses
{
    public class PaginerData<T>
    {
        private int _total;
        private int _page_size;

        /// <summary>
        /// 数据
        /// </summary>
        public List<T> Data { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int page_total { get; private set; }

        /// <summary>
        /// 数据量
        /// </summary>
        public int total { get => _total; set { _total = value; page_total = GetPageTotal(); } }

        /// <summary>
        /// 页码
        /// </summary>
        public int page_index { get; set; }

        /// <summary>
        /// 一页数据
        /// </summary>
        public int page_size { get => _page_size; set { _page_size = value; page_total = GetPageTotal(); } }

        /// <summary>
        /// 计算总页数
        /// </summary>
        /// <returns></returns>
        private int GetPageTotal()
        {
            if (total <= 0 || page_size <= 0)
            {
                return 0;
            }
            return (total % page_size > 0 ? 1 : 0) + total / page_size;
        }
    }
}
