using common.SqlMaker.Interface.Base;
using System;
using System.Collections.Generic;

namespace common.SqlMaker.Impl.Base
{
    abstract class SqlBase<T> : ISqlBase where T : new()
    {
        /// <summary>
        /// 单元类型
        /// </summary>
        protected readonly static List<Type> _unit_type_list = new List<Type> { typeof(int), typeof(string), typeof(short), typeof(uint) };

        /// <summary>
        /// 操作实体
        /// </summary>
        protected Type _dt_type = typeof(T);

        /// <summary>
        /// 生成中的SQL
        /// </summary>
        private string _making_sql;

        /// <summary>
        /// 生成SQL
        /// </summary>
        /// <returns></returns>
        public abstract string ToSQL();

        /// <summary>
        /// 连接SQL
        /// </summary>
        /// <param name="sql"></param>
        protected string SpliceSQL()
        {
            return _making_sql;
        }

        /// <summary>
        /// 连接SQL
        /// </summary>
        /// <param name="sql"></param>
        protected string SpliceSQL(string sql)
        {
            return _making_sql += string.IsNullOrWhiteSpace(_making_sql) || string.IsNullOrWhiteSpace(sql) ? sql : " " + sql;
        }
    }

    public enum EnumWherePair
    {
        AND = 1,
        OR = 2
    }

    public class WherePair
    {
        public string key { get; set; }
        public object val { get; set; }
        public string rel { get; set; }
        public EnumWherePair aor { get; set; }
    }
}
