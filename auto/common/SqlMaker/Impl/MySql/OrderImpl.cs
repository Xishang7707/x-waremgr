using common.SqlMaker.Impl.Base;
using common.SqlMaker.Interface;
using common.SqlMaker.Interface.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace common.SqlMaker.Impl.MySql
{
    /// <summary>
    /// 排序
    /// </summary>
    class OrderImpl<T> : SqlBase<T>, IOrder where T : new()
    {
        /// <summary>
        /// 排序字段 field/asc
        /// </summary>
        private Dictionary<string, bool> _order_dic = new Dictionary<string, bool>();

        /// <summary>
        /// 排序字段
        /// </summary>
        /// <param name="sql"></param>
        private OrderImpl(OrderImpl<T> _obj) : base(_obj._link_list)
        {
            this._order_dic = new Dictionary<string, bool>(_obj._order_dic);
            if (this._link_list.FirstOrDefault(f => f is IOrder) is IOrder obj)
            {
                this._link_list.Remove(obj);
            }
            this._link_list.Add(this);
        }

        /// <summary>
        /// 排序字段
        /// </summary>
        /// <param name="sql"></param>
        public OrderImpl(List<ISqlBase> _link) : base(_link)
        {
            if (this._link_list.FirstOrDefault(f => f is IOrder) is IOrder obj)
            {
                this._link_list.Remove(obj);
            }
            this._link_list.Add(this);
        }

        public override string ToThisSQL()
        {
            List<string> sql_list = new List<string>();
            foreach (var field in _order_dic.Keys)
            {
                if (_order_dic[field])
                {
                    sql_list.Add(field);
                }
                else
                {
                    sql_list.Add($"{field} DESC");
                }
            }
            return "ORDER BY " + string.Join(",", sql_list);
        }

        IOrder IOrder.OrderByAsc(string field)
        {
            return MakeLink(f =>
            {
                if (f._order_dic.ContainsKey(field))
                {
                    f._order_dic.Remove(field);
                }
                f._order_dic.Add(field, true);
            });
        }


        IOrder IOrder.OrderByDesc(string field)
        {
            return MakeLink(f =>
            {
                if (f._order_dic.ContainsKey(field))
                {
                    f._order_dic.Remove(field);
                }
                f._order_dic.Add(field, false);
            });
        }

        private IOrder MakeLink(Action<OrderImpl<T>> func)
        {
            //构造新链，传递给下一个
            OrderImpl<T> sel = new OrderImpl<T>(this);
            func.Invoke(sel);
            return sel;
        }

        IPager IOrder.Pager(int page_index, int page_size)
        {
            return new PagerImpl<T>(_link_list, page_index, page_size);
        }
    }
}
