using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using DemoHarnessUpd;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FRED.Pages
{
    public class UserControlsModel : PageModel
    {
        /*Mark's variables*/
        public static string textEntry = "";
        public string showEnrollWindow = Program.Temp.GetEnrollDisplay();
        public string errorMsg = "none";
        public string name = Program.Temp.ProfileName;
        public string desc = Program.Temp.ProfileDesc;
        public bool enrollBtn1 = Program.Temp.GetVoiceBtn1();
        public bool enrollBtn2 = Program.Temp.GetVoiceBtn2();
        public string showUpdKBWindow = Program.Temp.GetUpdKBDisplay();
        public string showEnrollment = Program.Temp.GetEnrollmentDisplay();
        public List<string> speechProfileIds = Program.Temp.SpeechProfileIds;
        public List<string> speechProfileNames = Program.Temp.SpeechProfileNames;
        public List<string> speechProfileDescs = Program.Temp.SpeechProfileDescs;
        public string recAudio = Program.Temp.GetRecordingStat();


        /************************************************************************/

        public string serverIP = Program.Controller.GetIP();                      
        public string error = Program.Temp.GetError();
        public string displayErrorPanel = Program.Temp.GetErrorPanelStatus();
        public string displayControlPanel = Program.Temp.GetContorlPanelStatus();
        public string displayVisionPanel = Program.Temp.GetVisionPanelStatus();
        public string displayFacePanel = Program.Temp.GetFacePanelStatus();
        public string displayVoicePanel = Program.Temp.GetVoicePanelStatus();
        public string displayTakePicPanel = Program.Temp.GetTakePicPanelStatus();
        public string light = Program.Temp.GetLightStatus();
        public int mySpeed = Program.Temp.GetSpeed();
        public string displayTextPanel = Program.Temp.GetTextPanelStatus();
        public string displayText = Program.Temp.GetDisplayText();

        public void OnGet()
        {            
            Program.Temp.SetTextPanel("none");

            /*Mark's variables*/
            showEnrollWindow = Program.Temp.GetEnrollDisplay();
            showUpdKBWindow = Program.Temp.GetUpdKBDisplay();
            showEnrollment = Program.Temp.GetEnrollmentDisplay();
            name = Program.Temp.ProfileName;
            desc = Program.Temp.ProfileDesc;
            enrollBtn1 = Program.Temp.GetVoiceBtn1();
            enrollBtn2 = Program.Temp.GetVoiceBtn2();
            recAudio = Program.Temp.GetRecordingStat();
        /*************************************/
    }        

        public void OpenErrorPanel()
        {
            Program.Temp.SetErrorPanel("block");
            Program.Temp.SetFacePanel("none");
            Program.Temp.SetVisionPanel("none");
            Program.Temp.SetControlPanel("none");
            Program.Temp.SetEnrollmentDisplay("none");
            Program.Temp.SetEnrollDisplay("none");
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
            Program.Temp.SetVoicePanel("none");
            Program.Temp.SetEnrollmentDisplay("none");
            Program.Temp.SetEnrollDisplay("none");
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
            Program.Temp.SetVoicePanel("none");
            Program.Temp.SetEnrollmentDisplay("none");
            Program.Temp.SetEnrollDisplay("none");
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
            Program.Temp.SetVoicePanel("none");
            Program.Temp.SetEnrollmentDisplay("none");
            Program.Temp.SetEnrollDisplay("none");
            Response.Redirect("./UserControls");
        }

        public void OnPostCloseVisionPanel()
        {
            Program.Temp.SetVisionPanel("none");
            Program.Temp.SetEnrollmentDisplay("none");
            Program.Temp.SetEnrollDisplay("none");
            Response.Redirect("./UserControls");
        }

        public void OnPostOpenVoicePanel()
        {
            if (displayVoicePanel == "none")
                Program.Temp.SetVoicePanel("grid");
            else
                Program.Temp.SetVoicePanel("none");

            Program.Temp.SetFacePanel("none");
            Program.Temp.SetControlPanel("none");
            Program.Temp.SetVisionPanel("none");
            Program.Temp.SetEnrollmentDisplay("none");
            Program.Temp.SetEnrollDisplay("none");
            Response.Redirect("./UserControls");
        }

        public void OnPostCloseVoicePanel()
        {
            Program.Temp.SetVoicePanel("none");
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

            Program.Temp.SetEnrollmentDisplay("none");
            Program.Temp.SetEnrollDisplay("none");
            OnGet();
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



        /**********************************Mark's code behind************************************/

        public void OnPostInstruction(string intent, string text)
        {
            textEntry = text;
            Connect(serverIP, intent);
            //Connect("10.2.200.146", intent);
            //Connect("192.168.50.20", intent);
            //Connect("192.168.0.12", intent);
            //Connect("192.168.1.162", intent);
            OnGet();
        }

        public void Connect(String server, String message)
        {
            try
            {
                string comm = message;
                if (message == "Speak" || message == "Reco1" || message == "Reco2" || message == "UpdateKB" || message == "DelProfile")
                {
                    message = message + "*" + textEntry;
                }
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer 
                // connected to the same address as specified by the server, port
                // combination.
                Int32 port = 13000;
                TcpClient client = new TcpClient(server, port);

                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();

                NetworkStream stream = client.GetStream();

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);

                //Console.WriteLine("Sent: {0}", message);

                // Receive the TcpServer.response.

                // Buffer to store the response bytes.
                //Read the first batch of the TcpServer response bytes.

                int count = 0;
                int bytesPerLoop = 1024;
                data = new Byte[bytesPerLoop];
                byte[] newData = new byte[data.Length];
                // String to store the response ASCII representation.
                String responseData = String.Empty;

                if (comm == "Record")
                {
                    Program.Temp.SetRecordingStat("block");
                    Program.Temp.SetEnrollmentDisplay("none");
                    Program.Temp.SetEnrollDisplay("none");
                }
                else if (comm == "StopR")
                {
                    Program.Temp.SetRecordingStat("none");
                    Program.Temp.SetEnrollmentDisplay("none");
                    Program.Temp.SetEnrollDisplay("none");
                }
                else if (comm == "Reco1")
                {
                    Program.Temp.SetEnrollDisplay("block");
                    Program.Temp.SetEnrollmentDisplay("none");
                    Program.Temp.ProfileName = textEntry.Split(":")[0];
                    Program.Temp.ProfileDesc = textEntry.Split(":")[1];
                    Program.Temp.SetVoiceBtn1(false);
                }
                else if (comm == "Reco2")
                {
                    Program.Temp.SetEnrollmentDisplay("none");
                    Program.Temp.SetVoiceBtn2(false);
                }
                else if (comm == "ConfEnroll" || comm == "CancEnroll")
                {
                    Program.Temp.ProfileName = "";
                    Program.Temp.ProfileDesc = "";
                    Program.Temp.SetEnrollDisplay("none");
                    Program.Temp.SetEnrollmentDisplay("none");
                    Program.Temp.SetVoiceBtn1(true);
                    Program.Temp.SetVoiceBtn2(true);
                }
                else if (comm == "FredSpy") // retrieves recorded audio
                {
                    NetCoreSample.player.Stop().Wait();
                    Thread.Sleep(500);
                    do
                    {
                        stream.Read(data, 0, data.Length);
                        Array.Copy(data, 0, newData, count, data.Length);
                        bytesPerLoop += 1024;
                        count += 1024;
                        Array.Resize(ref newData, bytesPerLoop);
                    }
                    while (stream.DataAvailable);

                    Program.Temp.SaveData(newData);

                    NetCoreSample.Audio(@"record.wav");
                    Program.Temp.SetEnrollmentDisplay("none");
                    Program.Temp.SetEnrollDisplay("none");
                }
                else if (comm == "DelProfile")
                {
                    Thread.Sleep(3000);
                    OnPostInstruction("GetEnroll", null);
                }
                else if (comm == "GetEnroll") // retrieves speaker recognition text file
                {
                    speechProfileDescs.Clear();
                    speechProfileIds.Clear();
                    speechProfileNames.Clear();
                    Thread.Sleep(500);
                    do
                    {
                        stream.Read(data, 0, data.Length);
                        Array.Copy(data, 0, newData, count, data.Length);
                        bytesPerLoop += 1;
                        count += 1;
                        Array.Resize(ref newData, bytesPerLoop);
                    }
                    while (stream.DataAvailable);

                    Program.Temp.SaveText(newData);

                    // retrieve stored 
                    StreamReader sr = new StreamReader(@"speaker_recog.txt");
                    while (!sr.EndOfStream)
                    {
                        string[] lineInfo = sr.ReadLine().Split(',');
                        if (!(lineInfo.Length < 3))
                        {
                            speechProfileNames.Add(lineInfo[1]);
                            speechProfileDescs.Add(lineInfo[2]);
                            speechProfileIds.Add(lineInfo[0]);
                        }
                    }
                    Program.Temp.SetSpeechProfileNames(speechProfileNames);
                    Program.Temp.SetSpeechProfileDescs(speechProfileDescs);
                    Program.Temp.SetSpeechProfileIds(speechProfileIds);

                    Program.Temp.SetEnrollmentDisplay("block");
                    Program.Temp.SetEnrollDisplay("block");
                    sr.Close();
                }
                else
                {
                    Program.Temp.SetEnrollmentDisplay("none");
                    Program.Temp.SetEnrollDisplay("none");
                }

                if (message.Contains("Reco1") || message.Contains("Reco2"))
                {
                    Thread.Sleep(10000); // waiting to allow for recording of voice
                }
                else if (message.Contains("UpdateKB")) // preferable to use a database instead!
                {
                    Thread.Sleep(30000); // waiting to allow for QnA to be updated
                }

                // Close everything.
                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                errorMsg = "block";
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                errorMsg = "block";
                Console.WriteLine("SocketException: {0}", e);
            }
            //Console.WriteLine("\n Press Enter to continue...");
            //Console.Read();
        }

    }
}