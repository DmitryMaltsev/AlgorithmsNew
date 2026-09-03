using Android_Silver.ViewModels;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Android_Silver.Entities.FBEntities
{
    public class HumiditySPS:BindableBase
    {

		private int _spPerc;
		public int SPPerc
		{
			get { return _spPerc; }
			set { 
				_spPerc = value;
				OnPropertyChanged(nameof(SPPerc));
			}
		}

		private ushort _valPerc;
		public ushort ValPerc
		{
			get { return _valPerc; }
			set { 
				_valPerc = value;
				OnPropertyChanged(nameof(_valPerc));
			}
		}

		private ushort _sensPerc;

		public ushort SensPerc
		{
			get { return _sensPerc; }
			set { 
				_sensPerc = value;
				OnPropertyChanged(nameof(SensPerc));
			}
		}

		public int Stage1OnS;
        public int Stage1OffS;
        public int PReg;
        public int IReg;
        public int DReg;
        public int Hyst;

    }
}
