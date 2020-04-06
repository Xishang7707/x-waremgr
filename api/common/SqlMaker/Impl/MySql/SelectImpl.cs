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
        public override string ToSQL()
        {
            Type ty = _select_cols.GetType();
            IEnumerable<string> select_cols = ty.GetProperties().Select(s => $"`{s.Name}`");
            return SpliceSQL($"SELECT {_top_sql} {string.Join(",", select_cols)} FROM `{_dt_type.Name}`");
        }

        public IWhere<T> Where(string key, string rel, object val)
        {
            return new WhereImpl<T>(ToSQL(), key, rel, val);
        }

        IGroup ISelect<T>.Group(Func<T, dynamic> selector)
        {
            return new GroupImpl<T>(ToSQL(), selector);
        }

        IOrder ISelect<T>.OrderByAsc(string field)
        {
            return (new OrderImpl<T>(ToSQL()) as IOrder).OrderByAsc(field);
        }

        IOrder ISelect<T>.OrderByDesc(string field)
        {
            return (new OrderImpl<T>(ToSQL()) as IOrder).OrderByDesc(field);
        }

        IPager ISelect<T>.Pager(int passcount, int count)
        {
            return new PagerImpl<T>(ToSQL(), passcount, count);
        }

        /// <summary>
        /// 条件
        /// </summary>
        /// <param name="where_sql">条件SQL</param>
        /// <returns></returns>
        IWhere<T> ISelect<T>.Where(string where_sql)
        {
            return new WhereImpl<T>(ToSQL(), where_sql);
        }
    }
}
