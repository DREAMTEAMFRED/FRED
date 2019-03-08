using System;
using System.Collections.Generic;
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
        public string displayErrorPanel = "none";
        public string displayTakePicPanel = "none";
        public string displayAddPersonPanel = "none";
        public string lightStatus = "off";
        public string error = null;
        public string displayTextPanel = "none";
        public string displayText = "";
        public string picStatus = "";
        public string loginError = "";
        public string createError = "";

        public void SetError(string error)
        {
            this.error = error;
        }

        public string GetError()
        {
            return error;
        }

        public void SetLoginError(string error)
        {
            loginError = error;
        }

        public string GetLoginError()
        {
            return loginError;
        }

        public void SetCreateError(string error)
        {
            createError = error;
        }

        public string GetCreateError()
        {
            return createError;
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

        public void SetAddPersonPanel(string display)
        {
            displayAddPersonPanel = display;
        }

        public string GetAddPersonPanelStatus()
        {
            return displayAddPersonPanel;
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
