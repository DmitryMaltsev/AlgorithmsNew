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
            set
            {
                OnPropertyChanged(nameof(FreqHZ));
                _freqHZ = value;
            }
        }

        public FloatValue BPolKoef;
        public int PReg;
        public int IReg;
        public int DReg;
        public FloatValue TEffSP;
        public int EffFailValue;
        public int EffFailDelay;
        public int HZMax;
        public FloatValue TempA;
        public FloatValue TempB;
        public FloatValue TempC;
        public FloatValue TempD;
        public IntValue RecInMeasureTrh;
        public List<RecPrfofile> RecProfiles = new List<RecPrfofile>();
        public Recup()
        {
            TEffSP = new FloatValue(-100, 100, 1);
            TempA = new FloatValue(-50, 50, 1);
            TempB = new FloatValue(-50, 50, 1);
            TempC = new FloatValue(-50, 50, 1);
            TempD = new FloatValue(-50, 50, 1);
            RecInMeasureTrh = new IntValue(0, 100);
        }

    }

    public class RecPrfofile
    {
        public byte ProfNum;
        public FloatValue I_Start;
        public FloatValue I_Cont;
        public FloatValue Kp;
        public FloatValue Ki;
        public IntValue IA0Limits;
        public int IA0;
        public IntValue IB0Limits;
        public int IB0;
        public IntValue SkipTaktsLimits;
        public int SkipTakts;

        public RecPrfofile(byte profNum, float iStart, float iCount, float kp, float ki)
        {
            ProfNum = profNum;
            I_Start = new FloatValue(0, 4, 1);
            I_Cont = new FloatValue(0, 4, 1);
            Kp = new FloatValue(0, 100, 2);
            Ki = new FloatValue(0, 100, 2);
         
        }
    }
}
