using models.db_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.responses
{
    /// <summary>
    /// 仓库信息
    /// </summary>
    public class WareResult
    {
        public int id { get; set; }

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

        /// <summary>
        /// 状态
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime add_time { get; set; }

        public WareResult() { }
        public WareResult(t_ware model)
        {
            this.id = model.id;
            this.name = model.name;
            this.location = model.location;
            this.remark = model.remark;
            this.status = model.status;
            this.add_time = model.add_time;
        }
    }
}
