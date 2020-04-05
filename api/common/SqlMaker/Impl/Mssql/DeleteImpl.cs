using common.SqlMaker.Impl.Base;
using common.SqlMaker.Interface;

namespace common.SqlMaker.Impl.Mssql
{
    /// <summary>
    /// 删除
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class DeleteImpl<T> : SqlBase<T>, IDelete<T> where T : new()
    {

        public DeleteImpl() { }

        public override string ToSQL()
        {
            return SpliceSQL($@"DELETE [{_dt_type.Name}]");
        }

        public IWhere<T> Where(string key, string rel, object val)
        {
            return new WhereImpl<T>(ToSQL(), key, rel, val);
        }

        IWhere<T> IDelete<T>.Where(string where_sql)
        {
            return new WhereImpl<T>(ToSQL(), where_sql);
        }
    }
}
