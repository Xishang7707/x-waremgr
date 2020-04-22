using common.SqlMaker.Exception;
using common.SqlMaker.Impl.Base;
using common.SqlMaker.Interface;
using common.SqlMaker.Interface.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace common.SqlMaker.Impl.Mssql
{
    class GroupImpl<T> : SqlBase<T>, IGroup where T : new()
    {
        /// <summary>
        /// 分组列
        /// </summary>
        private dynamic _group_cols;

        public GroupImpl(List<ISqlBase> _link, Func<T, dynamic> selector) : base(_link)
        {
            _group_cols = selector.Invoke(new T());
            if (!(_group_cols.GetType() as Type).Name.StartsWith("<>"))
            {
                throw new TypeErrorException(_dt_type, "匿名类型");
            }

            if (this._link_list.FirstOrDefault(f => f is IGroup) is IGroup obj)
            {
                this._link_list.Remove(obj);
            }
            this._link_list.Add(this);
        }

        /// <summary>
        /// 生成SQL
        /// </summary>
        /// <returns></returns>
        public override string ToThisSQL()
        {
            Type _ty = _group_cols.GetType();
            IEnumerable<string> group_cols = _ty.GetProperties().Select(s => s.Name);
            return $@"GROUP BY {string.Join(",", group_cols)}";
        }

        IOrder IGroup.OrderByAsc(string field)
        {
            return (new OrderImpl<T>(_link_list) as IOrder).OrderByAsc(field);
        }

        IOrder IGroup.OrderByDesc(string field)
        {
            return (new OrderImpl<T>(_link_list) as IOrder).OrderByDesc(field);
        }

        IPager IGroup.Pager(int page_index, int page_size)
        {
            return new PagerImpl<T>(_link_list, page_index, page_size);
        }
    }
}
