using common.SqlMaker.Impl.Base;
using common.SqlMaker.Interface;

namespace common.SqlMaker.Impl.MySql
{
    /// <summary>
    /// 删除
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class DeleteImpl<T> : SqlBase<T>, IDelete<T> where T : new()
    {

        public DeleteImpl() { }

        public override string ToThisSQL()
        {
            return $@"DELETE FROM `{_dt_type.Name}`";
        }

        IWhere<T> IDelete<T>.Where(string key, string rel, object val)
        {
            return new WhereImpl<T>(_link_list, key, rel, val);
        }
    }
}
