using Android_Silver.Entities.ValuesEntities;
using Android_Silver.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Unity.Injection;

namespace Android_Silver.Entities.FBEntities
{
    public class Recup : BindableBase
    {
        private int _efficiency;

        public int Efficiency
        {
            get { return _efficiency; }
            set
            {
                _efficiency = value;
                OnPropertyChanged(nameof(Efficiency));
            }
        }

        private int _freqHZ;
        public int FreqHZ
        {
            get { return _freqHZ; }
            set {
                OnPropertyChanged(nameof(FreqHZ));
                _freqHZ = value;
            }
        }


        public int PReg;
        public int IReg;
        public int DReg;
        public float TEffSP;
        public int EffFailValue;
        public int EffFailDelay;
        public int HZMax;
        public float TempA;
        public float TempB;
        public float TempC;
        public float TempD;
        public List<RecPrfofile> RecProfiles = new List<RecPrfofile>();
    }

    public class RecPrfofile
    {
        public byte ProfNum;
        public FloatValue I_StartLimits;
        public float I_Start;
        public FloatValue I_ContLimits;
        public float I_Cont;
        public FloatValue KpLimits;
        public float Kp;
        public FloatValue KiLimits;
        public float Ki;
        public IntValue IA0Limits;
        public int IA0;
        public IntValue IB0Limits;
        public int IB0;
        public IntValue SkipTaktsLimits;
        public int SkipTakts;

        public RecPrfofile(byte profNum, float iStart, float iCount, float kp, float ki)
        {
            ProfNum = profNum;
            I_StartLimits = new FloatValue(0, 3, 1);
            I_StartLimits.Value = iStart;
            I_ContLimits = new FloatValue(0, 3, 1);
            I_ContLimits.Value = iCount;
            KpLimits = new FloatValue(0, 1000, 2);
            KpLimits.Value = kp;
            KiLimits = new FloatValue(0, 1000, 2);
            KiLimits.Value = ki;
        }
    }
}
