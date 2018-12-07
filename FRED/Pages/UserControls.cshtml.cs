using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FRED.Pages
{
    public class UserControlsModel : PageModel
    {
        public void OnGet()
        {

        }

        public void OnPostLogout()
        {
            Program.Controller.UserID = 0;
            Program.Controller.IsVerified = false;
            
            Response.Redirect("./Index");
        }
    }
}