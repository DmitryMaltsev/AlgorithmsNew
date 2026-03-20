using Android_Silver.Entities.ValuesEntities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Android_Silver.Entities.FBEntities
{
    public class UFLeds
    {
        public FloatValue LEDsI;

        public UFLeds()
        {
            LEDsI = new FloatValue(0,1,1);
        }
    }
}
