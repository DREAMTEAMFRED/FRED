using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FRED.Utility
{
    public class Temp
    {
        private int mySpeed = 50;        
        private string personID = "";
        private string trainingStatus = "";
        public string displayControlPanel = "none";
        public string displayVisionPanel = "none";
        public string displayFacePanel = "none";
        public string displayVoicePanel = "none";
        public string displayErrorPanel = "none";
        public string displayTakePicPanel = "none";
        public string lightStatus = "off";
        public string error = null;
        public string displayTextPanel = "none";
        public string displayText = "";
        public string picStatus = "";

        /***************Mark's Variables*************/
        static string enrDisplay = "none"; // controls the enroll display popoup window
        static string recording = "none";
        static bool voiceBtn1 = true;
        static bool voiceBtn2 = true;
        static string upKbDisplay = "none"; // controls the update KB display popup window
        private static string enrollmentDisplay = "none";
        public List<string> SpeechProfileIds = new List<string>();
        public List<string> SpeechProfileNames = new List<string>();
        public List<string> SpeechProfileDescs = new List<string>();

        public string ProfileName { get; set; }
        public string ProfileDesc { get; set; }
        /***************************************************************/
        public void SaveData(byte[] data)
        {
            try
            {
                File.WriteAllBytes(@"record.wav", data);
            }
            catch
            {
                //do nothing!
            }
        }

        public void SaveText(byte[] data)
        {
            File.WriteAllBytes(@"speaker_recog.txt", data);
        }

        public string GetRecordingStat()
        {
            return recording;
        }

        public void SetRecordingStat(string recordDisplay)
        {
            recording = recordDisplay;
        }

        public List<string> GetSpeechProfileNames()
        {
            return SpeechProfileNames;
        }

        public void SetSpeechProfileNames(List<string> SpeechProfileName)
        {
            SpeechProfileNames = SpeechProfileName;
        }

        public List<string> GetSpeechProfileDescs()
        {
            return SpeechProfileDescs;
        }

        public void SetSpeechProfileDescs(List<string> SpeechProfileDesc)
        {
            SpeechProfileDescs = SpeechProfileDesc;
        }

        public List<string> GetSpeechProfileIds()
        {
            return SpeechProfileIds;
        }

        public void SetSpeechProfileIds(List<string> SpeechProfileId)
        {
            SpeechProfileIds = SpeechProfileId;
        }

        public string GetEnrollmentDisplay()
        {
            return enrollmentDisplay;
        }

        public void SetEnrollmentDisplay(string enrollDisplay)
        {
            enrollmentDisplay = enrollDisplay;
        }

        public string GetUpdKBDisplay()
        {
            return upKbDisplay;
        }

        public void SetUpdKBDisplay(string displayStat)
        {
            upKbDisplay = displayStat;
        }

        public bool GetVoiceBtn1()
        {
            return voiceBtn1;
        }

        public void SetVoiceBtn1(bool voiceBtn)
        {
            voiceBtn1 = voiceBtn;
        }
        public bool GetVoiceBtn2()
        {
            return voiceBtn2;
        }

        public void SetVoiceBtn2(bool voiceBtn)
        {
            voiceBtn2 = voiceBtn;
        }

        public string GetEnrollDisplay()
        {
            return enrDisplay;
        }

        public void SetEnrollDisplay(string displayStat)
        {
            enrDisplay = displayStat;
        }

        public void SetVoicePanel(string display)
        {
            displayVoicePanel = display;
        }

        public string GetVoicePanelStatus()
        {
            return displayVoicePanel;
        }

        /***************************************************/
        public void SetError(string error)
        {
            this.error = error;
        }

        public string GetError()
        {
            return error;
        }

        public void SetPicStatus(string status)
        {
            picStatus = status;
        }

        public string GetPicStatus()
        {
            return picStatus;
        }

        public void SetLight(string status)
        {
            lightStatus = status;
        }

        public string GetLightStatus()
        {
            return lightStatus;
        }

        public void SetDisplayText(string text)
        {
            displayText = text;
        }

        public string GetDisplayText()
        {
            return displayText;
        }

        public void SetTextPanel(string display)
        {
            displayTextPanel = display;
        }

        public string GetTextPanelStatus()
        {
            return displayTextPanel;
        }

        public void SetFacePanel(string display)
        {
            displayFacePanel = display;
        }

        public string GetFacePanelStatus()
        {
            return displayFacePanel;
        }

        public void SetControlPanel(string display)
        {
            displayControlPanel = display;
        }

        public string GetContorlPanelStatus()
        {
            return displayControlPanel;
        }

        public void SetVisionPanel(string display)
        {
            displayVisionPanel = display;
        }
                
        public string GetVisionPanelStatus()
        {
            return displayVisionPanel;
        }

        public string GetErrorPanelStatus()
        {
            return displayErrorPanel;
        }

        public void SetTakePicPanel(string display)
        {
            displayTakePicPanel = display;
        }

        public string GetTakePicPanelStatus()
        {
            return displayTakePicPanel;
        }

        public void SetErrorPanel(string display)
        {
            displayErrorPanel = display;
        }

        public void SetTrainingStatus(string status)
        {
            trainingStatus = status;
        }

        public string GetTrainingStatus()
        {
            return trainingStatus;
        }

        public void SetPersonID(string personID)
        {
            this.personID = personID;
        }

        public string GetPersonID()
        {
            return personID;
        }

        public void SetSpeed(int speed)
        {
            mySpeed = speed;
        }

        public int GetSpeed()
        {
            return mySpeed;
        }
                
        private List<string> nams = new List<string>();
        private List<string> descriptions = new List<string>();
        private List<int> faceCounts = new List<int>();
        private List<string> personIDs = new List<string>();

        public void SetListNames(List<string> names) { nams = names; }
        public List<string> GetListNames() { return nams; }

        public void SetListDesc(List<string> desc) { descriptions = desc; }
        public List<string> GetListDesc() { return descriptions; }

        public void SetListFaceCounts(List<int> count) { faceCounts = count; }
        public List<int> GetListFaceCounts() { return faceCounts; }

        public void SetListPersonIDs(List<string> IDs) { personIDs = IDs; }
        public List<string> GetListPersonIDs() { return personIDs; }
    }
}
