using common.SqlMaker.Interface.Base;

namespace common.SqlMaker.Interface
{
    /// <summary>
    /// 分组
    /// </summary>
    public interface IGroup : ISqlBase
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
        /// <param name="passcount">跳过的数据</param>
        /// <param name="count">取数据</param>
        /// <returns></returns>
        IPager Pager(int passcount, int count);
    }
}
