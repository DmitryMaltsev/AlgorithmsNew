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
        HumidityPage,
        TimePage,
        SwipePage,
        ShedulerPage
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
            set
            {
                _isHumidityPage = value;
                OnPropertyChanged(nameof(IsHumidityPage));
            }
        }

        private bool _isTimerPage;

        public bool IsTimePage
        {
            get { return _isTimerPage; }
            set
            {
                _isTimerPage = value;
                OnPropertyChanged(nameof(IsTimePage));
            }
        }

        private bool _isSwipePage;

        public bool IsSwipePage
        {
            get { return _isSwipePage; }
            set
            {
                _isSwipePage = value;
                OnPropertyChanged(nameof(IsSwipePage));
            }
        }

        private bool _isShedulerPage;
        public bool IsShedulerPage
        {
            get { return _isShedulerPage; }
            set
            {
                _isShedulerPage = value;
                OnPropertyChanged(nameof(IsShedulerPage));
            }
        }

        public int QueryStep { get; set; } = 0;

        #endregion

        ModesEntities _modesEntities;
        public ActivePagesEntities()
        {
            _modesEntities = DIContainer.Resolve<ModesEntities>();
        }


        public void SetActivePageState(ActivePageState activePageState, int pageState = 0)
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
                        IsTimePage = false;
                        IsSwipePage = false;
                        IsShedulerPage = false;
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
                        IsTimePage = false;
                        IsSwipePage = false;
                        IsShedulerPage = false;
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
                        IsTimePage = false;
                        IsSwipePage = false;
                        IsShedulerPage = false;
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
                        IsTimePage = false;
                        IsSwipePage = false;
                        IsShedulerPage = false;
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
                        IsTimePage = false;
                        IsSwipePage = false;
                        IsShedulerPage = false;
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
                        IsTimePage = false;
                        IsSwipePage = false;
                        IsShedulerPage = false;
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
                        IsTimePage = false;
                        IsSwipePage = false;
                        IsShedulerPage = false;
                    }
                    break;
                case ActivePageState.TSettingsPage:
                    {
                        if (pageState == 0)
                        {
                            for (int i = 0; i < _modesEntities.Mode2ValuesList[2].TimeModeValues.Count; i++)
                            {
                                _modesEntities.Mode2ValuesList[2].TimeModeValues[i].StrokeImg.Current =
                               _modesEntities.Mode2ValuesList[2].TimeModeValues[1].StrokeImg.Default;
                            }
                            _modesEntities.CTimeModeValues = _modesEntities.Mode2ValuesList[2].TimeModeValues;
                        }
                        else
                        {
                            for (int i = 0; i < _modesEntities.Mode2ValuesList[3].TimeModeValues.Count; i++)
                            {
                                _modesEntities.Mode2ValuesList[3].TimeModeValues[i].StrokeImg.Current =
                               _modesEntities.Mode2ValuesList[3].TimeModeValues[1].StrokeImg.Default;
                            }
                            _modesEntities.CTimeModeValues = _modesEntities.Mode2ValuesList[3].TimeModeValues;
                        }

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
                        IsTimePage = false;
                        IsSwipePage = false;
                        IsShedulerPage = false;
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
                        IsTimePage = false;
                        IsSwipePage = false;
                        IsShedulerPage = false;
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
                        IsTimePage = false;
                        IsSwipePage = false;
                        IsShedulerPage = false;
                    }
                    break;
                case ActivePageState.HumidityPage:
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
                        IsTimePage = false;
                        IsSwipePage = false;
                        IsShedulerPage = false;
                    }
                    break;
                case ActivePageState.TimePage:
                    {
                        IsTimePage = true;
                        IsHumidityPage = false;
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
                        IsSwipePage = false;
                        IsShedulerPage = false;
                    }
                    break;
                case ActivePageState.SwipePage:
                    {
                        IsSwipePage = true;
                        IsTimePage = false;
                        IsHumidityPage = false;
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
                        IsShedulerPage = false;
                    }
                    break;
                case ActivePageState.ShedulerPage:
                    {
                        IsShedulerPage = true;
                        IsSwipePage = false;
                        IsTimePage = false;
                        IsHumidityPage = false;
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
                        break;
                    }
            }
        }
    }
}