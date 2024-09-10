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
        EntryPage,
        StartPage,
        MainMenuPage,
        BaseSettingsPage,
        DamperSettingsPage,
        FanSettingsPage,
        WHSettingsPage,
        EHSettingsPage,
        FreonSettingsPage,
        RecupSettingsPage,
        HumSettingsPage,
        CommonSettingsPage
    }

    public class ServiceActivePagesEntities : BindableBase
    {
        public SActivePageState LastActivePageState { get; set; } = SActivePageState.EntryPage;
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

        private bool _isDamperPage;

        public bool IsDamperPage
        {
            get { return _isDamperPage; }
            set { 
                _isDamperPage = value;
                OnPropertyChanged(nameof(IsDamperPage));
            }
        }

        private bool _isWHPage;

        public bool IsWHPage
        {
            get { return _isWHPage; }
            set { 
                _isWHPage = value;
                OnPropertyChanged(nameof(IsWHPage));
            }
        }

        private bool _isEHPage;

        public bool IsEHPage
        {
            get { return _isEHPage; }
            set { _isEHPage = value;
                OnPropertyChanged(nameof(IsEHPage));
            }
        }

        private bool _isRecupPage;

        public bool IsRecupPage
        {
            get { return _isRecupPage; }
            set { 
                _isRecupPage = value;
                OnPropertyChanged(nameof(IsEHPage));
            }
        }

        private bool _isHumPage;
        public bool IsHumPage
        {
            get { return _isHumPage; }
            set { 
                _isHumPage = value;
                OnPropertyChanged(nameof(IsHumPage));
            }
        }

        private bool _isSensorsPage;
        public bool IsSensorsPage
        {
            get { return _isSensorsPage; }
            set { 
                _isSensorsPage = value;
                OnPropertyChanged(nameof(IsSensorsPage));
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

        private bool _isCommonSettingsPage;

        public bool IsCommonSettingsPage
        {
            get { return _isCommonSettingsPage; }
            set
            {
                _isCommonSettingsPage = value;
                OnPropertyChanged(nameof(IsCommonSettingsPage));
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
                        IsCommonSettingsPage = false;
                    }
                    break;
                case SActivePageState.EntryPage:
                    {
                        IsEntryPage = true;
                        IsStartPage = false;
                        IsMainMenuPage = false;
                        IsBaseSettingsPage = false;
                        IsFanSettingsPage = false;
                        IsCommonSettingsPage = false;
                    }
                    break;
                case SActivePageState.MainMenuPage:
                    {
                        IsMainMenuPage = true;
                        IsEntryPage = false;
                        IsStartPage = false;
                        IsBaseSettingsPage = false;
                        IsFanSettingsPage = false;
                        IsCommonSettingsPage = false;
                    }
                    break;
                case SActivePageState.BaseSettingsPage:
                    {
                        IsBaseSettingsPage = true;
                        IsEntryPage = false;
                        IsStartPage = false;
                        IsMainMenuPage = false;
                        IsFanSettingsPage = false;
                        IsCommonSettingsPage = false;
                    }
                    break;
                case SActivePageState.FanSettingsPage:
                    {
                        IsFanSettingsPage = true;
                        IsEntryPage = false;
                        IsStartPage = false;
                        IsMainMenuPage = false;
                        IsBaseSettingsPage = false;
                        IsCommonSettingsPage = false;
                    }
                    break;
                case SActivePageState.CommonSettingsPage:
                    {
                        IsCommonSettingsPage = true;
                        IsFanSettingsPage = false;
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
