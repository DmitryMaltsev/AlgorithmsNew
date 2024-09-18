using Android_Silver.Entities.FBEntities;
using Android_Silver.Services;
using Android_Silver.ViewModels;

using System.Collections.ObjectModel;

namespace Android_Silver.Entities.Visual.Menus
{
    public class MItem : BindableBase
    {
  


        private FBs _fbs;
        public SActivePageState CactivePageState;

        #region Rising properties 
        private ObservableCollection<StrSet> _strSetsCollection;
        public ObservableCollection<StrSet> StrSetsCollection
        {
            get { return _strSetsCollection; }
            set
            {
                _strSetsCollection = value;
                OnPropertyChanged(nameof(StrSetsCollection));
            }
        }


        private bool _menuIsVisible;
        public bool MenuIsVisible
        {
            get { return _menuIsVisible; }
            set
            {
                _menuIsVisible = value;
                OnPropertyChanged(nameof(MenuIsVisible));
            }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private PicByStates _imgSource;

        public PicByStates ImgSource
        {
            get { return _imgSource; }
            set
            {
                _imgSource = value;
                OnPropertyChanged(nameof(ImgSource));
            }
        }
        #endregion


        public int Address { get; private set; } = 0;

        public MItem(string name, bool isVisible, PicByStates imgSource, SActivePageState sactivePageState, ushort id, int startAddress)
        {
            StrSetsCollection = new ObservableCollection<StrSet>();
            _fbs = DIContainer.Resolve<FBs>();
            Name = name;
            MenuIsVisible = isVisible;
            ImgSource = imgSource;
            CactivePageState = sactivePageState;
            ID = id;
            Address = startAddress;
            // ToSettingsCommand = new Command(ExecuteToSettingsWindow);
            //  SetSettingsCommand = new Command(ExecuteSetSettings);
        }

        public ushort ID { get; private set; }




    }
}
