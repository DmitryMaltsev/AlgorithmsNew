using Android_Silver.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}
