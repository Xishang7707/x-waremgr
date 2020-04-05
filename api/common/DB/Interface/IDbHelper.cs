using common.Config.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace common.DB.Interface
{
    /// <summary>
    /// 数据库操作接口
    /// </summary>
    public interface IDbHelper : IDisposable
    {
        /// <summary>
        /// 打开连接
        /// </summary>
        /// <returns></returns>
        IDbHelper Open();

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        int Exec(string sql, object param = null);

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        Task<int> ExecAsync(string sql, object param = null);

        /// <summary>
        /// 查询单条
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        T Query<T>(string sql, object param = null);

        /// <summary>
        /// 查询单条
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        Task<T> QueryAsync<T>(string sql, object param = null);

        /// <summary>
        /// 查询单条
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        List<T> QueryList<T>(string sql, object param = null);

        /// <summary>
        /// 查询单条
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        Task<List<T>> QueryListAsync<T>(string sql, object param = null);

        /// <summary>
        /// 语句组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<T> ExecScalarAsync<T>(string sql, object param = null);

        /// <summary>
        /// 事务
        /// </summary>
        /// <param name="level">隔离等级</param>
        /// <returns></returns>
        IDbHelper Transaction(IsolationLevel? level = null);

        /// <summary>
        /// 提交
        /// </summary>
        /// <returns></returns>
        void Commit();

        /// <summary>
        /// 回滚
        /// </summary>
        /// <returns></returns>
        void Rollback();

        /// <summary>
        /// 获取数据库类型
        /// </summary>
        /// <returns></returns>
        EnumDBType GetDBType();
    }
}
