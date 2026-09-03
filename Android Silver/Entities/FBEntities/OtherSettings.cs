using Android_Silver.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Android_Silver.Entities.FBEntities
{
    public class OtherSettings:BindableBase
    {
		public Action<bool> SpecModeAction;
		public Action<bool> MFloorAction;

		private bool _isSpecMode;
		public bool IsSpecMode
		{
			get { return _isSpecMode; }
			set {
				if (_isSpecMode != value)
				{
                    _isSpecMode = value;
                    OnPropertyChanged(nameof(IsSpecMode));
                    SpecModeAction?.Invoke(_isSpecMode);
                }
            }
		}

		private bool _isScheduler;
		public bool IsScheduler
		{
			get { return _isScheduler; }
			set {
				_isScheduler = value;
				OnPropertyChanged(nameof(IsScheduler));
			}
		}
	}
}
