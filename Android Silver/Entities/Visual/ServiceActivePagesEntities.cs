using Android_Silver.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Android_Silver.Entities.Visual
{
    #region MyRegion


    #endregion
    public enum SActivePageState
    {
        EntyPage,
        StartPage,
        MainMenuPage,
        BaseSettingsPage,
        FanSettingsPage
    }

    public class ServiceActivePagesEntities : BindableBase
    {
        public SActivePageState LastActivePageState { get;  set; } = SActivePageState.EntyPage;
        #region Rising properties
        private bool _isStartPage;

        public bool IsStartPage
        {
            get { return _isStartPage; }
            set
            {
                _isStartPage = value;
                OnPropertyChanged(nameof(IsStartPage));
            }
        }

        private bool _isEntryPage;

        public bool IsEntryPage
        {
            get { return _isEntryPage; }
            set
            {
                _isEntryPage = value;
                OnPropertyChanged(nameof(IsEntryPage));
            }
        }


        private bool _isMainMenuPage;

        public bool IsMainMenuPage
        {
            get { return _isMainMenuPage; }
            set
            {
                _isMainMenuPage = value;
                OnPropertyChanged(nameof(IsMainMenuPage));
            }
        }

        private bool _isBaseSettingsPage;

        public bool IsBaseSettingsPage
        {
            get { return _isBaseSettingsPage; }
            set
            {
                _isBaseSettingsPage = value;
                OnPropertyChanged(nameof(IsBaseSettingsPage));
            }
        }

        private bool _isFanSettingsPage;
        public bool IsFanSettingsPage
        {
            get { return _isFanSettingsPage; }
            set
            {
                _isFanSettingsPage = value;
                OnPropertyChanged(nameof(IsFanSettingsPage));
            }
        }
        #endregion


        public void SetActivePageState(SActivePageState activePageState, int pageState = 0)
        {
            switch (activePageState)
            {
                case SActivePageState.StartPage:
                    {
                        IsStartPage = true;
                        IsEntryPage = false;
                        IsMainMenuPage = false;
                        IsBaseSettingsPage = false;
                        IsFanSettingsPage = false;

                    }
                    break;
                case SActivePageState.EntyPage:
                    {
                        IsEntryPage = true;
                        IsStartPage = false;
                        IsMainMenuPage = false;
                        IsBaseSettingsPage = false;
                        IsFanSettingsPage = false;
                    }
                    break;
                case SActivePageState.MainMenuPage:
                    {
                        IsMainMenuPage = true;
                        IsEntryPage = false;
                        IsStartPage = false;
                        IsBaseSettingsPage = false;
                        IsFanSettingsPage = false;
                    }
                    break;
                case SActivePageState.BaseSettingsPage:
                    {
                        IsBaseSettingsPage = true;
                        IsEntryPage = false;
                        IsStartPage = false;
                        IsMainMenuPage = false;
                        IsFanSettingsPage = false;
                    }
                    break;
                case SActivePageState.FanSettingsPage:
                    {
                        IsFanSettingsPage = true;
                        IsEntryPage = false;
                        IsStartPage = false;
                        IsMainMenuPage = false;
                        IsBaseSettingsPage = false;
                    }
                    break;
            }

            if (activePageState != SActivePageState.StartPage)
            {
                LastActivePageState = activePageState;
            }
        }
    }
}
