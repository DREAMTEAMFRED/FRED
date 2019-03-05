using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FRED.Pages
{
    public class UserControlsModel : PageModel
    {        
        public string serverIP = Program.Controller.GetIP();                      
        public string error = Program.Temp.GetError();
        public string displayErrorPanel = Program.Temp.GetErrorPanelStatus();
        public string displayControlPanel = Program.Temp.GetContorlPanelStatus();
        public string displayVisionPanel = Program.Temp.GetVisionPanelStatus();
        public string displayFacePanel = Program.Temp.GetFacePanelStatus();
        public string displayTakePicPanel = Program.Temp.GetTakePicPanelStatus();
        public string light = Program.Temp.GetLightStatus();
        public int mySpeed = Program.Temp.GetSpeed();
        public string displayTextPanel = Program.Temp.GetTextPanelStatus();
        public string displayText = Program.Temp.GetDisplayText();

        public void OnGet()
        {            
            Program.Temp.SetTextPanel("none");
        }        

        public void OpenErrorPanel()
        {
            Program.Temp.SetErrorPanel("block");
            Program.Temp.SetFacePanel("none");
            Program.Temp.SetVisionPanel("none");
            Program.Temp.SetControlPanel("none");            
        }

        public void OnPostCloseWindow()
        {
            Program.Temp.SetTakePicPanel("none");
            Program.Temp.SetPicStatus("");
            Program.FredVision.GetFacesList().Wait();
            Program.Temp.SetFacePanel("block");
            Response.Redirect("./UserControls");
        }

        public void OnPostOpenAddFacePanel()
        {          
            if (displayFacePanel == "none")                            
                Program.Temp.SetFacePanel("block");                           
            else
                Program.Temp.SetFacePanel("none");

            Program.Temp.SetControlPanel("none");
            Program.Temp.SetVisionPanel("none");
            Response.Redirect("./UserControls");
        }

        public void OnPostCloseAddFacePanel()
        {
            Program.Temp.SetFacePanel("none");
            Response.Redirect("./UserControls");
        }

        public void OnPostOpenControlPanel()
        {
            if (displayControlPanel == "none")
                Program.Temp.SetControlPanel("grid");
            else
                Program.Temp.SetControlPanel("none");

            Program.Temp.SetFacePanel("none");
            Program.Temp.SetVisionPanel("none");
            Response.Redirect("./UserControls");
        }

        public void OnPostCloseControlPanel()
        {
            Program.Temp.SetControlPanel("none");
            Response.Redirect("./UserControls");
        }

        public void OnPostOpenVisionPanel()
        {
            if (displayVisionPanel == "none")
                Program.Temp.SetVisionPanel("grid");
            else
                Program.Temp.SetVisionPanel("none");

            Program.Temp.SetFacePanel("none");
            Program.Temp.SetControlPanel("none");
            Response.Redirect("./UserControls");
        }

        public void OnPostCloseVisionPanel()
        {
            Program.Temp.SetVisionPanel("none");
            Response.Redirect("./UserControls");
        }

        public void OnPostLogout()
        {
            Program.Controller.UserID = 0;
            Program.Controller.IsVerified = false;

            Response.Redirect("./Index");
        }

        public void OnPostConnect(string ipAddress)
        {            
            string server = Program.Controller.GetIP();
            int port = 21567;
            Byte[] speed = System.Text.Encoding.ASCII.GetBytes("speed50");

            try
            {
                Program.client = new TcpClient();
                Program.client.Connect(server, port);
                Program.stream = Program.client.GetStream();
                Program.stream.Write(speed);
            }
            catch 
            {
                Program.Temp.SetError("Cant connect to server");
            }            
        }

        public void OnPostDisconnect()
        {
            Byte[] stop = System.Text.Encoding.ASCII.GetBytes("stop");
            if (Program.stream != null)
            {
                Program.stream.Write(stop);
                Program.stream.Close();
            }
            if (Program.client != null)
                Program.client.Close();
        }

        public void OnPostAuxConnect()
        {
            Program.Temp.SetError(null);
            Program.Controller.GetDeviceList();
            string server = Program.Controller.GetIP();
            int port = 13000;

            try
            {
                Program.coreClient = new TcpClient();
                Program.coreClient.Connect(server, port);
                Program.auxStream = Program.coreClient.GetStream();
                Program.Temp.SetErrorPanel("none");                
            }
            catch 
            {
                Program.Temp.SetError("Cant connect to server");
                OpenErrorPanel();
            }
            
            //Response.Redirect("./UserControls");
        }

        public void OnPostAuxDisconnect()
        {
            Byte[] stop = System.Text.Encoding.ASCII.GetBytes("stop");
            if (Program.auxStream.CanWrite)
            {
                Program.auxStream.Write(stop);
                Program.auxStream.Close();
            }
            if (Program.client != null)
                Program.coreClient.Close();
        }

        public void OnPostLight(string toggleLight)
        {
            if (toggleLight == "on")
            {                
                try
                {
                    Program.Temp.SetLight("on");
                    Byte[] cmd = System.Text.Encoding.ASCII.GetBytes("light-on");
                    Program.auxStream.Write(cmd);
                }
                catch
                {
                    Program.Temp.SetError("lost connection to the server");
                    OpenErrorPanel();
                }                              
            }
            else
            {
                try
                {
                    Program.Temp.SetLight("off");
                    Byte[] cmd = System.Text.Encoding.ASCII.GetBytes("light-off");
                    Program.auxStream.Write(cmd);
                }
                catch
                {
                    Program.Temp.SetError("lost connection to the server");
                    OpenErrorPanel();
                }                                         
            }
            Response.Redirect("./UserControls");
        }               

        public void OnPostSetSpeed(int speed)
        {
            Program.Temp.SetSpeed(speed);
            mySpeed = speed;
            string curSpeed = "speed" + mySpeed;
            Byte[] setSpeed = System.Text.Encoding.ASCII.GetBytes(curSpeed);

            if (Program.stream != null)
            {
                Program.stream.Write(setSpeed);
            }
        }

        public void OnPostControl(string cmd)
        {
            Byte[] forward = System.Text.Encoding.ASCII.GetBytes("forward");
            Byte[] backward = System.Text.Encoding.ASCII.GetBytes("backward");
            Byte[] left = System.Text.Encoding.ASCII.GetBytes("left");
            Byte[] right = System.Text.Encoding.ASCII.GetBytes("right");
            Byte[] stop = System.Text.Encoding.ASCII.GetBytes("stop");
            Byte[] home = System.Text.Encoding.ASCII.GetBytes("home");

            if (Program.stream != null)
            {
                switch (cmd)
                {
                    case "Forward":
                        {
                            Program.stream.Write(forward);
                            break;
                        }
                    case "Backward":
                        {
                            Program.stream.Write(backward);
                            break;
                        }
                    case "Left":
                        {
                            Program.stream.Write(left);
                            break;
                        }
                    case "Right":
                        {
                            Program.stream.Write(right);
                            break;
                        }
                    case "Home":
                        {
                            Program.stream.Write(home);
                            break;
                        }
                    case "Stop":
                        {
                            Program.stream.Write(stop);
                            break;
                        }
                }//switch
            } //if            
        }// onpost control

        public void OnPostCamControl(string cmd)
        {
            Byte[] up = System.Text.Encoding.ASCII.GetBytes("y+");
            Byte[] down = System.Text.Encoding.ASCII.GetBytes("y-");
            Byte[] left = System.Text.Encoding.ASCII.GetBytes("x-");
            Byte[] right = System.Text.Encoding.ASCII.GetBytes("x+");
            Byte[] home = System.Text.Encoding.ASCII.GetBytes("xy_home");

            if (Program.stream != null)
            {
                switch (cmd)
                {
                    case "Up":
                        {
                            Program.stream.Write(up);
                            break;
                        }
                    case "Down":
                        {
                            Program.stream.Write(down);
                            break;
                        }
                    case "Left":
                        {
                            Program.stream.Write(left);
                            break;
                        }
                    case "Right":
                        {
                            Program.stream.Write(right);
                            break;
                        }
                    case "Home":
                        {
                            Program.stream.Write(home);
                            break;
                        }
                }//switch
            } //if            
        }// onpost CamControl    

        public void OnPostSpeak(string text)
        {
            OnPostAuxConnect();
            if (Program.auxStream == null)
            {
                Program.Temp.SetError("Not connected to the server");
                OpenErrorPanel();                
            }
            else if (Program.Temp.GetError() == null)
            {
                try
                {
                    Byte[] speak = System.Text.Encoding.ASCII.GetBytes("TTS*" + text);
                    Program.auxStream.Write(speak);
                    Program.coreClient.Close();
                    Program.auxStream.Close();
                }
                catch
                {
                    Program.Temp.SetError("Lost connection to the server");
                    OpenErrorPanel();                    
                }
            }
            else // web cam is not working
            {
                OpenErrorPanel();                
            }            
        }

        public void OnPostFredSees()
        {
            OnPostAuxConnect();
            string fredSees ="";
            if (Program.auxStream == null)
            {
                Program.Temp.SetError("Not connected to the server");
                OpenErrorPanel();                
            }
            else if (Program.Temp.GetError() == null)
            {
                try
                {
                    Program.FredVision.GetVision("describe", null).Wait();
                    fredSees = Program.FredVision.FredSees();
                    Byte[] speak = System.Text.Encoding.ASCII.GetBytes("TTS*I see" + fredSees);
                    Program.auxStream.Write(speak);
                    Program.Temp.SetDisplayText("I see " + fredSees);
                    Program.Temp.SetTextPanel("block");
                    Program.coreClient.Close();
                    Program.auxStream.Close();
                }
                catch
                {
                    Program.Temp.SetError("Lost connection to the server");
                    OpenErrorPanel();                    
                }                
            }
            else // web cam is not working
            {
                OpenErrorPanel();                
            }
            Response.Redirect("./UserControls");            
        }

        public void OnPostFredReads()
        {
            OnPostAuxConnect();
            string fredReads;
            if (Program.auxStream == null)
            {
                Program.Temp.SetError("Not connected to the server");
                OpenErrorPanel();                
            }
            else if (Program.Temp.GetError() == null)
            {
                try
                {                    
                    Program.FredVision.GetVision("read", null).Wait();
                    fredReads = Program.FredVision.FredReads();
                    Byte[] speak = System.Text.Encoding.ASCII.GetBytes("TTS*" + fredReads);
                    Program.auxStream.Write(speak);
                    Program.Temp.SetDisplayText("I read - " + fredReads);
                    Program.Temp.SetTextPanel("block");
                    Program.coreClient.Close();
                    Program.auxStream.Close();
                }
                catch
                {
                    Program.Temp.SetError("Lost connection to the server");
                    OpenErrorPanel();                    
                }
            }
            else // web cam is not working
            {
                OpenErrorPanel();                
            }
            Response.Redirect("./UserControls");            
        }

        public void OnPostDetectFace()
        {
            OnPostAuxConnect();
            Program.FredVision.GetVision("detect", null).Wait();
            Program.FredVision.DetectFace().Wait();
            List<string> names = Program.FredVision.GetNames();
            Byte[] speak = null;
            string displayGreeting;

            if (names.Count > 1)
            {
                string sayNames = "";
                foreach (string name in names)
                {
                    sayNames += name + " and ";
                }
                sayNames = sayNames.Substring(0, sayNames.Length - 5);
                speak = System.Text.Encoding.ASCII.GetBytes("TTS*Hello " + sayNames + "! How are you today?");
                displayGreeting = "Hello " + sayNames + "! How are you today?";
            }
            else
            {
                switch (names[0])
                {
                    case "no face":
                        {
                            speak = System.Text.Encoding.ASCII.GetBytes("TTS*I dont see any faces to detect");
                            displayGreeting = "I dont see any faces to detect";
                            break;
                        }
                    case "dont recgonize":
                        {
                            speak = System.Text.Encoding.ASCII.GetBytes("TTS*I dont recgonize any faces?");
                            displayGreeting = "I dont recgonize any faces?";
                            break;
                        }
                    default:
                        {
                            string[] greeting = { "How are you today?", "whats up?", "What, you never heard a toy car talk before?" };
                            Random randNum = new Random();
                            randNum.Next(3);
                            speak = System.Text.Encoding.ASCII.GetBytes("TTS*Hello " + names[0] + "! How are you today?");
                            displayGreeting = "Hello " + names[0] + "! How are you today?";
                            break;
                        }
                }
            }

            Program.FredVision.ClearNames();
            names.Clear();

            if (Program.auxStream == null)
            {
                Program.Temp.SetError("Not connected to the server");
                OpenErrorPanel();                
            }
            else if (Program.Temp.GetError() == null)
            {
                try
                {
                    Program.auxStream.Write(speak);
                    Program.Temp.SetDisplayText(displayGreeting);
                    Program.Temp.SetTextPanel("block");
                    Program.coreClient.Close();
                    Program.auxStream.Close();
                }
                catch
                {
                    Program.Temp.SetError("Lost connection to the server");
                    OpenErrorPanel();                    
                }
            }
            else // web cam is not working
            {
                OpenErrorPanel();                
            }
            Response.Redirect("./UserControls");            
        }
        
        public void OnPostCreatePerson(string name, string desc)
        {
            Program.FredVision.CreatePerson(name, desc).Wait();
            Program.FredVision.GetFacesList().Wait();
            Response.Redirect("./UserControls");
        }
        
        public void OnPostDeletePerson(string personID)
        {            
            Program.FredVision.DeletePerson(personID).Wait();
            Program.FredVision.GetFacesList().Wait();
            Response.Redirect("./UserControls");
        }
                
        public void OnPostAddFace(string person)
        {
            Program.Temp.SetPersonID(person);
            Program.Temp.SetTakePicPanel("normal");            
            Program.Temp.SetFacePanel("none");
            Response.Redirect("./UserControls");
        }
        
        public void OnPostTakePic()
        {
            Program.FredVision.GetVision("addFace", Program.Temp.GetPersonID()).Wait();            
            Program.FredVision.TrainFace().Wait();
            Response.Redirect("./UserControls");
        }        

        public void OnPostTest()
        {
            if (Program.auxStream != null)
            {
                Byte[] speak = System.Text.Encoding.ASCII.GetBytes("auto");
                Program.auxStream.Write(speak);
            }            
        }
    }
}