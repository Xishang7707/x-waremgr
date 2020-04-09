using common.Config.Enums;
using common.SqlMaker.Impl;
using common.SqlMaker.Interface;

namespace common.SqlMaker
{
    /// <summary>
    /// SQL生成器工厂
    /// </summary>
    public static class SqlMakerFactory
    {
        /// <summary>
        /// 获取SQL生成器
        /// </summary>
        /// <returns></returns>
        public static ISqlMaker GetSqlMaker(EnumDBType db_type)
        {
            switch (db_type)
            {
                case EnumDBType.MYSQL:
                    return new MysqlSqlMakerImpl();
                case EnumDBType.MSSQL:
                    return new MssqlSqlMakerImpl();
                default:
                    return null;
            }
        }
    }
}
