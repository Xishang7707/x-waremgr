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

        /// <summary>
        /// discint
        /// </summary>
        protected string _discint_sql;

        public SelectImpl()
        {
            _link_list.Add(this);
            _select_cols = new T();
        }

        public SelectImpl(Func<T, dynamic> selector)
        {
            _link_list.Add(this);
            if (selector == null)
            {
                return;
            }

            _select_cols = selector.Invoke(new T());
        }
        private SelectImpl(SelectImpl<T> obj) : base(obj._link_list)
        {
            this._select_cols = obj._select_cols;
            this._top_sql = obj._top_sql;
            this._count_sql = obj._count_sql;
            this._discint_sql = obj._discint_sql;

            if (this._link_list.FirstOrDefault(f => f is ISelect<T>) is ISelect<T> _sel)
            {
                this._link_list.Remove(_sel);
            }
            this._link_list.Add(this);
        }

        public ISelect<T> Count(string col)
        {
            return MakeLink(f => f._count_sql = $"COUNT({col})");
        }

        public ISelect<T> Distinct()
        {
            return MakeLink(f => f._discint_sql = $"DISTINCT");
        }

        public ISelect<T> Top(int count)
        {
            return MakeLink(f => f._top_sql = $"TOP {count}");
        }

        private ISelect<T> MakeLink(Action<SelectImpl<T>> func)
        {
            //构造新链，传递给下一个
            SelectImpl<T> sel = new SelectImpl<T>(this);
            func.Invoke(sel);
            return sel;
        }

        /// <summary>
        /// 生成SQL
        /// </summary>
        /// <returns></returns>
        public override string ToThisSQL()
        {
            string sql = "SELECT";
            IEnumerable<string> select_cols = null;
            if (_select_cols != null)
            {
                Type ty = _select_cols.GetType();
                select_cols = ty.GetProperties().Select(s => $"`{s.Name}`");
            }

            sql += string.IsNullOrWhiteSpace(_top_sql) ? "" : $" {_top_sql}";
            sql += string.IsNullOrWhiteSpace(_count_sql) ? "" : $" {_count_sql}";
            sql += string.IsNullOrWhiteSpace(_discint_sql) ? "" : $" {_discint_sql}";

            if (select_cols != null)
            {
                sql += $" {string.Join(",", select_cols)}";
            }

            return $"{sql} FROM `{_dt_type.Name}`";
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

        IPager ISelect<T>.Pager(int page_index, int page_size)
        {
            return new PagerImpl<T>(_link_list, page_index, page_size);
        }
    }
}
