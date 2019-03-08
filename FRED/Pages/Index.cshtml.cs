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
                Program.Temp.SetError(null);
                Program.Controller.GetDeviceList();
                if (Program.Controller.GetDeviceStatus() == "active")
                {
                    string server = Program.Controller.GetIP();
                    int port = 21567;
                    int auxPort = 13000;
                    //Byte[] speed = System.Text.Encoding.ASCII.GetBytes("speed50");

                    try
                    {
                        Program.coreClient = new TcpClient();
                        Program.coreClient.Connect(server, auxPort);
                        Program.coreClient.Close();

                        Program.client = new TcpClient();
                        Program.client.Connect(server, port);                        
                        Program.client.Close();
                    }
                    catch
                    {
                        Program.Temp.SetErrorPanel("block");
                        Program.Temp.SetError("Cant connect to server");
                    }
                }
                else
                {
                    Program.Temp.SetErrorPanel("block");
                    Program.Temp.SetError("Cant connect to server");
                    Response.Redirect("./UserControls");
                }
                Program.Temp.SetLoginError("");
                Response.Redirect("./UserControls");
            }
            else
            {
                Program.Temp.SetLoginError("Incorrect Username and/or Password!");
                Response.Redirect("./Index");
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
                    Program.coreClient.Close();
                }
                catch
                {
                    Program.Temp.SetErrorPanel("block"); 
                    Program.Temp.SetError("Cant connect to server");
                }               
                
            }
            else
            {                
                Program.Temp.SetErrorPanel("block");
                Program.Temp.SetError("Cant connect to server");
                Response.Redirect("./UserControls");
            }            
        }
    }
}
