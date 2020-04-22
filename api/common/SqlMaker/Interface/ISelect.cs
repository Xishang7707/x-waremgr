using common.SqlMaker.Interface.Base;
using System;

namespace common.SqlMaker.Interface
{
    /// <summary>
    /// 查询
    /// </summary>
    public interface ISelect<T> : ISqlBase where T : new()
    {
        /// <summary>
        /// 取前n个
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        ISelect<T> Top(int count);

        /// <summary>
        /// 数据量
        /// </summary>
        /// <param name="col">列名</param>
        /// <returns></returns>
        ISelect<T> Count(string col = "1");

        /// <summary>
        /// 去重
        /// </summary>
        /// <returns></returns>
        ISelect<T> Distinct();

        /// <summary>
        /// 条件
        /// </summary>
        /// <param name="key"></param>
        /// <param name="rel"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IWhere<T> Where(string key, string rel, object val);

        /// <summary>
        /// 条件
        /// </summary>
        /// <returns></returns>
        IWhere<T> Where();

        /// <summary>
        /// 排序-升序
        /// </summary>
        /// <param name="field">字段</param>
        /// <returns></returns>
        IOrder OrderByAsc(string field);

        /// <summary>
        /// 排序-降序
        /// </summary>
        /// <param name="field">字段</param>
        /// <returns></returns>
        IOrder OrderByDesc(string field);

        /// <summary>
        /// 分组
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        IGroup Group(Func<T, dynamic> selector);

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="page_index">页码</param>
        /// <param name="page_size">数量</param>
        /// <returns></returns>
        IPager Pager(int page_index, int page_size);
    }
}
