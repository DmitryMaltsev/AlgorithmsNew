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


		private bool _isMF;
		public bool IsMF
		{
			get { return _isMF; }
			set {
				if (_isMF != value)
				{
                    _isMF = value;
                    OnPropertyChanged(nameof(IsMF));
					MFloorAction?.Invoke(_isMF);
                }
			}
		}



	}
}
