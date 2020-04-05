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
        /// 条件
        /// </summary>
        /// <param name="where_sql">条件SQL</param>
        /// <returns></returns>
        [Obsolete("不再使用")]
        IWhere<T> Where(string where_sql);

        /// <summary>
        /// 条件
        /// </summary>
        /// <param name="key"></param>
        /// <param name="rel"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IWhere<T> Where(string key, string rel, object val);

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
        /// <param name="passcount">跳过数量</param>
        /// <param name="count">取数量</param>
        /// <returns></returns>
        IPager Pager(int passcount, int count);
    }
}
