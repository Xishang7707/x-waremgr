using common.SqlMaker.Exception;
using common.SqlMaker.Impl.Base;
using common.SqlMaker.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace common.SqlMaker.Impl.MySql
{
    class SelectImpl<T> : SqlBase<T>, ISelect<T> where T : new()
    {
        /// <summary>
        /// 查询列
        /// </summary>
        protected dynamic _select_cols;

        /// <summary>
        /// top
        /// </summary>
        protected string _top_sql;

        /// <summary>
        /// count
        /// </summary>
        protected string _count_sql;

        public SelectImpl(Func<T, dynamic> selector)
        {
            if (selector == null)
            {
                _select_cols = new T();
                return;
            }

            _select_cols = selector.Invoke(new T());
            Type ty = _select_cols.GetType();
            if (!ty.Name.StartsWith("<>") && ty.FullName != _dt_type.FullName)
            {
                throw new TypeErrorException(ty, "匿名类型或者" + _dt_type.FullName);
            }
            _link_list.Add(this);
        }

        public ISelect<T> Count(string col)
        {
            _count_sql = $"COUNT({col})";
            return this;
        }

        public ISelect<T> Top(int count)
        {
            _top_sql = $"TOP {count}";
            return this;
        }

        /// <summary>
        /// 生成SQL
        /// </summary>
        /// <returns></returns>
        public override string ToThisSQL()
        {
            Type ty = _select_cols.GetType();
            IEnumerable<string> select_cols = ty.GetProperties().Select(s => $"`{s.Name}`");
            return $"SELECT {_top_sql} {_count_sql} {string.Join(",", select_cols)} FROM `{_dt_type.Name}`";
        }

        public IWhere<T> Where(string key, string rel, object val)
        {
            return new WhereImpl<T>(_link_list, key, rel, val);
        }

        public IWhere<T> Where()
        {
            return new WhereImpl<T>(_link_list);
        }

        IGroup ISelect<T>.Group(Func<T, dynamic> selector)
        {
            return new GroupImpl<T>(_link_list, selector);
        }

        IOrder ISelect<T>.OrderByAsc(string field)
        {
            return (new OrderImpl<T>(_link_list) as IOrder).OrderByAsc(field);
        }

        IOrder ISelect<T>.OrderByDesc(string field)
        {
            return (new OrderImpl<T>(_link_list) as IOrder).OrderByDesc(field);
        }

        IPager ISelect<T>.Pager(int passcount, int count)
        {
            return new PagerImpl<T>(_link_list, passcount, count);
        }
    }
}
