using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace web
{
    /// <summary>
    /// 错误页面
    /// </summary>
    [AllowAnonymous]
    public class error_500Model : PageModel
    {
        public void OnGet()
        {

        }
    }
}