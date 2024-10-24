using Android_Silver.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Android_Silver.Entities.FBEntities
{
    public class Fans:BindableBase
    {
		private int _supNominalFlow;
		public int SupNominalFlow
		{
			get { return _supNominalFlow; }
			set {
				if (value >= 0 && value<=99999)
				{
                    _supNominalFlow = value;
                    OnPropertyChanged(nameof(SupNominalFlow));
                }
			}
		}

		private int _exhaustNominalFlow;

		public int ExhaustNominalFlow
		{
			get { return _exhaustNominalFlow; }
			set {
				if (value >= 0 && value <= 99999)
				{
					_exhaustNominalFlow = value;
					OnPropertyChanged(nameof(ExhaustNominalFlow));
				}
			}
		}

		private ushort _sFlow;

		public ushort SFlow
		{
			get { return _sFlow; }
			set { 
				_sFlow = value; 
				OnPropertyChanged(nameof(SFlow));
			}
		}

		private ushort _eFlow;

		public ushort EFlow
		{
			get { return _eFlow; }
			set { 
				_eFlow = value; 
				OnPropertyChanged(nameof(EFlow));
			}
		}

		private ushort _sPersent;

		public ushort SPercent
		{
			get { return _sPersent; }
			set { 
				_sPersent = value; 
				OnPropertyChanged(nameof(SPercent));
			}
		}

		private ushort _ePercent;

		public ushort EPercent
		{
			get { return _ePercent; }
			set {
				_ePercent = value; 
				OnPropertyChanged(nameof(EPercent));
			}
		}




		public int SFanNominalFlow;
        public int EFanNominalFlow;
        public int Speed0v;
        public int Speed10v;
        public int PressureFailureDelay;
        public int FanFailureDelay;
        public int DecrFanConfig;
        public int PDecrFan;
        public int IDecrFan;
        public int DDecrFan;
        public int MinFanPercent;
    }
}
