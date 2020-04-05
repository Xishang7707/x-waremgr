using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Attributes
{
    /// <summary>
    /// 权限标记
    /// </summary>
    public class PrivilegeAttribute : Attribute
    {
        /// <summary>
        /// 权限key
        /// </summary>
        public string privilege_key { get; set; }

        public PrivilegeAttribute(string _privilege_key)
        {
            this.privilege_key = _privilege_key;
        }
    }

    /// <summary>
    /// 无权限标记
    /// </summary>
    public class PrivilegeAnyAttribute : Attribute
    {
    }
}
