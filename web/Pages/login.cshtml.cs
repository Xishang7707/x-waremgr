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
    /// 登录
    /// </summary>
    [AllowAnonymous]
    public class loginModel : PageModel
    {
        public void OnGet()
        {

        }
    }
}