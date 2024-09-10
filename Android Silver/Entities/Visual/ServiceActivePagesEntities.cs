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
        CommonSettingsPage,
        SensorsSettingPage,
        ConfigPage
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

        private bool _isDamperSettingsPage;

        public bool IsDamperSettingsPage
        {
            get { return _isDamperSettingsPage; }
            set { 
                _isDamperSettingsPage = value;
                OnPropertyChanged(nameof(IsDamperSettingsPage));
            }
        }

        private bool _isWHSettingsPage;

        public bool IsWHSettingsPage
        {
            get { return _isWHSettingsPage; }
            set { 
                _isWHSettingsPage = value;
                OnPropertyChanged(nameof(IsWHSettingsPage));
            }
        }

        private bool _isEHSettingsPage;

        public bool IsEHSettingsPage
        {
            get { return _isEHSettingsPage; }
            set { _isEHSettingsPage = value;
                OnPropertyChanged(nameof(IsEHSettingsPage));
            }
        }

        private bool _isRecupSettingsPage;

        public bool IsRecupSettingsPage
        {
            get { return _isRecupSettingsPage; }
            set { 
                _isRecupSettingsPage = value;
                OnPropertyChanged(nameof(IsEHSettingsPage));
            }
        }

        private bool _isHumSettingsPage;
        public bool IsHumSettingsPage
        {
            get { return _isHumSettingsPage; }
            set { 
                _isHumSettingsPage = value;
                OnPropertyChanged(nameof(IsHumSettingsPage));
            }
        }

        private bool _isSensorsSettingsPage;
        public bool IsSensorsSettingsPage
        {
            get { return _isSensorsSettingsPage; }
            set { 
                _isSensorsSettingsPage = value;
                OnPropertyChanged(nameof(IsSensorsSettingsPage));
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

        private bool _isConfigPage;

        public bool IsConfigPage
        {
            get { return _isConfigPage; }
            set { 
                _isConfigPage = value; 
                OnPropertyChanged(nameof(IsConfigPage));
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
                        IsCommonSettingsPage = false;
                        IsDamperSettingsPage = false;
                        IsFanSettingsPage = false;
                        IsWHSettingsPage = false;
                        IsEHSettingsPage = false;
                        IsRecupSettingsPage = false;
                        IsHumSettingsPage = false;
                        IsSensorsSettingsPage = false;
                        IsConfigPage = false;
                        
                    }
                    break;
                case SActivePageState.EntryPage:
                    {
                        IsEntryPage = true;
                        IsStartPage = false;
                        IsMainMenuPage = false;
                        IsBaseSettingsPage = false;
                        IsCommonSettingsPage = false;
                        IsDamperSettingsPage = false;
                        IsFanSettingsPage = false;
                        IsWHSettingsPage = false;
                        IsEHSettingsPage = false;
                        IsRecupSettingsPage = false;
                        IsHumSettingsPage = false;
                        IsSensorsSettingsPage = false;
                        IsConfigPage = false;
                    }
                    break;
                case SActivePageState.MainMenuPage:
                    {
                        IsMainMenuPage = true;
                        IsEntryPage = false;
                        IsStartPage = false; 
                        IsBaseSettingsPage = false;
                        IsCommonSettingsPage = false;
                        IsDamperSettingsPage = false;
                        IsFanSettingsPage = false;
                        IsWHSettingsPage = false;
                        IsEHSettingsPage = false;
                        IsRecupSettingsPage = false;
                        IsHumSettingsPage = false;
                        IsSensorsSettingsPage = false;
                        IsConfigPage = false;
                    }
                    break;
                case SActivePageState.BaseSettingsPage:
                    {
                        IsBaseSettingsPage = true;
                        IsMainMenuPage = false;
                        IsEntryPage = false;
                        IsStartPage = false;
                        IsCommonSettingsPage = false;
                        IsDamperSettingsPage = false;
                        IsFanSettingsPage = false;
                        IsWHSettingsPage = false;
                        IsEHSettingsPage = false;
                        IsRecupSettingsPage = false;
                        IsHumSettingsPage = false;
                        IsSensorsSettingsPage = false;
                        IsConfigPage = false;
                    }
                    break;
      
                case SActivePageState.CommonSettingsPage:
                    {
                        IsCommonSettingsPage = true;
                        IsBaseSettingsPage = false;
                        IsMainMenuPage = false;
                        IsEntryPage = false;
                        IsStartPage = false;
                        IsDamperSettingsPage = false;
                        IsFanSettingsPage = false;
                        IsWHSettingsPage = false;
                        IsEHSettingsPage = false;
                        IsRecupSettingsPage = false;
                        IsHumSettingsPage = false;
                        IsSensorsSettingsPage = false;
                        IsConfigPage = false;
                    }
                    break;
                case SActivePageState.DamperSettingsPage:
                    {
                        IsDamperSettingsPage = true;
                        IsCommonSettingsPage = false;
                        IsBaseSettingsPage = false;
                        IsMainMenuPage = false;
                        IsEntryPage = false;
                        IsStartPage = false;
                        IsFanSettingsPage = false;
                        IsWHSettingsPage = false;
                        IsEHSettingsPage = false;
                        IsRecupSettingsPage = false;
                        IsHumSettingsPage = false;
                        IsSensorsSettingsPage = false;
                        IsConfigPage = false;
                    }
                    break;
                case SActivePageState.FanSettingsPage:
                    {
                        IsFanSettingsPage = true;
                        IsDamperSettingsPage = false;
                        IsCommonSettingsPage = false;
                        IsBaseSettingsPage = false;
                        IsMainMenuPage = false;
                        IsEntryPage = false;
                        IsStartPage = false;
                        IsWHSettingsPage = false;
                        IsEHSettingsPage = false;
                        IsRecupSettingsPage = false;
                        IsHumSettingsPage = false;
                        IsSensorsSettingsPage = false;
                        IsConfigPage = false;
                    }
                    break;
                case SActivePageState.WHSettingsPage:
                    {
                        IsWHSettingsPage = true;
                        IsFanSettingsPage = false;
                        IsDamperSettingsPage = false;
                        IsCommonSettingsPage = false;
                        IsBaseSettingsPage = false;
                        IsMainMenuPage = false;
                        IsEntryPage = false;
                        IsStartPage = false;
                        IsEHSettingsPage = false;
                        IsRecupSettingsPage = false;
                        IsHumSettingsPage = false;
                        IsSensorsSettingsPage = false;
                        IsConfigPage = false;
                    }
                    break;
                case SActivePageState.EHSettingsPage:
                    {
                        IsEHSettingsPage = true;
                        IsWHSettingsPage = false;
                        IsFanSettingsPage = false;
                        IsDamperSettingsPage = false;
                        IsCommonSettingsPage = false;
                        IsBaseSettingsPage = false;
                        IsMainMenuPage = false;
                        IsEntryPage = false;
                        IsStartPage = false;
                        IsRecupSettingsPage = false;
                        IsHumSettingsPage = false;
                        IsSensorsSettingsPage = false;
                        IsConfigPage = false;
                    }
                    break;

                case SActivePageState.RecupSettingsPage: 
                    {
                        IsRecupSettingsPage = true;
                        IsEHSettingsPage = false;
                        IsWHSettingsPage = false;
                        IsFanSettingsPage = false;
                        IsDamperSettingsPage = false;
                        IsCommonSettingsPage = false;
                        IsBaseSettingsPage = false;
                        IsMainMenuPage = false;
                        IsEntryPage = false;
                        IsStartPage = false;
                        IsHumSettingsPage = false;
                        IsSensorsSettingsPage = false;
                        IsConfigPage = false;
                    }
                    break;
                case SActivePageState.HumSettingsPage:
                    {
                        IsHumSettingsPage = true;
                        IsRecupSettingsPage = false;
                        IsEHSettingsPage = false;
                        IsWHSettingsPage = false;
                        IsFanSettingsPage = false;
                        IsDamperSettingsPage = false;
                        IsCommonSettingsPage = false;
                        IsBaseSettingsPage = false;
                        IsMainMenuPage = false;
                        IsEntryPage = false;
                        IsStartPage = false;
                        IsSensorsSettingsPage = false;
                        IsConfigPage = false;
                    }
                    break;
                case SActivePageState.SensorsSettingPage:
                    {
                        IsSensorsSettingsPage = true;
                        IsHumSettingsPage = false;
                        IsRecupSettingsPage = false;
                        IsEHSettingsPage = false;
                        IsWHSettingsPage = false;
                        IsFanSettingsPage = false;
                        IsDamperSettingsPage = false;
                        IsCommonSettingsPage = false;
                        IsBaseSettingsPage = false;
                        IsMainMenuPage = false;
                        IsEntryPage = false;
                        IsStartPage = false;
                        IsConfigPage = false;
                    }
                    break;
                case SActivePageState.ConfigPage:
                    {
                        IsConfigPage = true;
                        IsSensorsSettingsPage = false;
                        IsHumSettingsPage = false;
                        IsRecupSettingsPage = false;
                        IsEHSettingsPage = false;
                        IsWHSettingsPage = false;
                        IsFanSettingsPage = false;
                        IsDamperSettingsPage = false;
                        IsCommonSettingsPage = false;
                        IsBaseSettingsPage = false;
                        IsMainMenuPage = false;
                        IsEntryPage = false;
                        IsStartPage = false;
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
