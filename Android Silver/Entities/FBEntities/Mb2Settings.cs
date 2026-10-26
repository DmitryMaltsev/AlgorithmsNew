using Android_Silver.Entities.ValuesEntities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Android_Silver.Entities.FBEntities
{
    public class Mb2Settings
    {
        public byte MbAddress;
        public IntValue MbSpeed;
        public byte Parity;
        public byte StpBits;

        public Mb2Settings()
        {
            MbSpeed = new IntValue(0, 4); 
        }
    }
}
