using System.ComponentModel.DataAnnotations;

namespace api.requests
{
    /// <summary>
    /// 搜索库存
    /// </summary>
    public class SearchStockModel
    {
        /// <summary>
        /// 产品名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int count { get; set; } = 7;
    }
}
