using common.SqlMaker.Interface.Base;

namespace common.SqlMaker.Interface
{
    /// <summary>
    /// 排序
    /// </summary>
    public interface IOrder : ISqlBase
    {
        /// <summary>
        /// 升序
        /// </summary>
        /// <param name="field">字段</param>
        /// <returns></returns>
        IOrder OrderByAsc(string field);

        /// <summary>
        /// 降序
        /// </summary>
        /// <param name="field">字段</param>
        /// <returns></returns>
        IOrder OrderByDesc(string field);

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="page_index">页码</param>
        /// <param name="page_size">数量</param>
        /// <returns></returns>
        IPager Pager(int page_index, int page_size);
    }
}
