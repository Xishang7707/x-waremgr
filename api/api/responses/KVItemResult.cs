using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.responses
{
    /// <summary>
    /// 键值对结果
    /// </summary>
    public class KVItemResult<k, v>
    {
        public k key { get; set; }
        public v value { get; set; }
    }
}
