using common.SqlMaker.Exception;
using common.SqlMaker.Impl.Base;
using common.SqlMaker.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace common.SqlMaker.Impl.MySql
{
    /// <summary>
    /// MySql插入实现
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class InsertImpl<T> : SqlBase<T>, IInsert<T> where T : new()
    {
        /// <summary>
        /// 插入的列
        /// </summary>
        protected dynamic _insert_cols;

        public InsertImpl(Func<T, dynamic> selector)
        {
            _insert_cols = selector.Invoke(new T());
            Type ty = _insert_cols.GetType();
            if (!ty.Name.StartsWith("<>") && ty.FullName != _dt_type.FullName)
            {
                throw new TypeErrorException(_dt_type, "匿名类型或者" + _dt_type.FullName);
            }
            _link_list.Add(this);
        }

        /// <summary>
        /// 生成SQL
        /// </summary>
        /// <returns>SQL</returns>
        public override string ToThisSQL()
        {
            Dictionary<string, string> paramsDic = new Dictionary<string, string>();
            Type ty = _insert_cols.GetType();

            IEnumerable<string> cols = ty.GetProperties().Select(s => s.Name);
            IEnumerable<string> cols_value = cols.Select(s => $@"@{s}");
            cols = cols.Select(s => $@"`{s}`");

            return $@"INSERT `{typeof(T).Name}`({string.Join(",", cols)}) VALUES({string.Join(",", cols_value)});SELECT LAST_INSERT_ID();";
        }
    }
}
