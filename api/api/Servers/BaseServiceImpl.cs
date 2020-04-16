using api.Servers.LogServer.Impl;
using api.Servers.LogServer.Interface;
using common.Config;
using common.DB.Impl;
using common.DB.Interface;
using common.SqlMaker;
using common.SqlMaker.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Servers
{
    public abstract class BaseServiceImpl
    {
        protected ISqlMaker _g_sqlMaker;
        protected IDbHelper g_dbHelper { get; }//数据库操作
        protected ILogServer g_logServer { get; }//日志操作
        protected ISqlMaker g_sqlMaker { get { lock (this) { if (_g_sqlMaker == null) { _g_sqlMaker = SqlMakerFactory.GetSqlMaker(g_dbHelper.GetDBType()); } } return _g_sqlMaker; } set { lock (this) { _g_sqlMaker = value; } } }//SQL 生成器
        public BaseServiceImpl(IDbHelper dbHelper = null, ILogServer logServer = null)
        {
            this.g_dbHelper = dbHelper;
            this.g_logServer = logServer;
        }
    }

    /// <summary>
    /// 服务返回结果
    /// </summary>
    public class SVResult
    {
        /// <summary>
        /// 执行状态
        /// </summary>
        public bool state { get; set; }

        /// <summary>
        /// 状态码信息
        /// </summary>
        public string code { get; set; }
    }

    public class SVResult<T> : SVResult
    {
        public T data { get; set; }
    }
}
