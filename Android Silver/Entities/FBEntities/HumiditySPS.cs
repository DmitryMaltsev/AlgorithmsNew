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
		private int _humidtitySP;
		public int HumiditySP
		{
			get { return _humidtitySP; }
			set { 
				_humidtitySP = value;
				OnPropertyChanged(nameof(HumiditySP));
			}
		}

	}
}
