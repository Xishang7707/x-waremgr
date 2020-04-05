using common.SqlMaker.Exception;
using common.SqlMaker.Impl.Base;
using common.SqlMaker.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace common.SqlMaker.Impl.MySql
{
    class UpdateImpl<T> : SqlBase<T>, IUpdate<T> where T : new()
    {
        /// <summary>
        /// 更新列
        /// </summary>
        protected dynamic _cols;

        public UpdateImpl(Func<T, dynamic> selector)
        {
            if (selector == null)
            {
                _cols = new T();
            }

            _cols = selector.Invoke(new T());
            Type ty = _cols.GetType();
            if (!ty.Name.StartsWith("<>") && ty.FullName != _dt_type.FullName)
            {
                throw new TypeErrorException(_dt_type, "匿名类型或者" + _dt_type.FullName);
            }
        }

        public override string ToSQL()
        {
            Type ty = _cols.GetType();
            IEnumerable<string> update_cols = ty.GetProperties().Select(s => $"`{s.Name}`=@{s.Name}");
            return SpliceSQL($@"UPDATE `{_dt_type.Name}` SET {string.Join(", ", update_cols)}");
        }

        public IWhere<T> Where(string key, string rel, object val)
        {
            return new WhereImpl<T>(ToSQL(), key, rel, val);
        }

        IWhere<T> IUpdate<T>.Where(string where_sql)
        {
            return new WhereImpl<T>(ToSQL(), where_sql);
        }
    }
}
