using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FRED.Pages
{
    public class UserControlsModel : PageModel
    {
        private static TcpClient client = null;
        private static TcpClient coreClient = null;
        private static NetworkStream stream = null;
        private static NetworkStream auxStream = null;
        public string serverIP = Program.Temp.GetIP();

        //public string personID = Program.Temp.GetPersonID();
        public string display = "none";
        public int mySpeed = Program.Temp.GetSpeed();

        public async Task OnGetAsync()
        {            
            await Program.FredVision.GetFacesList();
            
        }
        
        public void OnPostLogout()
        {
            Program.Controller.UserID = 0;
            Program.Controller.IsVerified = false;

            Response.Redirect("./Index");
        }

        public void OnPostConnect(string ipAddress)
        {
            Program.Temp.SetIP(ipAddress);
            string server = ipAddress;
            int port = 21567;
            Byte[] speed = System.Text.Encoding.ASCII.GetBytes("speed50");

            try
            {
                client = new TcpClient();
                client.Connect(server, port);
                stream = client.GetStream();
                stream.Write(speed);
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
        }

        public void OnPostDisconnect()
        {
            Byte[] stop = System.Text.Encoding.ASCII.GetBytes("stop");
            if (stream != null)
            {
                stream.Write(stop);
                stream.Close();
            }
            if (client != null)
                client.Close();
        }

        public void OnPostAuxConnect(string ipAddress)
        {
            Program.Temp.SetIP(ipAddress);
            string server = ipAddress;
            int port = 13000;

            try
            {
                coreClient = new TcpClient();
                coreClient.Connect(server, port);
                auxStream = coreClient.GetStream();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            Response.Redirect("./UserControls");
        }

        public void OnPostAuxDisconnect()
        {
            Byte[] stop = System.Text.Encoding.ASCII.GetBytes("stop");
            if (stream != null)
            {
                auxStream.Write(stop);
                auxStream.Close();
            }
            if (client != null)
                coreClient.Close();
        }

        public void OnPostLightOn()
        {
            Byte[] light = System.Text.Encoding.ASCII.GetBytes("light-on");
            auxStream.Write(light);
        }

        public void OnPostLightOff()
        {
            Byte[] light = System.Text.Encoding.ASCII.GetBytes("light-off");
            auxStream.Write(light);
        }

        public void OnPostSetSpeed(int speed)
        {
            Program.Temp.SetSpeed(speed);
            mySpeed = speed;
            string curSpeed = "speed" + mySpeed;
            Byte[] setSpeed = System.Text.Encoding.ASCII.GetBytes(curSpeed);

            if (stream != null)
            {
                stream.Write(setSpeed);
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

            if (stream != null)
            {
                switch (cmd)
                {
                    case "Forward":
                        {
                            stream.Write(forward);
                            break;
                        }
                    case "Backward":
                        {
                            stream.Write(backward);
                            break;
                        }
                    case "Left":
                        {
                            stream.Write(left);
                            break;
                        }
                    case "Right":
                        {
                            stream.Write(right);
                            break;
                        }
                    case "Home":
                        {
                            stream.Write(home);
                            break;
                        }
                    case "Stop":
                        {
                            stream.Write(stop);
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

            if (stream != null)
            {
                switch (cmd)
                {
                    case "Up":
                        {
                            stream.Write(up);
                            break;
                        }
                    case "Down":
                        {
                            stream.Write(down);
                            break;
                        }
                    case "Left":
                        {
                            stream.Write(left);
                            break;
                        }
                    case "Right":
                        {
                            stream.Write(right);
                            break;
                        }
                    case "Home":
                        {
                            stream.Write(home);
                            break;
                        }
                }//switch
            } //if            
        }// onpost CamControl    

        public void OnPostSpeak(string text)
        {
            Byte[] speak = System.Text.Encoding.ASCII.GetBytes("TTS-" + text);
            auxStream.Write(speak);
        }

        public async void OnPostFredSees()
        {
            await Program.FredVision.GetVision("describe", null);
            Byte[] speak = System.Text.Encoding.ASCII.GetBytes("TTS-I see" + Program.FredVision.FredSees());
            auxStream.Write(speak);
        }

        public async void OnPostFredReads()
        {
            await Program.FredVision.GetVision("read", null);
            Byte[] speak = System.Text.Encoding.ASCII.GetBytes("TTS-" + Program.FredVision.FredReads());
            auxStream.Write(speak);
        }

        public async void OnPostDetectFace()
        {
            await Program.FredVision.GetVision("detect", null);
            await Program.FredVision.DetectFace();
            List<string> names = Program.FredVision.GetNames();
            Byte[] speak = null;

            if (names.Count > 1)
            {
                string sayNames = "";
                foreach (string name in names)
                {
                    sayNames += name + " and ";
                }
                sayNames = sayNames.Substring(0, sayNames.Length - 5);
                speak = System.Text.Encoding.ASCII.GetBytes("TTS-Hello, " + sayNames + ". How are you today?");
                
            }
            else
            {
                switch (names[0])
                {
                    case "no face":
                        {
                            speak = System.Text.Encoding.ASCII.GetBytes("TTS-I dont see any faces to detect");                            
                            break;
                        }
                    case "dont recgonize":
                        {
                            speak = System.Text.Encoding.ASCII.GetBytes("TTS-I dont recgonize any faces?");                            
                            break;
                        }
                    default:
                        {
                            string[] greeting = { "How are you today?", "whats up?", "What, you never heard a toy car talk before?" };
                            Random randNum = new Random();
                            randNum.Next(3);
                            speak = System.Text.Encoding.ASCII.GetBytes("TTS-Hello, " + names[0] + ". How are you today?");                            
                            break;
                        }
                }
            }

            Program.FredVision.ClearNames();
            names.Clear();
            auxStream.Write(speak);            
        }
        
        public async void OnPostCreatePerson(string name, string desc)
        {
            await Program.FredVision.CreatePerson(name, desc);            
            await OnGetAsync();
            Response.Redirect("./UserControls");
        }
        
        public async void OnPostDeletePerson(string personID)
        {            
            await Program.FredVision.DeletePerson(personID);            
            await OnGetAsync();            
            Response.Redirect("./UserControls");
        }
                
        public void OnPostAddFace(string person)
        {
            Program.Temp.SetPersonID(person);
            display = "normal";
        }
        
        public async void OnPostTakePic()
        {
            await Program.FredVision.GetVision("addFace", Program.Temp.GetPersonID());
            await Program.FredVision.TrainFace();
            await OnGetAsync();
            //Thread.Sleep(1000);
            Response.Redirect("./UserControls");
        }

        public void CloseWindow()
        {
            display = "none";
        }

        
    }
}