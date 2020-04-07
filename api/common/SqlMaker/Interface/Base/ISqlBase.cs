namespace common.SqlMaker.Interface.Base
{
    public interface ISqlBase
    {
        /// <summary>
        /// 生成SQL
        /// </summary>
        /// <returns></returns>
        string ToSQL();

        /// <summary>
        /// 生成当前类的SQL
        /// </summary>
        /// <returns></returns>
        string ToThisSQL();
    }
}
