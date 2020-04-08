using System.Collections.Generic;

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
    }
}
