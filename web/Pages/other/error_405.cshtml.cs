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
    /// 未授权页面
    /// </summary>
    [AllowAnonymous]
    public class error_405Model : PageModel
    {
        public void OnGet()
        {

        }
    }
}