using common.SqlMaker.Impl.Base;
using common.SqlMaker.Interface;
using common.SqlMaker.Interface.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace common.SqlMaker.Impl.MySql
{
    class WhereImpl<T> : SqlBase<T>, IWhere<T> where T : new()
    {
        /// <summary>
        /// 拼接条件
        /// </summary>
        private Queue<WherePair> _where_que = new Queue<WherePair>();

        /// <summary>
        /// 条件
        /// </summary>
        /// <param name="_obj">继承类</param>
        private WhereImpl(WhereImpl<T> _obj) : base(_obj._link_list)
        {
            this._where_que = new Queue<WherePair>(_obj._where_que);

            if (this._link_list.FirstOrDefault(f => f is IWhere<T>) is IWhere<T> obj)
            {
                this._link_list.Remove(obj);
            }
            this._link_list.Add(this);
        }

        /// <summary>
        /// 条件
        /// </summary>
        /// <param name="_link">继承序列</param>
        /// <param name="key">字段名</param>
        /// <param name="rel">逻辑符号</param>
        /// <param name="val">值</param>
        public WhereImpl(List<ISqlBase> _link, string key, string rel, object val) : base(_link)
        {
            this._where_que.Enqueue(new WherePair
            {
                aor = 0,
                key = key,
                rel = rel,
                val = val
            });
            this._link_list.Add(this);
        }

        /// <summary>
        /// 条件
        /// </summary>
        /// <param name="_link">where</param>
        public WhereImpl(List<ISqlBase> _link) : base(_link)
        {
            this._link_list.Add(this);
        }

        public IWhere<T> And(string key, string rel, object val)
        {
            return MakeLink(f => f._where_que.Enqueue(new WherePair
            {
                aor = EnumWherePair.AND,
                key = key,
                val = val,
                rel = rel
            }));
        }

        public IWhere<T> Or(string key, string rel, object val)
        {
            return MakeLink(f => f._where_que.Enqueue(new WherePair
            {
                aor = EnumWherePair.OR,
                key = key,
                val = val,
                rel = rel
            }));
        }

        private IWhere<T> MakeLink(Action<WhereImpl<T>> func)
        {
            //构造新链，传递给下一个
            WhereImpl<T> sel = new WhereImpl<T>(this);
            func.Invoke(sel);
            return sel;
        }

        /// <summary>
        /// 生成SQL
        /// </summary>
        /// <returns></returns>
        public override string ToThisSQL()
        {
            WherePair wp;
            int index = 0;
            string sql = null;
            while (_where_que.Count > 0)
            {
                wp = _where_que.Dequeue();

                if (wp.aor == 0 || (string.IsNullOrWhiteSpace(sql) && index++ == 0))
                {
                    sql = $"`{wp.key}` {wp.rel} {wp.val}";
                }
                else
                {
                    sql += $" {wp.aor.ToString()} `{wp.key}` {wp.rel} {wp.val}";
                }
            }
            return string.IsNullOrWhiteSpace(sql) ? "" : "WHERE " + sql;
        }

        IGroup IWhere<T>.Group(Func<T, dynamic> selector)
        {
            return new GroupImpl<T>(_link_list, selector);
        }

        IOrder IWhere<T>.OrderByAsc(string field)
        {
            return (new OrderImpl<T>(_link_list) as IOrder).OrderByAsc(field);
        }

        IOrder IWhere<T>.OrderByDesc(string field)
        {
            return (new OrderImpl<T>(_link_list) as IOrder).OrderByDesc(field);
        }

        IPager IWhere<T>.Pager(int passcount, int count)
        {
            return new PagerImpl<T>(_link_list, passcount, count);
        }
    }
}
