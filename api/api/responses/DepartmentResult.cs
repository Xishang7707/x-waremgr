using models.db_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.responses
{
    /// <summary>
    /// 部门信息
    /// </summary>
    public class DepartmentResult
    {
        public int id { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 上级部门信息
        /// </summary>
        public int? parent { get; set; }

        /// <summary>
        /// 下级部门
        /// </summary>
        public IEnumerable<DepartmentResult> Childrens { get; set; }

        public DepartmentResult() { }
        public DepartmentResult(t_department model)
        {
            this.id = model.id;
            this.name = model.department_name;
            this.parent = model.department_parent;
        }
    }
}
