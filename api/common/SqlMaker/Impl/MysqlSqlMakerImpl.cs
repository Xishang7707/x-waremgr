using common.SqlMaker.Impl.MySql;
using common.SqlMaker.Interface;
using System;

namespace common.SqlMaker.Impl
{
    class MysqlSqlMakerImpl : ISqlMaker
    {
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
