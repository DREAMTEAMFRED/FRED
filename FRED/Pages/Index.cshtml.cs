using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FRED.Pages
{ 
    public class IndexModel : PageModel
    {               
        public void OnGet()
        {
            //Program.Controller.GetDeviceList();
            Program.FredVision.GetFacesList().Wait();
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

        public void OnPostConnect()
        {
            Program.Temp.SetError(null);
            Program.Controller.GetDeviceList();
            if (Program.Controller.GetDeviceStatus() == "active")
            {
                string server = Program.Controller.GetIP();
                int port = 13000;

                try
                {
                    Program.coreClient = new TcpClient();
                    Program.coreClient.Connect(server, port);
                    //Program.auxStream = Program.coreClient.GetStream();
                }
                catch
                {
                    Program.Temp.SetErrorPanel("none"); // make sure its set to block
                    Program.Temp.SetError("Cant connect to server");
                }
                Response.Redirect("./UserControls");
                Program.coreClient.Close();
            }
            else
            {
                // put error message to restart fred
                Program.Temp.SetErrorPanel("block");
                Program.Temp.SetError("Cant connect to server");
                Response.Redirect("./UserControls");
            }            
        }
    }
}
