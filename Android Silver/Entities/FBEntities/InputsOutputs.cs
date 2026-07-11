using Android_Silver.Entities.ValuesEntities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Android_Silver.Entities.FBEntities
{
    public class InputsOutputs
    {
       public byte DI1;
       public byte DI2;
       public byte DInOverheat;
       public byte DOut1;
       public byte DOut2;
       public FloatValue ET1;
       public FloatValue ET2;
       public FloatValue AR1;
       public FloatValue AR2;
       public FloatValue AR3;
       public FloatValue AR4;

        public InputsOutputs()
        {
            ET1 = new FloatValue(0, 10, 1);
            ET2 = new FloatValue(0, 10, 1);
            AR1 = new FloatValue(0, 10, 1);
            AR2 = new FloatValue(0, 10, 1);
            AR3 = new FloatValue(0, 10, 1);
            AR4 = new FloatValue(0, 10, 1);
        }
    }
}
