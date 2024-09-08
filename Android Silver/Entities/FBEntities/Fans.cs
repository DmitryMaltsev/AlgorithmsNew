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


	}
}
