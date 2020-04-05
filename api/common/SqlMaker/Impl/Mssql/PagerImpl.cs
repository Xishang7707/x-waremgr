using common.SqlMaker.Impl.Base;
using common.SqlMaker.Interface;

namespace common.SqlMaker.Impl.Mssql
{
    /// <summary>
    /// 分页
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class PagerImpl<T> : SqlBase<T>, IPager where T : new()
    {
        /// <summary>
        /// 跳过数量
        /// </summary>
        private int _passcount;

        /// <summary>
        /// 取数量
        /// </summary>
        private int _count;

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="sql">前置SQL</param>
        /// <param name="passcount">跳过数量</param>
        /// <param name="count">取数量</param>
        public PagerImpl(string sql, int passcount, int count)
        {
            SpliceSQL(sql);
            _passcount = passcount;
            _count = count;
        }

        public override string ToSQL()
        {
            return SpliceSQL($@"OFFSET {_passcount} ROWS FETCH NEXT {_count} ROWS ONLY");
        }
    }
}
