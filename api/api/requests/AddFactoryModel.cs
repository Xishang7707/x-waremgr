using common.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.requests
{
    /// <summary>
    /// 添加供应商
    /// </summary>
    public class AddFactoryModel
    {
        /// <summary>
        /// 供应商名称
        /// </summary>
        [Required(ErrorMessage = ErrorCodeConst.ERROR_1025)]
        public string factory_name { get; set; }

        /// <summary>
        /// 供应商电话
        /// </summary>
        [Required(ErrorMessage = ErrorCodeConst.ERROR_1026)]
        public string factory_tel { get; set; }

        /// <summary>
        /// 供应商联系人名称
        /// </summary>
        [Required(ErrorMessage = ErrorCodeConst.ERROR_1027)]
        public string factory_person_name { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int? status { get; set; }
    }
}
