using Android_Silver.Entities.Modes;
using Android_Silver.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Android_Silver.Entities.Visual
{
    public enum ActivePageState
    {
        MainPage,
        ChooseModePage,
        LoadingPage,
        KithchenTimerPage,
        SetPointsPage,
        SettingsPage,
        JournalPage,
        TSettingsPage,
        SetTSettingsPage,
        OtherSettingsPage,
        OtherSettingsPage2,
        HumidityPage
    }

    public class ActivePagesEntities : BindableBase
    {
        #region Rising properties
        private bool _isMainPage;
        public bool IsMainPage
        {
            get { return _isMainPage; }
            set
            {
                _isMainPage = value;
                OnPropertyChanged(nameof(IsMainPage));
            }
        }

        private bool _isChooseModePage;
        public bool IsChooseModePage
        {
            get { return _isChooseModePage; }
            set
            {
                _isChooseModePage = value;
                OnPropertyChanged(nameof(IsChooseModePage));
            }
        }

        private bool _isLoadingPage;

        public bool IsLoadingPage
        {
            get { return _isLoadingPage; }
            set
            {
                _isLoadingPage = value;
                OnPropertyChanged(nameof(IsLoadingPage));
            }
        }

        private bool _isKitchenTimerPage;

        public bool IsKitchenTimerPage
        {
            get { return _isKitchenTimerPage; }
            set
            {
                _isKitchenTimerPage = value;
                OnPropertyChanged(nameof(IsKitchenTimerPage));
            }
        }

        private bool _isSetPointsPage;

        public bool IsSetPointsPage
        {
            get { return _isSetPointsPage; }
            set
            {
                _isSetPointsPage = value;
                OnPropertyChanged(nameof(IsSetPointsPage));
            }
        }

        private bool _isSettingsPage;

        public bool IsSettingsPage
        {
            get { return _isSettingsPage; }
            set
            {
                _isSettingsPage = value;
                OnPropertyChanged(nameof(IsSettingsPage));
            }
        }

        private bool _isJournalPage;

        public bool IsJournalPage
        {
            get { return _isJournalPage; }
            set
            {
                _isJournalPage = value;
                OnPropertyChanged(nameof(IsJournalPage));
            }
        }

        private bool _isOtherSettingsPage;

        public bool IsOtherSettingsPage
        {
            get { return _isOtherSettingsPage; }
            set
            {
                _isOtherSettingsPage = value;
                OnPropertyChanged($"{nameof(IsOtherSettingsPage)}");
            }
        }


        private bool _isTSettingsPage;

        public bool IsTSettingsPage
        {
            get { return _isTSettingsPage; }
            set
            {
                _isTSettingsPage = value;
                OnPropertyChanged(nameof(IsTSettingsPage));
            }
        }

        private bool _isSetTSettingsPage;

        public bool IsSetTSettingsPage
        {
            get { return _isSetTSettingsPage; }
            set
            {
                _isSetTSettingsPage = value;
                OnPropertyChanged(nameof(IsSetTSettingsPage));
            }
        }

        private bool _isHumidityPage;

        public bool IsHumidityPage
        {
            get { return _isHumidityPage; }
            set {
                _isHumidityPage = value;
                OnPropertyChanged($"{nameof(IsHumidityPage)}");
            }
        }


        #endregion
        ModesEntities _modesEntities;
        public ActivePagesEntities()
        {
            _modesEntities = DIContainer.Resolve<ModesEntities>();
        }


        public void SetActivePageState(ActivePageState activePageState)
        {
            switch (activePageState)
            {
                case ActivePageState.MainPage:
                    {
                        IsMainPage = true;
                        IsChooseModePage = false;
                        IsLoadingPage = false;
                        IsKitchenTimerPage = false;
                        IsSetPointsPage = false;
                        IsSettingsPage = false;
                        IsJournalPage = false;
                        IsTSettingsPage = false;
                        IsSetTSettingsPage = false;
                        IsOtherSettingsPage = false;
                        IsHumidityPage = false;
                    }
                    break;
                case ActivePageState.ChooseModePage:
                    {
                        IsChooseModePage = true;
                        IsMainPage = false;
                        IsLoadingPage = false;
                        IsKitchenTimerPage = false;
                        IsSetPointsPage = false;
                        IsSettingsPage = false;
                        IsJournalPage = false;
                        IsTSettingsPage = false;
                        IsSetTSettingsPage = false;
                        IsOtherSettingsPage = false;
                        IsHumidityPage = false;
                    }
                    break;
                case ActivePageState.LoadingPage:
                    {
                        IsLoadingPage = true;
                        IsChooseModePage = false;
                        IsMainPage = false;
                        IsKitchenTimerPage = false;
                        IsSetPointsPage = false;
                        IsSettingsPage = false;
                        IsJournalPage = false;
                        IsTSettingsPage = false;
                        IsSetTSettingsPage = false;
                        IsOtherSettingsPage = false;
                        IsHumidityPage = false;
                    }
                    break;
                case ActivePageState.KithchenTimerPage:
                    {
                        IsKitchenTimerPage = true;
                        IsLoadingPage = false;
                        IsChooseModePage = false;
                        IsMainPage = false;
                        IsSetPointsPage = false;
                        IsSettingsPage = false;
                        IsJournalPage = false;
                        IsTSettingsPage = false;
                        IsSetTSettingsPage = false;
                        IsOtherSettingsPage = false;
                        IsHumidityPage = false;
                        _modesEntities.Mode2ValuesList[1].TimeModeValues[0].Minute = _modesEntities.Mode2ValuesList[1].TimeModeValues[0].Hour;
                    }
                    break;
                case ActivePageState.SetPointsPage:
                    {
                        IsSetPointsPage = true;
                        IsKitchenTimerPage = false;
                        IsLoadingPage = false;
                        IsChooseModePage = false;
                        IsMainPage = false;
                        IsSettingsPage = false;
                        IsJournalPage = false;
                        IsTSettingsPage = false;
                        IsSetTSettingsPage = false;
                        IsOtherSettingsPage = false;
                        IsHumidityPage = false;
                    }
                    break;
                case ActivePageState.SettingsPage:
                    {
                        IsSettingsPage = true;
                        IsSetPointsPage = false;
                        IsKitchenTimerPage = false;
                        IsLoadingPage = false;
                        IsChooseModePage = false;
                        IsMainPage = false;
                        IsJournalPage = false;
                        IsTSettingsPage = false;
                        IsSetTSettingsPage = false;
                        IsOtherSettingsPage = false;
                        IsHumidityPage = false;
                    }
                    break;
                case ActivePageState.JournalPage:
                    {
                        IsJournalPage = true;
                        IsSettingsPage = false;
                        IsSetPointsPage = false;
                        IsKitchenTimerPage = false;
                        IsLoadingPage = false;
                        IsChooseModePage = false;
                        IsMainPage = false;
                        IsTSettingsPage = false;
                        IsSetTSettingsPage = false;
                        IsOtherSettingsPage = false;
                        IsHumidityPage = false;
                    }
                    break;
                case ActivePageState.TSettingsPage:
                    {
                        IsTSettingsPage = true;
                        IsJournalPage = false;
                        IsSettingsPage = false;
                        IsSetPointsPage = false;
                        IsKitchenTimerPage = false;
                        IsLoadingPage = false;
                        IsChooseModePage = false;
                        IsMainPage = false;
                        IsSetTSettingsPage = false;
                        IsOtherSettingsPage = false;
                        IsHumidityPage = false;
                        for (int i = 0; i < _modesEntities.Mode2ValuesList[2].TimeModeValues.Count; i++)
                        {
                            _modesEntities.Mode2ValuesList[2].TimeModeValues[i].StrokeImg.Current =
                           _modesEntities.Mode2ValuesList[2].TimeModeValues[1].StrokeImg.Default;
                        }
                    }
                    break;
                case ActivePageState.SetTSettingsPage:
                    {
                        IsSetTSettingsPage = true;
                        IsTSettingsPage = false;
                        IsJournalPage = false;
                        IsSettingsPage = false;
                        IsSetPointsPage = false;
                        IsKitchenTimerPage = false;
                        IsLoadingPage = false;
                        IsChooseModePage = false;
                        IsMainPage = false;
                        IsOtherSettingsPage = false;
                        IsHumidityPage = false;
                    }
                    break;
                case ActivePageState.OtherSettingsPage:
                    {
                        IsOtherSettingsPage = true;
                        IsSetTSettingsPage = false;
                        IsTSettingsPage = false;
                        IsJournalPage = false;
                        IsSettingsPage = false;
                        IsSetPointsPage = false;
                        IsKitchenTimerPage = false;
                        IsLoadingPage = false;
                        IsChooseModePage = false;
                        IsMainPage = false;
                        IsHumidityPage = false;
                    }
                    break;
                case    ActivePageState.HumidityPage:
                    {
                        IsHumidityPage = true;
                        IsOtherSettingsPage = false;
                        IsSetTSettingsPage = false;
                        IsTSettingsPage = false;
                        IsJournalPage = false;
                        IsSettingsPage = false;
                        IsSetPointsPage = false;
                        IsKitchenTimerPage = false;
                        IsLoadingPage = false;
                        IsChooseModePage = false;
                        IsMainPage = false;
                    }
                    break;
            }
        }
    }
}


