using common.SqlMaker.Impl.Base;
using common.SqlMaker.Interface;
using common.SqlMaker.Interface.Base;
using System.Collections.Generic;
using System.Linq;

namespace common.SqlMaker.Impl.MySql
{
    /// <summary>
    /// 分页
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class PagerImpl<T> : SqlBase<T>, IPager where T : new()
    {
        /// <summary>
        /// 跳过数量
        /// </summary>
        private int _passcount;

        /// <summary>
        /// 取数量
        /// </summary>
        private int _count;

        public PagerImpl(List<ISqlBase> _link, int page_index, int page_size) : base(_link)
        {
            _passcount = (page_index - 1) * page_size;
            _count = page_size;

            if (this._link_list.FirstOrDefault(f => f is IPager) is IPager obj)
            {
                this._link_list.Remove(obj);
            }
            this._link_list.Add(this);
        }

        public override string ToThisSQL()
        {
            return $@"LIMIT {_passcount},{_count}";
        }
    }
}
