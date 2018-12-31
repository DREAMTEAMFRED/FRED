using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FRED.Utility
{
    public class Temp
    {
        public int mySpeed = 50;

        public void SetSpeed(int speed)
        {
            mySpeed = speed;
        }

        public int GetSpeed()
        {
            return mySpeed;
        }
    }
}
