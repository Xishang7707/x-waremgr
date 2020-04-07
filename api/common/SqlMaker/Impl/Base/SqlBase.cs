using common.SqlMaker.Interface.Base;
using System;
using System.Collections.Generic;

namespace common.SqlMaker.Impl.Base
{
    abstract class SqlBase<T> : ISqlBase where T : new()
    {
        public SqlBase(List<ISqlBase> link = null)
        {
            _link_list = link == null ? new List<ISqlBase>() : new List<ISqlBase>(link);
        }

        /// <summary>
        /// 单元类型
        /// </summary>
        protected readonly static List<Type> _unit_type_list = new List<Type> { typeof(int), typeof(string), typeof(short), typeof(uint) };

        /// <summary>
        /// 操作实体
        /// </summary>
        protected Type _dt_type = typeof(T);

        /// <summary>
        /// 链
        /// </summary>
        protected List<ISqlBase> _link_list;

        /// <summary>
        /// 生成SQL
        /// </summary>
        /// <returns></returns>
        public abstract string ToThisSQL();

        public string ToSQL()
        {
            string sql = "";
            foreach (var item in _link_list)
            {
                sql += item.ToThisSQL() + " ";
            }
            return sql;
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
