using common.SqlMaker.Impl.Mssql;
using common.SqlMaker.Interface;
using System;

namespace common.SqlMaker.Impl
{
    /// <summary>
    /// MSSQL 语句
    /// </summary>
    class MssqlSqlMakerImpl : ISqlMaker
    {
        public ISelect<T> Top<T>(Func<T, dynamic> selector = null) where T : new()
        {
            throw new NotImplementedException();
        }

        IDelete<T> ISqlMaker.Delete<T>()
        {
            return new DeleteImpl<T>();
        }

        IInsert<T> ISqlMaker.Insert<T>(Func<T, dynamic> selector)
        {
            return new InsertImpl<T>(selector);
        }

        ISelect<T> ISqlMaker.Select<T>(Func<T, dynamic> selector)
        {
            return new SelectImpl<T>(selector);
        }

        IUpdate<T> ISqlMaker.Update<T>(Func<T, dynamic> selector)
        {
            return new UpdateImpl<T>(selector);
        }
    }
}
