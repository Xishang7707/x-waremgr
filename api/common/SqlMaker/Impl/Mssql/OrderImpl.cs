using common.SqlMaker.Impl.Base;
using common.SqlMaker.Interface;
using System.Collections.Generic;

namespace common.SqlMaker.Impl.Mssql
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
        public OrderImpl(string sql)
        {
            SpliceSQL(sql);
        }

        public override string ToSQL()
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
            return SpliceSQL("ORDER BY " + string.Join(",", sql_list));
        }

        IOrder IOrder.OrderByAsc(string field)
        {
            if (_order_dic.ContainsKey(field))
            {
                _order_dic.Remove(field);
            }
            _order_dic.Add(field, true);
            return this;
        }


        IOrder IOrder.OrderByDesc(string field)
        {
            if (_order_dic.ContainsKey(field))
            {
                _order_dic.Remove(field);
            }
            _order_dic.Add(field, false);
            return this;
        }

        IPager IOrder.Pager(int passcount, int count)
        {
            return new PagerImpl<T>(ToSQL(), passcount, count);
        }
    }
}
