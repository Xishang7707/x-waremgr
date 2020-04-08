namespace api.responses
{
    /// <summary>
    /// 仓库信息
    /// </summary>
    public class WareResult
    {
        /// <summary>
        /// 仓库名
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        public string location { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
    }
}
