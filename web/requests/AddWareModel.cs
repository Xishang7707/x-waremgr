namespace api.requests
{
    /// <summary>
    /// 添加仓库
    /// </summary>
    public class AddWareModel
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
