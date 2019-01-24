using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FRED.Utility
{
    public class Temp
    {
        private int mySpeed = 50;
        private string serverIP = "empty";

        public void SetSpeed(int speed)
        {
            mySpeed = speed;
        }

        public int GetSpeed()
        {
            return mySpeed;
        }

        public void SetIP(string ip)
        {
            serverIP = ip;
        }

        public string GetIP()
        {
            return serverIP;
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
