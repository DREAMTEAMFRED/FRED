using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FRED.Utility
{
    public class Temp
    {
        public int mySpeed = 50;
        public string serverIP = "empty";

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
    }
}
