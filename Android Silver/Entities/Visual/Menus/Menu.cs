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
    public class MItem:BindableBase
    {
		public ICommand ChangeWindowCommand { get;private set; }

		private ServiceActivePagesEntities _sActivePagesentities { get;  set; }
		SActivePageState _activePageState;

		private ObservableCollection<StrSet> _strSets;
		public ObservableCollection<StrSet> StrSetsCollection
		{
			get { return _strSets; }
			set { 
				_strSets = value;
				OnPropertyChanged(nameof(StrSetsCollection));
			}
		}


		private bool _menuIsVisible;
		public bool MenuIsVisible
		{
			get { return _menuIsVisible; }
			set {
				_menuIsVisible = value;
				OnPropertyChanged(nameof(MenuIsVisible));
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

		public MItem(string name, bool isVisible, PicByStates imgSource, SActivePageState sactivePageState, ushort id)
        {
            StrSetsCollection=new ObservableCollection<StrSet>();
			Name = name;
            MenuIsVisible = isVisible;
            ImgSource = imgSource;
			_activePageState = sactivePageState;
			ID = id;
            _sActivePagesentities =DIContainer.Resolve<ServiceActivePagesEntities>();
            ChangeWindowCommand = new Command(ExecuteChangeWindow);
        }

        public ushort ID { get; private set; }

        private void ExecuteChangeWindow(object obj)
        {
			_sActivePagesentities.SetActivePageState(_activePageState);
        }
    }
}
