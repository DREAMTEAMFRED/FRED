﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FRED.Pages
{ 
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
            /// this is testing git hub
           //this is a joint project - testing
        }

        public void OnPostLogin(string username, string password)
        {
            Program.Controller.CheckID(username, password);
            if(Program.Controller.IsVerified)
            {
                Response.Redirect("./UserControls");
            }
            else
            {
                Response.Redirect("./Error");
            }
            
        }
    }
}