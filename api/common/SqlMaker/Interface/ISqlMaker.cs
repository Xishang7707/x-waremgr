using System;

namespace common.SqlMaker.Interface
{
    /// <summary>
    /// SQL语句生成器
    /// </summary>
    public interface ISqlMaker
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="selector">列</param>
        /// <returns></returns>
        ISelect<T> Select<T>(Func<T, dynamic> selector = null) where T : new();

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="selector">数据实体</param>
        /// <returns></returns>
        IUpdate<T> Update<T>(Func<T, dynamic> selector) where T : new();

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="id">主键id</param>
        /// <returns></returns>
        IDelete<T> Delete<T>() where T : new();

        /// <summary>
        /// 插入
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="selector">数据实体</param>
        /// <returns></returns>
        IInsert<T> Insert<T>(Func<T, dynamic> selector) where T : new();
    }
}
