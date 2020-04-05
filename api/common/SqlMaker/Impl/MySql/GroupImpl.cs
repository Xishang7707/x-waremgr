using common.SqlMaker.Exception;
using common.SqlMaker.Impl.Base;
using common.SqlMaker.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace common.SqlMaker.Impl.MySql
{
    class GroupImpl<T> : SqlBase<T>, IGroup where T : new()
    {
        /// <summary>
        /// 分组列
        /// </summary>
        private dynamic _group_cols;

        public GroupImpl(string sql, Func<T, dynamic> selector)
        {
            SpliceSQL(sql);
            _group_cols = selector.Invoke(new T());
            if (!(_group_cols.GetType() as Type).Name.StartsWith("<>"))
            {
                throw new TypeErrorException(_dt_type, "匿名类型");
            }
        }

        /// <summary>
        /// 生成SQL
        /// </summary>
        /// <returns></returns>
        public override string ToSQL()
        {
            Type _ty = _group_cols.GetType();
            IEnumerable<string> group_cols = _ty.GetProperties().Select(s => s.Name);
            return SpliceSQL($@"GROUP BY {string.Join(",", group_cols)}");
        }

        IOrder IGroup.OrderByAsc(string field)
        {
            return (new OrderImpl<T>(ToSQL()) as IOrder).OrderByAsc(field);
        }

        IOrder IGroup.OrderByDesc(string field)
        {
            return (new OrderImpl<T>(ToSQL()) as IOrder).OrderByDesc(field);
        }

        IPager IGroup.Pager(int passcount, int count)
        {
            return new PagerImpl<T>(ToSQL(), passcount, count);
        }
    }
}
