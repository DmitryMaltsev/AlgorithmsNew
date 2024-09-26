using Android_Silver.Entities.FBEntities;
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
        ConfigPage,
        LoadingPage
    }


    public class ServiceActivePagesEntities : BindableBase
    {
        MenusEntities _menusEntities;
        FBs _fbs;

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
            set {
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
        #endregion

        public ServiceActivePagesEntities()
        {
            _menusEntities = DIContainer.Resolve<MenusEntities>();
            _fbs = DIContainer.Resolve<FBs>();
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
                        _menusEntities.Title = _menusEntities.StartMenuCollection[1].Name;
                        // _menuEntities.
                        _menusEntities.StartMenuCollection[1].StrSetsCollection[0].CVal = _fbs.CCommonSetPoints.SPTempAlarm;
                        _menusEntities.StartMenuCollection[1].StrSetsCollection[1].CPickVal = _fbs.CEConfig.TregularCh_R;
                        _menusEntities.StartMenuCollection[1].StrSetsCollection[2].CVal = _fbs.CCommonSetPoints.SPTempMaxCh;
                        _menusEntities.StartMenuCollection[1].StrSetsCollection[3].CVal = _fbs.CCommonSetPoints.SPTempMinCh;
                        _menusEntities.StartMenuCollection[1].StrSetsCollection[4].CVal = _fbs.CCommonSetPoints.TControlDelayS;
                        _menusEntities.StartMenuCollection[1].StrSetsCollection[5].CVal = _fbs.CCommonSetPoints.SeasonMode;
                        _menusEntities.StartMenuCollection[1].StrSetsCollection[6].CVal = _fbs.CCommonSetPoints.SPSeason;
                        _menusEntities.StartMenuCollection[1].StrSetsCollection[7].CVal = _fbs.CCommonSetPoints.HystSeason;
                        _menusEntities.StartMenuCollection[1].StrSetsCollection[8].CPickVal = _fbs.CEConfig.AutoResetFire;
                        _menusEntities.StartMenuCollection[1].StrSetsCollection[9].CPickVal = _fbs.CEConfig.AutoRestart;
                        _menusEntities.GenerateInterfaceTable(1);
                    }
                    break;
                case SActivePageState.DamperSettingsPage:
                    {
                        _menusEntities.Title = _menusEntities.StartMenuCollection[1].Name;
                        _menusEntities.StartMenuCollection[2].StrSetsCollection[0].CVal = _fbs.CDamperSetPoints.DamperOpenTime;
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
                    }
                    break;
                case SActivePageState.FanSettingsPage:
                    {
                        _menusEntities.Title = _menusEntities.StartMenuCollection[3].Name;
                        _menusEntities.StartMenuCollection[3].StrSetsCollection[0].CVal = _fbs.CFans.SFanNominalFlow;
                        _menusEntities.StartMenuCollection[3].StrSetsCollection[1].CVal = _fbs.CFans.EFanNominalFlow;
                        _menusEntities.StartMenuCollection[3].StrSetsCollection[2].CVal = _fbs.CFans.Speed0v;
                        _menusEntities.StartMenuCollection[3].StrSetsCollection[3].CVal = _fbs.CFans.Speed10v;
                        _menusEntities.StartMenuCollection[3].StrSetsCollection[4].CVal = _fbs.CFans.PressureFailureDelay;
                        _menusEntities.StartMenuCollection[3].StrSetsCollection[5].CVal = _fbs.CFans.FanFailureDelay;
                        _menusEntities.StartMenuCollection[3].StrSetsCollection[6].CVal = _fbs.CFans.DecrFanConfig;
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
                    }
                    break;
                case SActivePageState.FreonSettingsPage:
                    {
                        _menusEntities.Title = _menusEntities.StartMenuCollection[6].Name;
                        _menusEntities.StartMenuCollection[6].StrSetsCollection[0].CVal = _fbs.CFreonCoolerSP.PReg;
                        _menusEntities.StartMenuCollection[6].StrSetsCollection[1].CVal = _fbs.CFreonCoolerSP.IReg;
                        _menusEntities.StartMenuCollection[6].StrSetsCollection[2].CVal = _fbs.CFreonCoolerSP.DReg;
                        _menusEntities.StartMenuCollection[6].StrSetsCollection[3].CVal = _fbs.CFreonCoolerSP.Stage1OffS;
                        _menusEntities.StartMenuCollection[6].StrSetsCollection[4].CVal = _fbs.CFreonCoolerSP.Stage1OnS;
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
                    }
                    break;
                case SActivePageState.RecupSettingsPage:
                    {
                        _menusEntities.Title = _menusEntities.StartMenuCollection[7].Name;
                        _menusEntities.StartMenuCollection[7].StrSetsCollection[0].CVal = _fbs.CRecup.PReg;
                        _menusEntities.StartMenuCollection[7].StrSetsCollection[1].CVal = _fbs.CRecup.IReg;
                        _menusEntities.StartMenuCollection[7].StrSetsCollection[2].CVal = _fbs.CRecup.DReg;
                        _menusEntities.StartMenuCollection[7].StrSetsCollection[3].CVal = _fbs.CRecup.EffSP;
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
                    }
                    break;
                case SActivePageState.HumSettingsPage:
                    {
                        _menusEntities.Title = _menusEntities.StartMenuCollection[8].Name;
                        _menusEntities.StartMenuCollection[8].StrSetsCollection[0].CVal = _fbs.CHumiditySPS.PReg;
                        _menusEntities.StartMenuCollection[8].StrSetsCollection[1].CVal = _fbs.CHumiditySPS.IReg;
                        _menusEntities.StartMenuCollection[8].StrSetsCollection[2].CVal = _fbs.CHumiditySPS.DReg;
                        _menusEntities.StartMenuCollection[8].StrSetsCollection[3].CVal = _fbs.CHumiditySPS.Stage1OffS;
                        _menusEntities.StartMenuCollection[8].StrSetsCollection[4].CVal = _fbs.CHumiditySPS.Stage1OnS;
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
                        _menusEntities.Title = _menusEntities.StartMenuCollection[10].Name;
                        _menusEntities.StartMenuCollection[10].StrSetsCollection[0].CVal = _fbs.ThmSps.SupTHmKoef;
                        _menusEntities.StartMenuCollection[10].StrSetsCollection[1].CVal = _fbs.ThmSps.SupCurveKoef;
                        _menusEntities.StartMenuCollection[10].StrSetsCollection[2].CVal = _fbs.ThmSps.ExhaustTHmKoef;
                        _menusEntities.StartMenuCollection[10].StrSetsCollection[3].CVal = _fbs.ThmSps.ExhaustCurveKoef;
                        _menusEntities.StartMenuCollection[10].StrSetsCollection[4].CVal = _fbs.ThmSps.PReg;
                        _menusEntities.StartMenuCollection[10].StrSetsCollection[5].CVal = _fbs.ThmSps.IReg;
                        _menusEntities.StartMenuCollection[10].StrSetsCollection[6].CVal = _fbs.ThmSps.DReg;
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
