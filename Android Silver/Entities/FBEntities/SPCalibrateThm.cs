using Android_Silver.Entities.ValuesEntities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Android_Silver.Entities.FBEntities
{

    public class SPCalibrateThm
    {
        public bool isSup;
        public bool NeedToCalibrate;
        public IntValue CalibrateMode;
        public IntValue FanControlType;
        public FloatValue TWorkingFilter;
        public FloatValue TCalibrateFilter;
        public FloatValue DeltaThm;
        public FloatValue LeakFlow;
        public IntValue CalibrateTimeS;
        public IntValue TestTimeS;
        public int[] CalibrateStepPercs;
        public IntValue CalibrateStepsLimits;
        public float[] PCalibrates;
        public FloatValue PCalibratesLimits;
        public float[] DeltaTCalibrates;
        public FloatValue DeltaTCalibratesLimits;
        public float[] FlowCalibrates;
        public FloatValue FlowCalibratesLimits;
        public IntValue CavType;
        public FloatValue CalibrateDeltaT;


        public SPCalibrateThm()
        {
            CalibrateMode = new IntValue(0, 4);
            FanControlType = new IntValue(0, 1);
            DeltaThm = new FloatValue(-150, 150, 2);
            CalibrateTimeS = new IntValue(0, 65000);
            TestTimeS = new IntValue(0, 65000);
            CalibrateStepsLimits = new IntValue(0, 100);
            CalibrateStepPercs = new int[7];
            TWorkingFilter = new FloatValue(0, 9, 2);
            TCalibrateFilter = new FloatValue(0, 9, 2);
            TCalibrateFilter = new FloatValue(-150, 150, 2);
            LeakFlow = new FloatValue(0, 100_000, 0);
            DeltaTCalibratesLimits = new FloatValue(-150, 150, 2);
            DeltaTCalibrates = new float[7];
            PCalibratesLimits = new FloatValue(0, 100, 2);
            PCalibrates = new float[7];
            FlowCalibratesLimits = new FloatValue(0, 100_000, 0);
            FlowCalibrates = new float[7];
            CavType = new IntValue(0, 1);
            CalibrateDeltaT = new FloatValue(20, 50, 0);
        }
    }
}
