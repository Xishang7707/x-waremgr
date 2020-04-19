using common.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
        [Required(ErrorMessage = ErrorCodeConst.ERROR_1058)]
        public string name { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        [Required(ErrorMessage = ErrorCodeConst.ERROR_1059)]
        public string location { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
    }
}
