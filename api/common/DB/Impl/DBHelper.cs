using common.Config;
using common.Config.Enums;
using common.DB.Interface;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace common.DB.Impl
{
    public class DBHelper : IDbHelper
    {
        private IDbHelper _this;

        /// <summary>
        /// 数据库配置
        /// </summary>
        private DBConfig _db_config;

        /// <summary>
        /// 连接
        /// </summary>
        private IDbConnection _conn;

        /// <summary>
        /// 事务
        /// </summary>
        private IDbTransaction _tran;

        /// <summary>
        /// 数据库配置
        /// </summary>
        /// <param name="config"></param>
        public DBHelper(DBConfig config)
        {
            if (config == null)
                throw new ArgumentNullException("数据库配置不能为空");
            _this = this;
            _db_config = config;
        }

        /// <summary>
        /// 数据库标签
        /// </summary>
        /// <param name="_tag"></param>
        public DBHelper(string _tag)
        {
            _this = this;
            _db_config = GConfig.DBConfig.Get(_tag);
            if (_db_config == null)
                throw new ArgumentNullException("数据库配置不存在");
        }

        /// <summary>
        /// 检查是否连接
        /// </summary>
        /// <returns></returns>
        private bool IsConnection()
        {
            if (_conn == null)
            {
                return false;
            }
            ConnectionState[] conn_state_arr = new ConnectionState[] { ConnectionState.Executing, ConnectionState.Fetching, ConnectionState.Open };
            return conn_state_arr.Any(a => a == _conn.State);
        }

        int IDbHelper.Exec(string sql, dynamic param)
        {
            if (!IsConnection())
            {
                _this.Open();
            }
            return _conn.Execute(sql, (object)param);
        }

        IDbHelper IDbHelper.Open()
        {
            if (_conn == null)
            {
                _conn = DBFactory.Get(_db_config);
                _conn.Open();
                return this;
            }

            ConnectionState[] conn_state_arr = new ConnectionState[] { ConnectionState.Broken, ConnectionState.Closed };
            if (conn_state_arr.Any(a => a == _conn.State))
            {
                _conn.Close();
                _conn.Dispose();
                _conn = DBFactory.Get(_db_config);
                _conn.Open();
            }

            return this;
        }

        IDbHelper IDbHelper.Transaction(IsolationLevel? level)
        {
            if (!IsConnection())
            {
                _this.Open();
            }
            if (level == null)
            {
                _tran = _conn.BeginTransaction();
            }
            else
            {
                _tran = _conn.BeginTransaction(level.Value);
            }

            return this;
        }

        T IDbHelper.Query<T>(string sql, object param)
        {
            if (!IsConnection())
            {
                _this.Open();
            }

            return _conn.Query<T>(sql, param, _tran, commandTimeout: _db_config.TimeOut).FirstOrDefault();
        }

        List<T> IDbHelper.QueryList<T>(string sql, object param)
        {
            if (!IsConnection())
            {
                _this.Open();
            }

            return _conn.Query<T>(sql, param, _tran, commandTimeout: _db_config.TimeOut).ToList();
        }

        public async Task<int> ExecAsync(string sql, object param = null)
        {
            if (!IsConnection())
            {
                _this.Open();
            }
            return await _conn.ExecuteAsync(sql, (object)param, _tran);
        }

        public async Task<T> QueryAsync<T>(string sql, object param = null)
        {
            if (!IsConnection())
            {
                _this.Open();
            }

            return (await _conn.QueryAsync<T>(sql, param, _tran, commandTimeout: _db_config.TimeOut)).FirstOrDefault();
        }

        public async Task<List<T>> QueryListAsync<T>(string sql, object param = null)
        {
            if (!IsConnection())
            {
                _this.Open();
            }

            return (await _conn.QueryAsync<T>(sql, param, _tran, commandTimeout: _db_config.TimeOut)).ToList();
        }

        public void Commit()
        {
            if (_tran != null)
            {
                _tran.Commit();
                _tran.Dispose();
                _tran = null;
            }
        }

        public void Rollback()
        {
            if (_tran != null)
            {
                _tran.Rollback();
                _tran.Dispose();
                _tran = null;
            }
        }

        public EnumDBType GetDBType()
        {
            return _db_config.DbType;
        }

        public void Dispose()
        {
            if (_tran != null)
            {
                Rollback();
            }
            if (_conn != null)
            {
                _conn.Close();
                _conn.Dispose();
                _conn = null;
            }
        }

        public async Task<T> ExecScalarAsync<T>(string sql, object param = null)
        {
            if (!IsConnection())
            {
                _this.Open();
            }

            return await _conn.ExecuteScalarAsync<T>(sql, param, _tran, commandTimeout: _db_config.TimeOut);
        }
    }
}
