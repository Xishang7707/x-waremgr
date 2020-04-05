using common.SqlMaker.Impl.Base;
using common.SqlMaker.Interface;
using System;
using System.Collections.Generic;

namespace common.SqlMaker.Impl.MySql
{
    class WhereImpl<T> : SqlBase<T>, IWhere<T> where T : new()
    {
        /// <summary>
        /// 条件语句
        /// </summary>
        private string _where_sql;

        /// <summary>
        /// 拼接条件
        /// </summary>
        private Queue<WherePair> _where_que;

        /// <summary>
        /// 条件
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="where_sql">条件语句</param>
        public WhereImpl(string sql, string where_sql = null)
        {
            SpliceSQL(sql);
            _where_sql = where_sql;
        }

        /// <summary>
        /// 条件
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="where_sql">条件语句</param>
        public WhereImpl(string sql, string key, string rel, object val)
        {
            _where_que = new Queue<WherePair>();

            SpliceSQL(sql);
            _where_que.Enqueue(new WherePair
            {
                aor = 0,
                key = key,
                rel = rel,
                val = val
            });
        }

        public IWhere<T> And(string key, string rel, object val)
        {
            _where_que.Enqueue(new WherePair
            {
                aor = EnumWherePair.AND,
                key = key,
                val = val,
                rel = rel
            });
            return this;
        }

        public IWhere<T> Or(string key, string rel, object val)
        {
            _where_que.Enqueue(new WherePair
            {
                aor = EnumWherePair.OR,
                key = key,
                val = val,
                rel = rel
            });
            return this;
        }

        /// <summary>
        /// 生成SQL
        /// </summary>
        /// <returns></returns>
        public override string ToSQL()
        {
            WherePair wp;
            while (_where_que.Count > 0)
            {
                wp = _where_que.Dequeue();
                if (wp.aor == 0)
                {
                    _where_sql = $"`{wp.key}` {wp.rel} {wp.val}";
                }
                else
                {
                    _where_sql += $" {wp.aor.ToString()} [{wp.key}] {wp.rel} {wp.val}";
                }
            }
            return SpliceSQL("WHERE " + _where_sql);
        }

        IGroup IWhere<T>.Group(Func<T, dynamic> selector)
        {
            return new GroupImpl<T>(ToSQL(), selector);
        }

        IOrder IWhere<T>.OrderByAsc(string field)
        {
            return (new OrderImpl<T>(ToSQL()) as IOrder).OrderByAsc(field);
        }

        IOrder IWhere<T>.OrderByDesc(string field)
        {
            return (new OrderImpl<T>(ToSQL()) as IOrder).OrderByDesc(field);
        }

        IPager IWhere<T>.Pager(int passcount, int count)
        {
            return new PagerImpl<T>(ToSQL(), passcount, count);
        }
    }
}
