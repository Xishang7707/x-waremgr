using System;
using System.Collections.Generic;
using System.Text;

namespace models.db_models
{
    /// <summary>
    /// 入库详情
    /// </summary>
    public class t_stock_in_detail
    {
        public int id { get; set; }

        /// <summary>
        /// 入库单号
        /// </summary>
        public string order_sn { get; set; }

        /// <summary>
        /// 库存id
        /// </summary>
        public int stock_id { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal quantity { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string product_name { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        public string unit_name { get; set; }

        /// <summary>
        /// 物料编号
        /// </summary>
        public string material_number { get; set; }

        /// <summary>
        /// 批号
        /// </summary>
        public string batch_number { get; set; }

        /// <summary>
        /// 型号
        /// </summary>
        public string model_number { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        public string package_size { get; set; }

        /// <summary>
        /// 报告单url
        /// </summary>
        public string report_card_url { get; set; }

        /// <summary>
        /// 有效期
        /// </summary>
        public DateTime? expiration_date { get; set; }

        /// <summary>
        /// 复验期
        /// </summary>
        public DateTime? retest_date { get; set; }

        /// <summary>
        /// 使用说明
        /// </summary>
        public string instructions { get; set; }

        /// <summary>
        /// 配件
        /// </summary>
        public string spare_parts { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public decimal unit_price { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 供货商id
        /// </summary>
        public int factory_id { get; set; }
    }
}
