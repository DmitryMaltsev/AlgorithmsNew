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
    public class Recup:BindableBase
    {
		private int _efficiency;

		public int Efficiency
		{
			get { return _efficiency; }
			set {
				_efficiency = value;
				OnPropertyChanged(nameof(Efficiency));
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
        public List<RecPrfofile> RecProfiles=new List<RecPrfofile>();
    }

    public class RecPrfofile
    {
        public byte ProfNum;
        public FloatValue I_Start;
        public FloatValue I_Cont;
        public FloatValue Kp;
        public FloatValue Ki;
        public IntValue IA0;
        public IntValue IB0;
        public IntValue SkipTakts;

        public RecPrfofile(byte profNum,float iStart, float iCount, float kp, float ki)
        {
            ProfNum = profNum;
            I_Start.Min = 0;
            I_Start.Value = iStart;
            I_Start.Max = 3;
            I_Start.NumChr = 1;
            I_Cont.Min = 0;
            I_Cont.Value = iCount;
            I_Cont.Max = 3;
            I_Cont.NumChr =1;
            Kp.Min = 0;
            Kp.Value = kp;
            Kp.Max = 1000;
            Kp.NumChr = 2;
            Ki.Min  = 0;
            Ki.Value = ki;
            Ki.Max = 1000;
            Ki.NumChr = 2;
        }
    }
}
