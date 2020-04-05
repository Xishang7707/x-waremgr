using common.Config;
using common.Config.Enums;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.SqlClient;

namespace common.DB
{
    /// <summary>
    /// 数据库构造工厂
    /// </summary>
    public static class DBFactory
    {
        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IDbConnection Get(DBConfig config)
        {
            IDbConnection conn = null;
            switch (config.DbType)
            {
                case EnumDBType.MYSQL:
                    conn = new MySqlConnection(config.Conn);
                    break;
                case EnumDBType.MSSQL:
                    conn = new SqlConnection(config.Conn);
                    break;
            }

            return conn;
        }
    }
}
