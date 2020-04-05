using System;
using System.Collections.Generic;
using System.Text;

namespace models.enums
{
    /// <summary>
    /// 订单类型
    /// </summary>
    public enum EnumOrderType
    {
        /// <summary>
        /// 入库
        /// </summary>
        IN = 1,

        /// <summary>
        /// 出库
        /// </summary>
        OT = 2,

        /// <summary>
        /// 采购
        /// </summary>
        PS = 3,

        /// <summary>
        /// 借货
        /// </summary>
        BG = 4,

        /// <summary>
        /// 还货
        /// </summary>
        EG = 5
    }
}
