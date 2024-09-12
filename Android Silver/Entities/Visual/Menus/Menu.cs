using Android_Silver.ViewModels;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Android_Silver.Entities.Visual.Menus
{
    public class MenuItem:BindableBase
    {
		public ICommand ChangeWindowCommand { get;private set; }

		private ServiceActivePagesEntities _sActivePagesentities { get;  set; }
		SActivePageState _activePageState;

		private ObservableCollection<StrSet> _strSets;
		public ObservableCollection<StrSet> StrSets
		{
			get { return _strSets; }
			set { 
				_strSets = value;
				OnPropertyChanged(nameof(StrSets));
			}
		}


		private bool _isVisible;
		public bool IsVisible
		{
			get { return _isVisible; }
			set {
				_isVisible = value;
				OnPropertyChanged(nameof(IsVisible));
			}
		}

		private string _name;

		public string Name
		{
			get { return _name; }
			set { 
				_name = value;
				OnPropertyChanged(nameof(Name));
			}
		}

		private PicByStates _imgSource;

		public PicByStates ImgSource
		{
			get { return _imgSource; }
			set { 
				_imgSource = value;
				OnPropertyChanged(nameof(ImgSource));
			}
		}

		public MenuItem(string name, bool isVisible, PicByStates imgSource, SActivePageState sactivePageState)
        {
            StrSets=new ObservableCollection<StrSet>();
			Name = name;
			ImgSource = imgSource;
			_activePageState = sactivePageState;
			_sActivePagesentities=DIContainer.Resolve<ServiceActivePagesEntities>();
            ChangeWindowCommand = new Command(ExecuteChangeWindow);
        }

        private void ExecuteChangeWindow(object obj)
        {
			_sActivePagesentities.SetActivePageState(_activePageState);
        }
    }
}
