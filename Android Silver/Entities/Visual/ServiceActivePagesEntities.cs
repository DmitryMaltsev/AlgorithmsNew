using Android_Silver.Entities.FBEntities;
using Android_Silver.Entities.Modes;
using Android_Silver.Entities.Visual.Menus;
using Android_Silver.ViewModels;

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
        TmhSettingsPage,
        ThmCalibratePage,
        TConstThmPage,
        MBRecupSettingsPage,
        FiltersSettingsPage,
        SpecModeSettingsPage,
        ConfigPage,
        RecupCurrentPage,
        LoadingPage
    }

    public class ServiceActivePagesEntities : BindableBase
    {
        MenusEntities _menusEntities;
        FBs _fbs;
        ModesEntities _modesEntities;
        public bool EntryIsEntered = false;
        public SActivePageState LastActivePageState { get; set; } = SActivePageState.EntryPage;

        #region Rising properties
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

        private bool _isFBSettingsPage;

        public bool IsFBSettingsPage
        {
            get { return _isFBSettingsPage; }
            set
            {
                _isFBSettingsPage = value;
                OnPropertyChanged(nameof(IsFBSettingsPage));
            }
        }



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
            set
            {
                _isDamperSettingsPage = value;
                OnPropertyChanged(nameof(IsDamperSettingsPage));
            }
        }

        private bool _isWHSettingsPage;

        public bool IsWHSettingsPage
        {
            get { return _isWHSettingsPage; }
            set
            {
                _isWHSettingsPage = value;
                OnPropertyChanged(nameof(IsWHSettingsPage));
            }
        }

        private bool _isEHSettingsPage;

        public bool IsEHSettingsPage
        {
            get { return _isEHSettingsPage; }
            set
            {
                _isEHSettingsPage = value;
                OnPropertyChanged(nameof(IsEHSettingsPage));
            }
        }

        private bool _isFreonSettingsPage;

        public bool IsFreonSettingsPage
        {
            get { return _isFreonSettingsPage; }
            set
            {
                _isFreonSettingsPage = value;
                OnPropertyChanged(nameof(IsFreonSettingsPage));
            }
        }


        private bool _isRecupSettingsPage;

        public bool IsRecupSettingsPage
        {
            get { return _isRecupSettingsPage; }
            set
            {
                _isRecupSettingsPage = value;
                OnPropertyChanged(nameof(IsRecupSettingsPage));
            }
        }

        private bool _isHumSettingsPage;
        public bool IsHumSettingsPage
        {
            get { return _isHumSettingsPage; }
            set
            {
                _isHumSettingsPage = value;
                OnPropertyChanged(nameof(IsHumSettingsPage));
            }
        }

        private bool _isSensorsSettingsPage;
        public bool IsSensorsSettingsPage
        {
            get { return _isSensorsSettingsPage; }
            set
            {
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

        private bool _istmhSettingsPage;

        public bool IsTmhSettingsPage
        {
            get { return _istmhSettingsPage; }
            set
            {
                _istmhSettingsPage = value;
                OnPropertyChanged(nameof(IsTmhSettingsPage));
            }
        }


        private bool _isConfigPage;
        public bool IsConfigPage
        {
            get { return _isConfigPage; }
            set
            {
                _isConfigPage = value;
                OnPropertyChanged(nameof(IsConfigPage));
            }
        }

        private bool _isMBRecupSettingsPage;

        public bool IsMBRecupSettingsPage
        {
            get { return _isMBRecupSettingsPage; }
            set
            {
                _isMBRecupSettingsPage = value;
                OnPropertyChanged(nameof(IsMBRecupSettingsPage));
            }
        }

        private bool _isFiltersSettingsPage;

        public bool IsFiltersSettingsPage
        {
            get { return _isFiltersSettingsPage; }
            set
            {
                _isFiltersSettingsPage = value;
                OnPropertyChanged($"{nameof(IsFiltersSettingsPage)}");
            }
        }

        private bool _isSpecModesSettingsPage;

        public bool IsSpecModeSettingsPage
        {
            get { return _isSpecModesSettingsPage; }
            set
            {
                _isSpecModesSettingsPage = value;
                OnPropertyChanged(nameof(IsSpecModeSettingsPage));
            }
        }

        private bool _isThmCalibratePage;

        public bool IsThmCalibratePage
        {
            get { return _isThmCalibratePage; }
            set
            {
                _isThmCalibratePage = value;
                OnPropertyChanged(nameof(IsThmCalibratePage));
            }
        }

        private bool _isTConstThmPage;

        public bool IsTConstThmPage
        {
            get { return _isTConstThmPage; }
            set
            {
                _isTConstThmPage = value;
                OnPropertyChanged(nameof(IsTConstThmPage));
            }
        }


        private bool _isRecupCurrentPage;
        public bool IsRecupCurrentPage
        {
            get { return _isRecupCurrentPage; }
            set
            {
                _isRecupCurrentPage = value;
                OnPropertyChanged(nameof(IsRecupCurrentPage));
            }
        }



        #endregion

        public ServiceActivePagesEntities()
        {
            _menusEntities = DIContainer.Resolve<MenusEntities>();
            _fbs = DIContainer.Resolve<FBs>();
            _modesEntities = DIContainer.Resolve<ModesEntities>();
        }

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
                        IsFreonSettingsPage = false;
                        IsFBSettingsPage = false;
                        IsLoadingPage = false;
                        IsTmhSettingsPage = false;
                        IsMBRecupSettingsPage = false;
                        IsFiltersSettingsPage = false;
                        IsSpecModeSettingsPage = false;
                        IsRecupCurrentPage = false;
                        IsTConstThmPage = false;
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
                        IsFreonSettingsPage = false;
                        IsFBSettingsPage = false;
                        IsLoadingPage = false;
                        IsTmhSettingsPage = false;
                        IsMBRecupSettingsPage = false;
                        IsFiltersSettingsPage = false;
                        IsSpecModeSettingsPage = false;
                        IsRecupCurrentPage = false;
                        IsTConstThmPage = false;
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
                        IsFreonSettingsPage = false;
                        IsFBSettingsPage = false;
                        IsLoadingPage = false;
                        IsTmhSettingsPage = false;
                        IsMBRecupSettingsPage = false;
                        IsFiltersSettingsPage = false;
                        IsSpecModeSettingsPage = false;
                        IsRecupCurrentPage = false;
                        IsTConstThmPage = false;
                    }
                    break;
                case SActivePageState.BaseSettingsPage:
                    {
                        _menusEntities.GenerateBaseTable();
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
                        IsFreonSettingsPage = false;
                        IsFBSettingsPage = false;
                        IsLoadingPage = false;
                        IsTmhSettingsPage = false;
                        IsMBRecupSettingsPage = false;
                        IsFiltersSettingsPage = false;
                        IsSpecModeSettingsPage = false;
                        IsRecupCurrentPage = false;
                        IsTConstThmPage = false;
                    }
                    break;
                case SActivePageState.ConfigPage:
                    {
                        _menusEntities.StartMenuCollection[0].StrSetsCollection[0].CPickVal = _fbs.CEConfig.ET1;
                        _menusEntities.StartMenuCollection[0].StrSetsCollection[1].CPickVal = _fbs.CEConfig.ET2;
                        _menusEntities.StartMenuCollection[0].StrSetsCollection[2].CPickVal = _fbs.CEConfig.OUT1;
                        _menusEntities.StartMenuCollection[0].StrSetsCollection[3].CPickVal = _fbs.CEConfig.OUT2;
                        _menusEntities.StartMenuCollection[0].StrSetsCollection[4].CPickVal = _fbs.CEConfig.AR1;
                        _menusEntities.StartMenuCollection[0].StrSetsCollection[5].CPickVal = _fbs.CEConfig.AR2;
                        _menusEntities.StartMenuCollection[0].StrSetsCollection[6].CPickVal = _fbs.CEConfig.AR3;
                        _menusEntities.StartMenuCollection[0].StrSetsCollection[7].CPickVal = _fbs.CEConfig.Recup;
                        _menusEntities.Title = _menusEntities.StartMenuCollection[0].Name;
                        _menusEntities.GenerateInterfaceTable(0);
                        IsConfigPage = true;
                        IsFBSettingsPage = true;
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
                        IsFreonSettingsPage = false;
                        IsLoadingPage = false;
                        IsTmhSettingsPage = false;
                        IsMBRecupSettingsPage = false;
                        IsFiltersSettingsPage = false;
                        IsSpecModeSettingsPage = false;
                        IsRecupCurrentPage = false;
                        IsTConstThmPage = false;
                    }
                    break;
                case SActivePageState.CommonSettingsPage:
                    {
                        IsCommonSettingsPage = true;
                        IsFBSettingsPage = true;
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
                        IsFreonSettingsPage = false;
                        IsLoadingPage = false;
                        IsTmhSettingsPage = false;
                        IsMBRecupSettingsPage = false;
                        IsFiltersSettingsPage = false;
                        IsSpecModeSettingsPage = false;
                        IsRecupCurrentPage = false;
                        IsTConstThmPage = false;
                        _menusEntities.Title = _menusEntities.StartMenuCollection[1].Name;
                        // _menuEntities.
                        _menusEntities.StartMenuCollection[1].StrSetsCollection[0].CVal = _fbs.CCommonSetPoints.SPTempAlarm;
                        _menusEntities.StartMenuCollection[1].StrSetsCollection[1].CPickVal = _fbs.CEConfig.TregularCh_R;
                        _menusEntities.StartMenuCollection[1].StrSetsCollection[2].CVal = _fbs.CCommonSetPoints.SPTempMaxCh;
                        _menusEntities.StartMenuCollection[1].StrSetsCollection[3].CVal = _fbs.CCommonSetPoints.SPTempMinCh;
                        _menusEntities.StartMenuCollection[1].StrSetsCollection[4].CVal = _fbs.CCommonSetPoints.TControlDelayS;
                        _menusEntities.StartMenuCollection[1].StrSetsCollection[5].CPickVal = _fbs.CCommonSetPoints.SeasonMode;
                        _menusEntities.StartMenuCollection[1].StrSetsCollection[6].CVal = _fbs.CCommonSetPoints.SPSeason;
                        _menusEntities.StartMenuCollection[1].StrSetsCollection[7].CVal = _fbs.CCommonSetPoints.HystSeason;
                        _menusEntities.StartMenuCollection[1].StrSetsCollection[8].CPickVal = _fbs.CEConfig.AutoRestart;
                        _menusEntities.StartMenuCollection[1].StrSetsCollection[9].CPickVal = _fbs.CEConfig.AutoResetFire;
                        _menusEntities.StartMenuCollection[1].StrSetsCollection[10].CVal = _fbs.UFLeds.LEDsI;
                        _menusEntities.StartMenuCollection[1].StrSetsCollection[11].CPickVal = _fbs.CEConfig.IsDemoConfig;
                        _menusEntities.StartMenuCollection[1].StrSetsCollection[12].CVal = _fbs.CCommonSetPoints.RoomSPPReg.Value;
                        _menusEntities.StartMenuCollection[1].StrSetsCollection[13].CVal = _fbs.CCommonSetPoints.RoomSPIReg.Value;
                        _menusEntities.StartMenuCollection[1].StrSetsCollection[14].CVal = _fbs.CCommonSetPoints.RoomSPDReg.Value;
                        _menusEntities.GenerateInterfaceTable(1);
                    }
                    break;
                case SActivePageState.DamperSettingsPage:
                    {
                        _menusEntities.Title = _menusEntities.StartMenuCollection[2].Name;
                        _menusEntities.StartMenuCollection[2].StrSetsCollection[0].CVal = _fbs.CDamperSetPoints.DamperOpenTime;
                        _menusEntities.StartMenuCollection[2].StrSetsCollection[1].CVal = _fbs.CDamperSetPoints.DamperHeatingTime;
                        _menusEntities.StartMenuCollection[2].StrSetsCollection[2].CVal = _fbs.CDamperSetPoints.ServoDampers[0].StartPos;
                        _menusEntities.StartMenuCollection[2].StrSetsCollection[3].CVal = _fbs.CDamperSetPoints.ServoDampers[0].EndPos;
                        _menusEntities.StartMenuCollection[2].StrSetsCollection[4].CVal = _fbs.CDamperSetPoints.ServoDampers[1].StartPos;
                        _menusEntities.StartMenuCollection[2].StrSetsCollection[5].CVal = _fbs.CDamperSetPoints.ServoDampers[1].EndPos;
                        _menusEntities.StartMenuCollection[2].StrSetsCollection[6].CVal = _fbs.CDamperSetPoints.ServoDampers[2].StartPos;
                        _menusEntities.StartMenuCollection[2].StrSetsCollection[7].CVal = _fbs.CDamperSetPoints.ServoDampers[2].EndPos;
                        _menusEntities.StartMenuCollection[2].StrSetsCollection[8].CVal = _fbs.CDamperSetPoints.ServoDampers[3].StartPos;
                        _menusEntities.StartMenuCollection[2].StrSetsCollection[9].CVal = _fbs.CDamperSetPoints.ServoDampers[3].EndPos;
                        _menusEntities.StartMenuCollection[2].StrSetsCollection[10].CVal = _fbs.CDamperSetPoints.ServoDampers[0].CloseAngle;
                        _menusEntities.StartMenuCollection[2].StrSetsCollection[11].CVal = _fbs.CDamperSetPoints.ServoDampers[0].OpenAngle;
                        _menusEntities.StartMenuCollection[2].StrSetsCollection[12].CVal = _fbs.CDamperSetPoints.ServoDampers[1].CloseAngle;
                        _menusEntities.StartMenuCollection[2].StrSetsCollection[13].CVal = _fbs.CDamperSetPoints.ServoDampers[1].OpenAngle;
                        _menusEntities.StartMenuCollection[2].StrSetsCollection[14].CVal = _fbs.CDamperSetPoints.ServoDampers[2].CloseAngle;
                        _menusEntities.StartMenuCollection[2].StrSetsCollection[15].CVal = _fbs.CDamperSetPoints.ServoDampers[2].OpenAngle;
                        _menusEntities.StartMenuCollection[2].StrSetsCollection[16].CVal = _fbs.CDamperSetPoints.ServoDampers[3].CloseAngle;
                        _menusEntities.StartMenuCollection[2].StrSetsCollection[17].CVal = _fbs.CDamperSetPoints.ServoDampers[3].OpenAngle;
                        _menusEntities.StartMenuCollection[2].StrSetsCollection[18].CPickVal = _fbs.CDamperSetPoints.isTest;
                        _menusEntities.StartMenuCollection[2].StrSetsCollection[19].CVal = _fbs.CDamperSetPoints.ServoDampers[0].CalPos;
                        _menusEntities.StartMenuCollection[2].StrSetsCollection[20].CVal = _fbs.CDamperSetPoints.ServoDampers[1].CalPos;
                        _menusEntities.StartMenuCollection[2].StrSetsCollection[21].CVal = _fbs.CDamperSetPoints.ServoDampers[2].CalPos;
                        _menusEntities.StartMenuCollection[2].StrSetsCollection[22].CVal = _fbs.CDamperSetPoints.ServoDampers[3].CalPos;
                        //   _menusEntities.StartMenuCollection[1].StrSetsCollection[1].CVal = _fbs.CDamperSetPoints.DamperHeatingTime;
                        _menusEntities.GenerateInterfaceTable(2);
                        IsDamperSettingsPage = true;
                        IsFBSettingsPage = true;
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
                        IsFreonSettingsPage = false;
                        IsLoadingPage = false;
                        IsTmhSettingsPage = false;
                        IsMBRecupSettingsPage = false;
                        IsFiltersSettingsPage = false;
                        IsSpecModeSettingsPage = false;
                        IsRecupCurrentPage = false;
                        IsTConstThmPage = false;
                    }
                    break;
                case SActivePageState.FanSettingsPage:
                    {
                        _menusEntities.Title = _menusEntities.StartMenuCollection[3].Name;
                        _menusEntities.StartMenuCollection[3].StrSetsCollection[0].CVal = _fbs.CFans.SFanNominalFlow;
                        _menusEntities.StartMenuCollection[3].StrSetsCollection[1].CVal = _fbs.CFans.EFanNominalFlow;
                        _menusEntities.StartMenuCollection[3].StrSetsCollection[2].CVal = _fbs.CFans.LowLimitBan;
                        _menusEntities.StartMenuCollection[3].StrSetsCollection[3].CVal = _fbs.CFans.HighLimitBan;
                        _menusEntities.StartMenuCollection[3].StrSetsCollection[4].CVal = _fbs.CFans.PressureFailureDelay;
                        _menusEntities.StartMenuCollection[3].StrSetsCollection[5].CVal = _fbs.CFans.FanFailureDelay;
                        _menusEntities.StartMenuCollection[3].StrSetsCollection[6].CPickVal = _fbs.CFans.DecrFanConfig;
                        _menusEntities.StartMenuCollection[3].StrSetsCollection[7].CVal = _fbs.CFans.PDecrFan;
                        _menusEntities.StartMenuCollection[3].StrSetsCollection[8].CVal = _fbs.CFans.IDecrFan;
                        _menusEntities.StartMenuCollection[3].StrSetsCollection[9].CVal = _fbs.CFans.DDecrFan;
                        _menusEntities.StartMenuCollection[3].StrSetsCollection[10].CVal = _fbs.CFans.MinFanPercent;
                        _menusEntities.GenerateInterfaceTable(3);
                        IsFanSettingsPage = true;
                        IsFBSettingsPage = true;
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
                        IsFreonSettingsPage = false;
                        IsLoadingPage = false;
                        IsTmhSettingsPage = false;
                        IsMBRecupSettingsPage = false;
                        IsFiltersSettingsPage = false;
                        IsSpecModeSettingsPage = false;
                        IsRecupCurrentPage = false;
                        IsTConstThmPage = false;
                    }
                    break;
                case SActivePageState.WHSettingsPage:
                    {
                        _menusEntities.Title = _menusEntities.StartMenuCollection[4].Name;
                        _menusEntities.StartMenuCollection[4].StrSetsCollection[0].CVal = _fbs.CWHSetPoints.PWork;
                        _menusEntities.StartMenuCollection[4].StrSetsCollection[1].CVal = _fbs.CWHSetPoints.IWork;
                        _menusEntities.StartMenuCollection[4].StrSetsCollection[2].CVal = _fbs.CWHSetPoints.DWork;
                        _menusEntities.StartMenuCollection[4].StrSetsCollection[3].CVal = _fbs.CWHSetPoints.PRet;
                        _menusEntities.StartMenuCollection[4].StrSetsCollection[4].CVal = _fbs.CWHSetPoints.IRet;
                        _menusEntities.StartMenuCollection[4].StrSetsCollection[5].CVal = _fbs.CWHSetPoints.DRet;
                        _menusEntities.StartMenuCollection[4].StrSetsCollection[6].CVal = _fbs.CWHSetPoints.TRetMax;
                        _menusEntities.StartMenuCollection[4].StrSetsCollection[7].CVal = _fbs.CWHSetPoints.TRetMin;
                        _menusEntities.StartMenuCollection[4].StrSetsCollection[8].CVal = _fbs.CWHSetPoints.TRetStb;
                        _menusEntities.StartMenuCollection[4].StrSetsCollection[9].CVal = _fbs.CWHSetPoints.TRetF;
                        _menusEntities.StartMenuCollection[4].StrSetsCollection[10].CVal = _fbs.CWHSetPoints.TRetStart;
                        _menusEntities.StartMenuCollection[4].StrSetsCollection[11].CVal = _fbs.CWHSetPoints.TRetStart;
                        _menusEntities.StartMenuCollection[4].StrSetsCollection[12].CVal = _fbs.CWHSetPoints.SSMaxIntervalS;
                        _menusEntities.StartMenuCollection[4].StrSetsCollection[13].CVal = _fbs.CWHSetPoints.MinDamperPerc;
                        _menusEntities.StartMenuCollection[4].StrSetsCollection[14].CVal = _fbs.CWHSetPoints.SPWinterProcess;
                        _menusEntities.StartMenuCollection[4].StrSetsCollection[15].CPickVal = _fbs.CWHSetPoints.IsSummerTestPump;
                        _menusEntities.GenerateInterfaceTable(4);
                        IsWHSettingsPage = true;
                        IsFBSettingsPage = true;
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
                        IsFreonSettingsPage = false;
                        IsLoadingPage = false;
                        IsTmhSettingsPage = false;
                        IsMBRecupSettingsPage = false;
                        IsFiltersSettingsPage = false;
                        IsSpecModeSettingsPage = false;
                        IsRecupCurrentPage = false;
                        IsTConstThmPage = false;
                    }
                    break;
                case SActivePageState.EHSettingsPage:
                    {
                        _menusEntities.Title = _menusEntities.StartMenuCollection[5].Name;
                        _menusEntities.StartMenuCollection[5].StrSetsCollection[0].CVal = _fbs.CEHSetPoints.NomPowerVT;
                        _menusEntities.StartMenuCollection[5].StrSetsCollection[1].CVal = _fbs.CEHSetPoints.PReg;
                        _menusEntities.StartMenuCollection[5].StrSetsCollection[2].CVal = _fbs.CEHSetPoints.IReg;
                        _menusEntities.StartMenuCollection[5].StrSetsCollection[3].CVal = _fbs.CEHSetPoints.DReg;
                        _menusEntities.StartMenuCollection[5].StrSetsCollection[4].CVal = _fbs.CEHSetPoints.BlowDownTime;
                        _menusEntities.GenerateInterfaceTable(5);
                        IsEHSettingsPage = true;
                        IsFBSettingsPage = true;
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
                        IsFreonSettingsPage = false;
                        IsLoadingPage = false;
                        IsTmhSettingsPage = false;
                        IsMBRecupSettingsPage = false;
                        IsFiltersSettingsPage = false;
                        IsSpecModeSettingsPage = false;
                        IsRecupCurrentPage = false;
                        IsTConstThmPage = false;
                    }
                    break;
                case SActivePageState.FreonSettingsPage:
                    {
                        _menusEntities.Title = _menusEntities.StartMenuCollection[6].Name;
                        _menusEntities.StartMenuCollection[6].StrSetsCollection[0].CVal = _fbs.CFreonCoolerSP.PReg;
                        _menusEntities.StartMenuCollection[6].StrSetsCollection[1].CVal = _fbs.CFreonCoolerSP.IReg;
                        _menusEntities.StartMenuCollection[6].StrSetsCollection[2].CVal = _fbs.CFreonCoolerSP.DReg;
                        _menusEntities.StartMenuCollection[6].StrSetsCollection[3].CVal = _fbs.CFreonCoolerSP.Stage1OnS;
                        _menusEntities.StartMenuCollection[6].StrSetsCollection[4].CVal = _fbs.CFreonCoolerSP.Stage1OffS;
                        _menusEntities.StartMenuCollection[6].StrSetsCollection[5].CVal = _fbs.CFreonCoolerSP.Hyst;
                        _menusEntities.GenerateInterfaceTable(6);
                        IsFreonSettingsPage = true;
                        IsFBSettingsPage = true;
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
                        IsHumSettingsPage = false;
                        IsSensorsSettingsPage = false;
                        IsConfigPage = false;
                        IsLoadingPage = false;
                        IsTmhSettingsPage = false;
                        IsMBRecupSettingsPage = false;
                        IsFiltersSettingsPage = false;
                        IsSpecModeSettingsPage = false;
                        IsRecupCurrentPage = false;
                        IsTConstThmPage = false;
                    }
                    break;
                case SActivePageState.RecupSettingsPage:
                    {
                        _menusEntities.Title = _menusEntities.StartMenuCollection[7].Name;
                        _menusEntities.StartMenuCollection[7].StrSetsCollection[0].CVal = _fbs.CRecup.PReg;
                        _menusEntities.StartMenuCollection[7].StrSetsCollection[1].CVal = _fbs.CRecup.IReg;
                        _menusEntities.StartMenuCollection[7].StrSetsCollection[2].CVal = _fbs.CRecup.DReg;
                        _menusEntities.StartMenuCollection[7].StrSetsCollection[3].CVal = _fbs.CRecup.TEffSP;
                        _menusEntities.StartMenuCollection[7].StrSetsCollection[4].CVal = _fbs.CRecup.EffFailValue;
                        _menusEntities.StartMenuCollection[7].StrSetsCollection[5].CVal = _fbs.CRecup.EffFailDelay;
                        _menusEntities.StartMenuCollection[7].StrSetsCollection[6].CVal = _fbs.CRecup.HZMax;
                        _menusEntities.StartMenuCollection[7].StrSetsCollection[7].CVal = _fbs.CRecup.TempA;
                        _menusEntities.StartMenuCollection[7].StrSetsCollection[8].CVal = _fbs.CRecup.TempB;
                        _menusEntities.StartMenuCollection[7].StrSetsCollection[9].CVal = _fbs.CRecup.TempC;
                        _menusEntities.StartMenuCollection[7].StrSetsCollection[10].CVal = _fbs.CRecup.TempD;
                        _menusEntities.GenerateInterfaceTable(7);
                        IsRecupSettingsPage = true;
                        IsFBSettingsPage = true;
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
                        IsFreonSettingsPage = false;
                        IsLoadingPage = false;
                        IsTmhSettingsPage = false;
                        IsMBRecupSettingsPage = false;
                        IsFiltersSettingsPage = false;
                        IsSpecModeSettingsPage = false;
                        IsRecupCurrentPage = false;
                        IsTConstThmPage = false;
                    }
                    break;
                case SActivePageState.HumSettingsPage:
                    {
                        _menusEntities.Title = _menusEntities.StartMenuCollection[8].Name;
                        _menusEntities.StartMenuCollection[8].StrSetsCollection[0].CVal = _fbs.CHumiditySPS.PReg;
                        _menusEntities.StartMenuCollection[8].StrSetsCollection[1].CVal = _fbs.CHumiditySPS.IReg;
                        _menusEntities.StartMenuCollection[8].StrSetsCollection[2].CVal = _fbs.CHumiditySPS.DReg;
                        _menusEntities.StartMenuCollection[8].StrSetsCollection[3].CVal = _fbs.CHumiditySPS.Stage1OnS;
                        _menusEntities.StartMenuCollection[8].StrSetsCollection[4].CVal = _fbs.CHumiditySPS.Stage1OffS;
                        _menusEntities.StartMenuCollection[8].StrSetsCollection[5].CVal = _fbs.CHumiditySPS.Hyst;
                        _menusEntities.GenerateInterfaceTable(8);
                        IsHumSettingsPage = true;
                        IsFBSettingsPage = true;
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
                        IsFreonSettingsPage = false;
                        IsLoadingPage = false;
                        IsTmhSettingsPage = false;
                        IsMBRecupSettingsPage = false;
                        IsFiltersSettingsPage = false;
                        IsSpecModeSettingsPage = false;
                        IsRecupCurrentPage = false;
                        IsTConstThmPage = false;
                    }
                    break;
                case SActivePageState.SensorsSettingPage:
                    {
                        if (_fbs.CEConfig.TOutDoorConfig == 2 && _menusEntities.StartMenuCollection[8].StrSetsCollection[0].CPickVal == 2)
                        {
                            _fbs.CEConfig.TOutDoorConfig = 1;
                        }

                        //_menusEntities.StartMenuCollection[9].StrSetsCollection[0].CPickVal = _fbs.CEConfig.TOutDoorConfig;
                        //_menusEntities.StartMenuCollection[9].StrSetsCollection[1].CPickVal = _fbs.CEConfig.TSupplyFConfig;
                        //_menusEntities.StartMenuCollection[9].StrSetsCollection[2].CPickVal = _fbs.CEConfig.TExhaustFConfig;
                        //_menusEntities.StartMenuCollection[9].StrSetsCollection[3].CPickVal = _fbs.CEConfig.TRoomConfig;
                        //_menusEntities.StartMenuCollection[9].StrSetsCollection[4].CPickVal = _fbs.CEConfig.TReturnWaterConfig;
                        //_menusEntities.StartMenuCollection[9].StrSetsCollection[5].CPickVal = _fbs.CEConfig.AirQualityConfig;
                        //_menusEntities.StartMenuCollection[9].StrSetsCollection[6].CPickVal = _fbs.CEConfig.HumSensorConfig;
                        _menusEntities.Title = _menusEntities.StartMenuCollection[9].Name;
                        _menusEntities.StartMenuCollection[9].StrSetsCollection[0].CVal = _fbs.CSensors.OutdoorTemp.Correction;
                        _menusEntities.StartMenuCollection[9].StrSetsCollection[1].CVal = _fbs.CSensors.SupTemp.Correction;
                        _menusEntities.StartMenuCollection[9].StrSetsCollection[2].CVal = _fbs.CSensors.ExhaustTemp.Correction;
                        _menusEntities.StartMenuCollection[9].StrSetsCollection[3].CVal = _fbs.CSensors.RoomTemp.Correction;
                        _menusEntities.StartMenuCollection[9].StrSetsCollection[4].CVal = _fbs.CSensors.ReturnTemp.Correction;
                        _menusEntities.GenerateInterfaceTable(9);
                        IsSensorsSettingsPage = true;
                        IsFBSettingsPage = true;
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
                        IsFreonSettingsPage = false;
                        IsLoadingPage = false;
                        IsTmhSettingsPage = false;
                        IsMBRecupSettingsPage = false;
                        IsFiltersSettingsPage = false;
                        IsSpecModeSettingsPage = false;
                        IsRecupCurrentPage = false;
                        IsTConstThmPage = false;
                    }
                    break;
                case SActivePageState.TmhSettingsPage:
                    {
                        IsTmhSettingsPage = true;
                        IsFBSettingsPage = true;
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
                        IsConfigPage = false;
                        IsFreonSettingsPage = false;
                        IsLoadingPage = false;
                        IsMBRecupSettingsPage = false;
                        IsFiltersSettingsPage = false;
                        IsSpecModeSettingsPage = false;
                        IsRecupCurrentPage = false;
                        IsTConstThmPage = false;
                        _menusEntities.Title = _menusEntities.StartMenuCollection[10].Name;
                        _menusEntities.StartMenuCollection[10].StrSetsCollection[0].CPickVal = _fbs.SupCalibrateThm.FanControlType.Value;
                        _menusEntities.StartMenuCollection[10].StrSetsCollection[1].CVal = _fbs.ThmSps.TempH1;
                        _menusEntities.StartMenuCollection[10].StrSetsCollection[2].CVal = _fbs.ThmSps.TempC1;
                        _menusEntities.StartMenuCollection[10].StrSetsCollection[3].CVal = _fbs.ThmSps.TempH2;
                        _menusEntities.StartMenuCollection[10].StrSetsCollection[4].CVal = _fbs.ThmSps.TempC2;
                        _menusEntities.StartMenuCollection[10].StrSetsCollection[5].CVal = _fbs.ThmSps.PReg.Value;
                        _menusEntities.StartMenuCollection[10].StrSetsCollection[6].CVal = _fbs.ThmSps.IReg.Value;
                        _menusEntities.StartMenuCollection[10].StrSetsCollection[7].CVal = _fbs.ThmSps.DReg.Value;
                        _menusEntities.StartMenuCollection[10].StrSetsCollection[8].CVal = _fbs.ThmSps.KPolKoef.Value;
                        _menusEntities.StartMenuCollection[10].StrSetsCollection[9].CVal = _fbs.ThmSps.BPolKoef.Value;
                        _menusEntities.StartMenuCollection[10].StrSetsCollection[10].CVal = _fbs.ThmSps.KClKoef.Value;
                        _menusEntities.StartMenuCollection[10].StrSetsCollection[11].CVal = _fbs.ThmSps.BClKoef.Value;
                        _menusEntities.StartMenuCollection[10].StrSetsCollection[12].CVal = _fbs.ThmSps.PThmSup.Value;
                        _menusEntities.GenerateInterfaceTable(10);
                    }
                    break;
                case SActivePageState.LoadingPage:
                    {
                        IsLoadingPage = true;
                        IsConfigPage = false;
                        IsFBSettingsPage = false;
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
                        IsFreonSettingsPage = false;
                        IsTmhSettingsPage = false;
                        IsMBRecupSettingsPage = false;
                        IsFiltersSettingsPage = false;
                        IsSpecModeSettingsPage = false;
                        IsRecupCurrentPage = false;
                        IsTConstThmPage = false;
                    }
                    break;
                case SActivePageState.MBRecupSettingsPage:
                    {
                        IsMBRecupSettingsPage = true;
                        IsFBSettingsPage = true;
                        IsLoadingPage = false;
                        IsConfigPage = false;
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
                        IsFreonSettingsPage = false;
                        IsTmhSettingsPage = false;
                        IsFiltersSettingsPage = false;
                        IsSpecModeSettingsPage = false;
                        IsRecupCurrentPage = false;
                        IsTConstThmPage = false;
                        _menusEntities.Title = _menusEntities.StartMenuCollection[11].Name;
                        _menusEntities.StartMenuCollection[11].StrSetsCollection[0].CPickVal = _fbs.MbRecSPs.MBRecMode;
                        _menusEntities.StartMenuCollection[11].StrSetsCollection[1].CPickVal = _fbs.MbRecSPs.IsRotTest1;
                        _menusEntities.StartMenuCollection[11].StrSetsCollection[2].CPickVal = _fbs.MbRecSPs.IsRotTest2;
                        _menusEntities.StartMenuCollection[11].StrSetsCollection[3].CPickVal = _fbs.MbRecSPs.IsForward1;
                        _menusEntities.StartMenuCollection[11].StrSetsCollection[4].CPickVal = _fbs.MbRecSPs.IsForward2;
                        _menusEntities.StartMenuCollection[11].StrSetsCollection[5].CPickVal = _fbs.MbRecSPs.IsGrindingMode;
                        _menusEntities.StartMenuCollection[11].StrSetsCollection[6].CVal = _fbs.MbRecSPs.NominalCurrent;
                        _menusEntities.StartMenuCollection[11].StrSetsCollection[7].CVal = _fbs.MbRecSPs.ReductKoef;
                        _menusEntities.StartMenuCollection[11].StrSetsCollection[8].CVal = _fbs.MbRecSPs.NominalTurns1;
                        _menusEntities.StartMenuCollection[11].StrSetsCollection[9].CVal = _fbs.MbRecSPs.NominalTurns2;
                        _menusEntities.StartMenuCollection[11].StrSetsCollection[10].CVal = _fbs.MbRecSPs.NominalTemp1;
                        _menusEntities.StartMenuCollection[11].StrSetsCollection[11].CVal = _fbs.MbRecSPs.NominalTemp2;
                        _menusEntities.StartMenuCollection[11].StrSetsCollection[12].CVal = _fbs.MbRecSPs.GrindingCurrent;
                        _menusEntities.StartMenuCollection[11].StrSetsCollection[13].CVal = _fbs.MbRecSPs.GrindingTurns;
                        _menusEntities.GenerateInterfaceTable(11);
                    }
                    break;
                case (SActivePageState.SpecModeSettingsPage):
                    {
                        IsSpecModeSettingsPage = true;
                        IsFBSettingsPage = true;
                        IsMBRecupSettingsPage = false;
                        IsLoadingPage = false;
                        IsConfigPage = false;
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
                        IsFreonSettingsPage = false;
                        IsTmhSettingsPage = false;
                        IsFiltersSettingsPage = false;
                        IsRecupCurrentPage = false;
                        IsTConstThmPage = false;
                        _menusEntities.Title = _menusEntities.StartMenuCollection[12].Name;
                        _menusEntities.StartMenuCollection[12].StrSetsCollection[0].CVal = _modesEntities.Mode1ValuesList[6].SupMinVal;
                        _menusEntities.StartMenuCollection[12].StrSetsCollection[2].CVal = _modesEntities.Mode1ValuesList[6].SupMaxVal;
                        _menusEntities.StartMenuCollection[12].StrSetsCollection[3].CVal = _modesEntities.Mode1ValuesList[6].ExhaustMinVal;
                        _menusEntities.StartMenuCollection[12].StrSetsCollection[5].CVal = _modesEntities.Mode1ValuesList[6].ExhaustMaxVal;
                        _menusEntities.StartMenuCollection[12].StrSetsCollection[6].CVal = _modesEntities.Mode1ValuesList[6].TempSP;
                        _menusEntities.StartMenuCollection[12].StrSetsCollection[7].CVal = _modesEntities.Mode1ValuesList[6].PowerLimitSP;
                        _menusEntities.GenerateInterfaceTable(12);
                    }
                    break;
                case (SActivePageState.ThmCalibratePage):
                    {
                        IsThmCalibratePage = true;
                        IsSpecModeSettingsPage = false;
                        IsFBSettingsPage = true;
                        IsMBRecupSettingsPage = false;
                        IsLoadingPage = false;
                        IsConfigPage = false;
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
                        IsFreonSettingsPage = false;
                        IsTmhSettingsPage = true;
                        IsFiltersSettingsPage = false;
                        IsRecupCurrentPage = false;
                        IsTConstThmPage = false;
                        _menusEntities.Title = _menusEntities.StartMenuCollection[13].Name;

                        _menusEntities.StartMenuCollection[13].StrSetsCollection[0].CPickVal = _fbs.SupCalibrateThm.CalibrateMode.Value;
                        _menusEntities.StartMenuCollection[13].StrSetsCollection[1].CVal = _fbs.SupCalibrateThm.DeltaThm.Value;
                        _menusEntities.StartMenuCollection[13].StrSetsCollection[2].CVal = _fbs.ExhaustCalibrateThm.DeltaThm.Value;
                        _menusEntities.StartMenuCollection[13].StrSetsCollection[3].CVal = _fbs.ThmSps.PThmSupValue.Value;
                        _menusEntities.StartMenuCollection[13].StrSetsCollection[4].CVal = _fbs.ThmSps.PThmExhaustValue.Value;
                        _menusEntities.StartMenuCollection[13].StrSetsCollection[5].CPickVal = _fbs.SupCalibrateThm.CavType.Value;
                        _menusEntities.StartMenuCollection[13].StrSetsCollection[6].CVal = _fbs.SupCalibrateThm.CalibrateTimeS.Value;
                        _menusEntities.StartMenuCollection[13].StrSetsCollection[7].CVal = _fbs.SupCalibrateThm.TestTimeS.Value;
                        _menusEntities.StartMenuCollection[13].StrSetsCollection[8].CVal = _fbs.SupCalibrateThm.LeakFlow.Value;
                        _menusEntities.StartMenuCollection[13].StrSetsCollection[9].CVal = _fbs.ExhaustCalibrateThm.LeakFlow.Value;
                        _menusEntities.StartMenuCollection[13].StrSetsCollection[10].CVal = _fbs.SupCalibrateThm.CalibrateStepPercs[1];
                        _menusEntities.StartMenuCollection[13].StrSetsCollection[11].CVal = _fbs.SupCalibrateThm.CalibrateStepPercs[2];
                        _menusEntities.StartMenuCollection[13].StrSetsCollection[12].CVal = _fbs.SupCalibrateThm.CalibrateStepPercs[3];
                        _menusEntities.StartMenuCollection[13].StrSetsCollection[13].CVal = _fbs.SupCalibrateThm.CalibrateStepPercs[4];
                        _menusEntities.StartMenuCollection[13].StrSetsCollection[14].CVal = _fbs.SupCalibrateThm.CalibrateStepPercs[5];

                        _menusEntities.StartMenuCollection[13].StrSetsCollection[15].CVal = _fbs.SupCalibrateThm.DeltaTCalibrates[0];
                        _menusEntities.StartMenuCollection[13].StrSetsCollection[16].CVal = _fbs.SupCalibrateThm.DeltaTCalibrates[1];
                        _menusEntities.StartMenuCollection[13].StrSetsCollection[17].CVal = _fbs.SupCalibrateThm.DeltaTCalibrates[2];
                        _menusEntities.StartMenuCollection[13].StrSetsCollection[18].CVal = _fbs.SupCalibrateThm.DeltaTCalibrates[3];
                        _menusEntities.StartMenuCollection[13].StrSetsCollection[19].CVal = _fbs.SupCalibrateThm.DeltaTCalibrates[4];
                        _menusEntities.StartMenuCollection[13].StrSetsCollection[20].CVal = _fbs.SupCalibrateThm.DeltaTCalibrates[5];
                        _menusEntities.StartMenuCollection[13].StrSetsCollection[21].CVal = _fbs.SupCalibrateThm.DeltaTCalibrates[6];

                        _menusEntities.StartMenuCollection[13].StrSetsCollection[22].CVal = _fbs.SupCalibrateThm.FlowCalibrates[0];
                        _menusEntities.StartMenuCollection[13].StrSetsCollection[23].CVal = _fbs.SupCalibrateThm.FlowCalibrates[1];
                        _menusEntities.StartMenuCollection[13].StrSetsCollection[24].CVal = _fbs.SupCalibrateThm.FlowCalibrates[2];
                        _menusEntities.StartMenuCollection[13].StrSetsCollection[25].CVal = _fbs.SupCalibrateThm.FlowCalibrates[3];
                        _menusEntities.StartMenuCollection[13].StrSetsCollection[26].CVal = _fbs.SupCalibrateThm.FlowCalibrates[4];
                        _menusEntities.StartMenuCollection[13].StrSetsCollection[27].CVal = _fbs.SupCalibrateThm.FlowCalibrates[5];
                        _menusEntities.StartMenuCollection[13].StrSetsCollection[28].CVal = _fbs.SupCalibrateThm.FlowCalibrates[6];

                        _menusEntities.StartMenuCollection[13].StrSetsCollection[29].CVal = _fbs.ExhaustCalibrateThm.DeltaTCalibrates[0];
                        _menusEntities.StartMenuCollection[13].StrSetsCollection[30].CVal = _fbs.ExhaustCalibrateThm.DeltaTCalibrates[1];
                        _menusEntities.StartMenuCollection[13].StrSetsCollection[31].CVal = _fbs.ExhaustCalibrateThm.DeltaTCalibrates[2];
                        _menusEntities.StartMenuCollection[13].StrSetsCollection[32].CVal = _fbs.ExhaustCalibrateThm.DeltaTCalibrates[3];
                        _menusEntities.StartMenuCollection[13].StrSetsCollection[33].CVal = _fbs.ExhaustCalibrateThm.DeltaTCalibrates[4];
                        _menusEntities.StartMenuCollection[13].StrSetsCollection[34].CVal = _fbs.ExhaustCalibrateThm.DeltaTCalibrates[5];
                        _menusEntities.StartMenuCollection[13].StrSetsCollection[35].CVal = _fbs.ExhaustCalibrateThm.DeltaTCalibrates[6];

                        _menusEntities.StartMenuCollection[13].StrSetsCollection[36].CVal = _fbs.ExhaustCalibrateThm.FlowCalibrates[0];
                        _menusEntities.StartMenuCollection[13].StrSetsCollection[37].CVal = _fbs.ExhaustCalibrateThm.FlowCalibrates[1];
                        _menusEntities.StartMenuCollection[13].StrSetsCollection[38].CVal = _fbs.ExhaustCalibrateThm.FlowCalibrates[2];
                        _menusEntities.StartMenuCollection[13].StrSetsCollection[39].CVal = _fbs.ExhaustCalibrateThm.FlowCalibrates[3];
                        _menusEntities.StartMenuCollection[13].StrSetsCollection[40].CVal = _fbs.ExhaustCalibrateThm.FlowCalibrates[4];
                        _menusEntities.StartMenuCollection[13].StrSetsCollection[41].CVal = _fbs.ExhaustCalibrateThm.FlowCalibrates[5];
                        _menusEntities.StartMenuCollection[13].StrSetsCollection[42].CVal = _fbs.ExhaustCalibrateThm.FlowCalibrates[6];
                        _menusEntities.GenerateInterfaceTable(13);
                    }
                    break;
                case SActivePageState.TConstThmPage:
                    {
                        IsThmCalibratePage = false;
                        IsSpecModeSettingsPage = false;
                        IsFBSettingsPage = true;
                        IsMBRecupSettingsPage = false;
                        IsLoadingPage = false;
                        IsConfigPage = false;
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
                        IsFreonSettingsPage = false;
                        IsTmhSettingsPage = false;
                        IsFiltersSettingsPage = false;
                        IsRecupCurrentPage = false;
                        IsTConstThmPage = true;
                        _menusEntities.Title = _menusEntities.StartMenuCollection[14].Name;
                        _menusEntities.StartMenuCollection[14].StrSetsCollection[0].CVal = _fbs.SupCalibrateThm.CalibrateDeltaT.Value;
                        _menusEntities.StartMenuCollection[14].StrSetsCollection[1].CVal = _fbs.SupCalibrateThm.PUReg.Value;
                        _menusEntities.StartMenuCollection[14].StrSetsCollection[2].CVal = _fbs.SupCalibrateThm.IUReg.Value;
                        _menusEntities.StartMenuCollection[14].StrSetsCollection[3].CVal = _fbs.SupCalibrateThm.DUReg.Value;
                        _menusEntities.StartMenuCollection[14].StrSetsCollection[4].CVal = _fbs.SupCalibrateThm.PCalibrates[0];
                        _menusEntities.StartMenuCollection[14].StrSetsCollection[5].CVal = _fbs.SupCalibrateThm.PCalibrates[1];
                        _menusEntities.StartMenuCollection[14].StrSetsCollection[6].CVal = _fbs.SupCalibrateThm.PCalibrates[2];
                        _menusEntities.StartMenuCollection[14].StrSetsCollection[7].CVal = _fbs.SupCalibrateThm.PCalibrates[3];
                        _menusEntities.StartMenuCollection[14].StrSetsCollection[8].CVal = _fbs.SupCalibrateThm.PCalibrates[4];
                        _menusEntities.StartMenuCollection[14].StrSetsCollection[9].CVal = _fbs.SupCalibrateThm.PCalibrates[5];
                        _menusEntities.StartMenuCollection[14].StrSetsCollection[10].CVal = _fbs.SupCalibrateThm.PCalibrates[6];
                        _menusEntities.StartMenuCollection[14].StrSetsCollection[11].CVal = _fbs.ExhaustCalibrateThm.PCalibrates[0];
                        _menusEntities.StartMenuCollection[14].StrSetsCollection[12].CVal = _fbs.ExhaustCalibrateThm.PCalibrates[1];
                        _menusEntities.StartMenuCollection[14].StrSetsCollection[13].CVal = _fbs.ExhaustCalibrateThm.PCalibrates[2];
                        _menusEntities.StartMenuCollection[14].StrSetsCollection[14].CVal = _fbs.ExhaustCalibrateThm.PCalibrates[3];
                        _menusEntities.StartMenuCollection[14].StrSetsCollection[15].CVal = _fbs.ExhaustCalibrateThm.PCalibrates[4];
                        _menusEntities.StartMenuCollection[14].StrSetsCollection[16].CVal = _fbs.ExhaustCalibrateThm.PCalibrates[5];
                        _menusEntities.StartMenuCollection[14].StrSetsCollection[17].CVal = _fbs.ExhaustCalibrateThm.PCalibrates[6];
                        _menusEntities.GenerateInterfaceTable(14);
                    }
                    break;
                case SActivePageState.RecupCurrentPage:
                    {
                        IsThmCalibratePage = false;
                        IsSpecModeSettingsPage = false;
                        IsFBSettingsPage = true;
                        IsMBRecupSettingsPage = false;
                        IsLoadingPage = false;
                        IsConfigPage = false;
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
                        IsFreonSettingsPage = false;
                        IsTmhSettingsPage = false;
                        IsFiltersSettingsPage = false;
                        IsRecupCurrentPage = true;
                        IsTConstThmPage = false;
                        _menusEntities.Title = _menusEntities.StartMenuCollection[15].Name;
                        _menusEntities.StartMenuCollection[15].StrSetsCollection[0].CVal = _fbs.CRecup.RecProfiles[0].I_Start;
                        _menusEntities.StartMenuCollection[15].StrSetsCollection[1].CVal = _fbs.CRecup.RecProfiles[0].I_Cont;
                        _menusEntities.StartMenuCollection[15].StrSetsCollection[2].CVal = _fbs.CRecup.RecProfiles[0].Kp;
                        _menusEntities.StartMenuCollection[15].StrSetsCollection[3].CVal = _fbs.CRecup.RecProfiles[0].Ki;
                        _menusEntities.StartMenuCollection[15].StrSetsCollection[4].CVal = _fbs.CRecup.RecProfiles[1].I_Start;
                        _menusEntities.StartMenuCollection[15].StrSetsCollection[5].CVal = _fbs.CRecup.RecProfiles[1].I_Cont;
                        _menusEntities.StartMenuCollection[15].StrSetsCollection[6].CVal = _fbs.CRecup.RecProfiles[1].Kp;
                        _menusEntities.StartMenuCollection[15].StrSetsCollection[7].CVal = _fbs.CRecup.RecProfiles[1].Ki;
                        _menusEntities.StartMenuCollection[15].StrSetsCollection[8].CVal = _fbs.CRecup.RecProfiles[2].I_Start;
                        _menusEntities.StartMenuCollection[15].StrSetsCollection[9].CVal = _fbs.CRecup.RecProfiles[2].I_Cont;
                        _menusEntities.StartMenuCollection[15].StrSetsCollection[10].CVal = _fbs.CRecup.RecProfiles[2].Kp;
                        _menusEntities.StartMenuCollection[15].StrSetsCollection[11].CVal = _fbs.CRecup.RecProfiles[2].Ki;
                        _menusEntities.StartMenuCollection[15].StrSetsCollection[12].CVal = _fbs.CRecup.RecProfiles[3].I_Start;
                        _menusEntities.StartMenuCollection[15].StrSetsCollection[13].CVal = _fbs.CRecup.RecProfiles[3].I_Cont;
                        _menusEntities.StartMenuCollection[15].StrSetsCollection[14].CVal = _fbs.CRecup.RecProfiles[3].Kp;
                        _menusEntities.StartMenuCollection[15].StrSetsCollection[15].CVal = _fbs.CRecup.RecProfiles[3].Ki;
                        _menusEntities.StartMenuCollection[15].StrSetsCollection[16].CVal = _fbs.CRecup.RecProfiles[4].I_Start;
                        _menusEntities.StartMenuCollection[15].StrSetsCollection[17].CVal = _fbs.CRecup.RecProfiles[4].I_Cont;
                        _menusEntities.StartMenuCollection[15].StrSetsCollection[18].CVal = _fbs.CRecup.RecProfiles[4].Kp;
                        _menusEntities.StartMenuCollection[15].StrSetsCollection[19].CVal = _fbs.CRecup.RecProfiles[4].Ki;
                        _menusEntities.StartMenuCollection[15].StrSetsCollection[20].CVal = _fbs.CRecup.RecProfiles[5].I_Start;
                        _menusEntities.StartMenuCollection[15].StrSetsCollection[21].CVal = _fbs.CRecup.RecProfiles[5].I_Cont;
                        _menusEntities.StartMenuCollection[15].StrSetsCollection[22].CVal = _fbs.CRecup.RecProfiles[5].Kp;
                        _menusEntities.StartMenuCollection[15].StrSetsCollection[23].CVal = _fbs.CRecup.RecProfiles[5].Ki;
                        _menusEntities.StartMenuCollection[15].StrSetsCollection[24].CVal = _fbs.CRecup.RecProfiles[6].I_Start;
                        _menusEntities.StartMenuCollection[15].StrSetsCollection[25].CVal = _fbs.CRecup.RecProfiles[6].I_Cont;
                        _menusEntities.StartMenuCollection[15].StrSetsCollection[26].CVal = _fbs.CRecup.RecProfiles[6].Kp;
                        _menusEntities.StartMenuCollection[15].StrSetsCollection[27].CVal = _fbs.CRecup.RecProfiles[6].Ki;
                        _menusEntities.StartMenuCollection[15].StrSetsCollection[28].CVal = _fbs.CRecup.RecProfiles[7].I_Start;
                        _menusEntities.StartMenuCollection[15].StrSetsCollection[29].CVal = _fbs.CRecup.RecProfiles[7].I_Cont;
                        _menusEntities.StartMenuCollection[15].StrSetsCollection[30].CVal = _fbs.CRecup.RecProfiles[7].Kp;
                        _menusEntities.StartMenuCollection[15].StrSetsCollection[31].CVal = _fbs.CRecup.RecProfiles[7].Ki;
                        _menusEntities.StartMenuCollection[15].StrSetsCollection[32].CVal = _fbs.CRecup.RecProfiles[8].I_Start;
                        _menusEntities.StartMenuCollection[15].StrSetsCollection[33].CVal = _fbs.CRecup.RecProfiles[8].I_Cont;
                        _menusEntities.StartMenuCollection[15].StrSetsCollection[34].CVal = _fbs.CRecup.RecProfiles[8].Kp;
                        _menusEntities.StartMenuCollection[15].StrSetsCollection[35].CVal = _fbs.CRecup.RecProfiles[8].Ki;
                        _menusEntities.StartMenuCollection[15].StrSetsCollection[36].CVal = _fbs.CRecup.RecProfiles[9].I_Start;
                        _menusEntities.StartMenuCollection[15].StrSetsCollection[37].CVal = _fbs.CRecup.RecProfiles[9].I_Cont;
                        _menusEntities.StartMenuCollection[15].StrSetsCollection[38].CVal = _fbs.CRecup.RecProfiles[9].Kp;
                        _menusEntities.StartMenuCollection[15].StrSetsCollection[39].CVal = _fbs.CRecup.RecProfiles[9].Ki;
                        _menusEntities.GenerateInterfaceTable(15);
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
