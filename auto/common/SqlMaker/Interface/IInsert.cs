using common.SqlMaker.Interface.Base;

namespace common.SqlMaker.Interface
{
    /// <summary>
    /// 插入
    /// </summary>
    public interface IInsert<T> : ISqlBase where T : new() { }
}
