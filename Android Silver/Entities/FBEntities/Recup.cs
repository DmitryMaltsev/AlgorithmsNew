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


	}
}
