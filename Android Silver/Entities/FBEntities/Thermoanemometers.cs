using Android_Silver.Entities.ValuesEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Android_Silver.Entities.FBEntities
{
    public class ThermoanemometersSPS
    {
        public FloatValue SupTHmKoefA;
        public FloatValue SupTHmKoefB;
        public FloatValue SupTHmKoefK;
        public FloatValue SupCurveKoef;
        public FloatValue ExhaustTHmKoefA;
        public FloatValue ExhaustTHmKoefB;
        public FloatValue ExhaustTHmKoefK;
        public FloatValue ExhaustCurveKoef;
        public FloatValue SupPTa;
        public FloatValue EPTa;
       
        public IntValue PTaReg;
        public IntValue ITaReg;
        public IntValue DTaReg;
        public FloatValue SupDeltaTime;
        public FloatValue EDeltaTime;



        public FloatValue KClKoef;
        public FloatValue BClKoef;
        public FloatValue KPolKoef;
        public FloatValue BPolKoef;

        public FloatValue SupKCold;
        public FloatValue SupBCold;
        public FloatValue EKCold;
        public FloatValue EBCold;


        public ThermoanemometersSPS()
        {
            SupTHmKoefA = new FloatValue(-100_000, 100_000, 2);
            SupTHmKoefB = new FloatValue(-100_000, 100_000, 2);
            SupTHmKoefK = new FloatValue(-100_000, 100_000, 2);
            SupCurveKoef = new FloatValue(-100_000, 100_000, 2);

            ExhaustTHmKoefA = new FloatValue(-100_000, 100_000, 2);
            ExhaustTHmKoefB = new FloatValue(-100_000, 100_000, 2);
            ExhaustTHmKoefK = new FloatValue(-100_000, 100_000, 2);
            ExhaustCurveKoef = new FloatValue(-100_000, 100_000, 2);

            SupPTa = new FloatValue(0, 100, 2);
            EPTa = new FloatValue(0, 100, 2);
           // PThmSupValue = new FloatValue(0, 100, 2);
           // PThmExhaustValue = new FloatValue(0, 100, 2);
            PTaReg = new IntValue(0, 10_000);
            ITaReg = new IntValue(0, 10_000);
            DTaReg = new IntValue(0, 10_000);

            KClKoef = new FloatValue(-99, 99, 2);
            BClKoef = new FloatValue(-99, 99, 2);
            KPolKoef = new FloatValue(-99, 99, 2);
            BPolKoef = new FloatValue(-99, 99, 2);

         
            SupDeltaTime = new FloatValue(0, 100, 1);
            EDeltaTime = new FloatValue(0, 100, 1);
            SupKCold = new FloatValue(-322, 322, 2);
            SupBCold = new FloatValue(-322, 322, 2);
            EKCold = new FloatValue(-322, 322, 2);
            EBCold = new FloatValue(-322, 322, 2);
        }

    }

}