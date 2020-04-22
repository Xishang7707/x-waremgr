using common.SqlMaker.Interface.Base;
using System;

namespace common.SqlMaker.Interface
{
    /// <summary>
    /// 条件
    /// </summary>
    public interface IWhere<T> : ISqlBase where T : new()
    {

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

        /// <summary>
        /// and
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        IWhere<T> And(string key, string rel, object val);
    }
}
