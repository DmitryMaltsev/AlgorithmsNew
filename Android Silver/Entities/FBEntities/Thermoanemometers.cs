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
        public FloatValue PThmSup;
        public FloatValue PThmExhaust;
        public FloatValue PThmSupValue;
        public FloatValue PThmExhaustValue;
        public float TempH1;
        public float TempC1;
        public float TempH2;
        public float TempC2;
        public IntValue PReg;
        public IntValue IReg;
        public IntValue DReg;
        public FloatValue KClKoef;
        public FloatValue BClKoef;
        public FloatValue KPolKoef;
        public FloatValue BPolKoef;
  

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

            PThmSup = new FloatValue(0, 100, 2);
            PThmExhaust = new FloatValue(0, 100, 2);
            PThmSupValue = new FloatValue(0, 100, 2);
            PThmExhaustValue = new FloatValue(0, 100, 2);
            PReg = new IntValue(0, 10_000);
            IReg = new IntValue(0, 10_000);
            DReg = new IntValue(0, 10_000);

            KClKoef = new FloatValue(-99, 99, 2);
            BClKoef = new FloatValue(-99, 99, 2);
            KPolKoef = new FloatValue(-99, 99, 2);
            BPolKoef = new FloatValue(-99, 99, 2);


        }

    }

}