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
        KithchenTimerPage
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
            set { 
                _isKitchenTimerPage = value;
                OnPropertyChanged(nameof(IsKitchenTimerPage));
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
                    }
                    break;
                case ActivePageState.ChooseModePage:
                    {
                        IsChooseModePage = true;
                        IsMainPage = false;
                        IsLoadingPage = false;
                        IsKitchenTimerPage = false;
                    }
                    break;
                case ActivePageState.LoadingPage:
                    {
                        IsLoadingPage = true;
                        IsChooseModePage = false;
                        IsMainPage = false;
                        IsKitchenTimerPage = false;
                    }
                    break;
                case ActivePageState.KithchenTimerPage:
                    {
                        IsKitchenTimerPage = true;
                        IsLoadingPage = false;
                        IsChooseModePage = false;
                        IsMainPage = false;
                        _modesEntities.Mode2ValuesList[1].TimeModeValues[0].Hour = _modesEntities.Mode2ValuesList[1].TimeModeValues[0].Minute;
                    }
                    break;
            }
        }
    }

}
