using System.ComponentModel.DataAnnotations;

namespace api.requests
{
    /// <summary>
    /// 添加部门
    /// </summary>
    public class AddDepartmentModel
    {
        /// <summary>
        /// 部门名称
        /// </summary>
        public string department_name { get; set; }

        /// <summary>
        /// 上级部门
        /// </summary>
        public int? department_parent { get; set; }

        /// <summary>
        /// 启用状态
        /// </summary>
        public int status { get; set; }
    }
}
