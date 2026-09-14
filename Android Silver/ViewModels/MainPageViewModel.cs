using Android_Silver.Entities;
using Android_Silver.Entities.FBEntities;
using Android_Silver.Entities.Modes;
using Android_Silver.Entities.Visual;
using Android_Silver.Entities.Visual.Menus;
using Android_Silver.Services;
using Android_Silver.ViewModels;
using System.Timers;
using System;
using System.Globalization;
using System.Windows.Input;
using Timer = System.Timers.Timer;

namespace Android_Silver.Pages
{
    public class MainPageViewModel : BindableBase
    {
        #region Rising properties
        private int _value;
        public int Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged(nameof(Value));
            }
        }

        private string _messageToClient;
        public string MessageToClient
        {
            get { return _messageToClient; }
            set
            {
                _messageToClient = value;
                OnPropertyChanged(nameof(MessageToClient));
            }
        }

        private string _sendMessageToClient;
        public string SendMessageToClient
        {
            get { return _sendMessageToClient; }
            set
            {
                _sendMessageToClient = value;
                OnPropertyChanged(nameof(SendMessageToClient));
            }
        }

        private bool _sendIsActive = false;
        public bool SendIsActive
        {
            get { return _sendIsActive; }
            set
            {
                _sendIsActive = value;
                OnPropertyChanged(nameof(SendIsActive));
            }
        }

        private bool _connected = false;
        public bool Connected
        {
            get { return _connected; }
            set
            {
                _connected = value;
                OnPropertyChanged(nameof(Connected));
            }
        }


        //Текущее значение M1, отбраженное на экране
        private Mode1Values _m1Values;
        public Mode1Values M1Values
        {
            get { return _m1Values; }

            set
            {
                _m1Values = value;
                OnPropertyChanged(nameof(M1Values));
            }
        }

        /// <summary>
        /// Промежуточное значение режима времени для настройки в
        /// соответствующем окне.
        /// </summary>
        private TimeModeValues _tValues;
        public TimeModeValues TValues
        {
            get { return _tValues; }
            set
            {
                _tValues = value;
                OnPropertyChanged(nameof(TValues));
            }
        }

        private int _humiditySP;

        public int HumiditySP
        {
            get { return _humiditySP; }
            set
            {
                _humiditySP = value;
                OnPropertyChanged(nameof(HumiditySP));
            }
        }

        private Time _timeBuffer;
        public Time TimeBuffer
        {
            get { return _timeBuffer; }
            set
            {
                _timeBuffer = value;
                OnPropertyChanged(nameof(TimeBuffer));
            }
        }

        private Mode1Values _contactMode1Buf;
        public Mode1Values ContactMode1Buf
        {
            get { return _contactMode1Buf; }
            set
            {
                _contactMode1Buf = value;
                OnPropertyChanged(nameof(ContactMode1Buf));
            }
        }



        private OtherSettings _cOtherSettings = new OtherSettings();

        public OtherSettings COtherSettings
        {
            get { return _cOtherSettings; }
            set
            {
                _cOtherSettings = value;
                OnPropertyChanged(nameof(COtherSettings));
            }
        }

        #endregion

        #region Commands
        public ICommand TestCommand { get; private set; }
        public ICommand StartPageConnectCommand { get; private set; }
        #region MainPageCommands

        #region Loading command
        public ICommand LoadingReturnCommand { get; private set; }
        #endregion

        public ICommand ConnectCommand { get; private set; }
        public ICommand DisconnectCommand { get; private set; }
        public ICommand GetIPCommand { get; private set; }
        public ICommand SettingsCommand { get; private set; }
        public ICommand ChooseModeCommand { get; private set; }
        public ICommand GoToPageCommand { get; private set; }
        public ICommand SetTDataCommand { get; private set; }
        #endregion
        #region Choose mode page commands
        public ICommand MinModeCommand { get; private set; }
        public ICommand NormalModeCommand { get; private set; }
        public ICommand MaxModeCommand { get; private set; }
        public ICommand KitchenModeCommand { get; private set; }
        public ICommand IsShedulerCommand { get; private set; }
        public ICommand VacationModeCommand { get; private set; }
        public ICommand TurnOffModeCommand { get; private set; }
        public ICommand HomeCommand { get; private set; }
        #endregion
        #region KitchenTimerCommands
        public ICommand UpMInutesCommand { get; private set; }
        public ICommand DnMinutesCommand { get; private set; }
        public ICommand KitchenOkCommand { get; private set; }
        public ICommand KitchenCancelCommand { get; private set; }
        #endregion
        #region SPCommands
        public ICommand NextSetPointsCommand { get; private set; }
        public ICommand PrevSetPointsCommand { get; private set; }
        public ICommand SPReturnCommand { get; private set; }
        public ICommand SPOkCommand { get; private set; }
        public ICommand SPAdd0Command { get; private set; }
        public ICommand SPSub0Command { get; private set; }
        public ICommand SPAdd1Command { get; private set; }
        public ICommand SPSub1Command { get; private set; }
        public ICommand SPAdd2Command { get; private set; }
        public ICommand SPSub2Command { get; private set; }
        public ICommand SPSub3Command { get; private set; }
        public ICommand SPAdd3Command { get; private set; }
        public ICommand SPSub4Command { get; private set; }
        public ICommand SPAdd4Command { get; private set; }
        #endregion
        #region SPTimers
        public Timer SubBut0Timer;
        public Timer AddBut0Timer;
        public Timer SubBut1Timer;
        public Timer AddBut1Timer;
        public Timer SubBut2Timer;
        public Timer AddBut2Timer;
        public Timer SubBut3Timer;
        public Timer AddBut3Timer;
        public Timer SubBut4Timer;
        public Timer AddBut4Timer;
        #endregion
        #region Settings commands
        public ICommand OtherSettingsCommand { get; private set; }
        public ICommand JournalCommand { get; private set; }
        public ICommand ShedulerTableCommand { get; private set; }
        public ICommand SetSPCommand { get; private set; }

        public ICommand SetCorrSPCommand { get; private set; }
        public ICommand InformationCommand { get; private set; }
        public ICommand HelperCommand { get; private set; }
        #endregion
        #region TSettingsCommands
        public ICommand TRetCommand { get; private set; }
        #endregion
        #region SetTSettingsCommands
        public ICommand TSetReturnCommand { get; private set; }
        public ICommand TSetOkCommand { get; private set; }
        public ICommand TSetBtnLeftCommand0 { get; private set; }
        public ICommand TSetBtnRightCommand0 { get; private set; }
        public ICommand TSetBtnLeftCommand1 { get; private set; }
        public ICommand TSetBtnRightCommand1 { get; private set; }
        public ICommand TSetBtnLeftCommand2 { get; private set; }
        public ICommand TSetBtnRightCommand2 { get; private set; }
        public ICommand TSetBtnLeftCommand3 { get; private set; }
        public ICommand TSetBtnRightCommand3 { get; private set; }
        #endregion
        #region Other settings commands
        public ICommand OtherSettingsReturnCommand { get; private set; }
        public ICommand SetOtherSettingsCommand { get; private set; }
        public ICommand ContactArrLeftCommand { get; private set; }
        public ICommand ContactArrRightCommand { get; private set; }
        public ICommand HumidityCommand { get; private set; }
        public ICommand SetTimeCommand { get; private set; }
        public ICommand IsSpecModeCommand { get; private set; }
        public ICommand ChangeFilterCommand { get; private set; }
        public ICommand BootloaderSetCommand { get; private set; }
        #endregion
        #region Humidity commands
        public ICommand OkHumidityCommand { get; private set; }
        public ICommand CancelHumidityCommand { get; private set; }
        public ICommand HumidityBtnUpCommand { get; private set; }
        public ICommand HumidityBtnDnCommand { get; private set; }
        #endregion
        #region ResetCommand
        public ICommand ResetJournalCommand { get; private set; }
        #endregion
        #region MyRegion
        public ICommand JournalReturnCommand { get; private set; }

        #endregion
        #region Time command
        public ICommand TimeReturnCommand { get; private set; }
        public ICommand TimeAdd0Command { get; private set; }
        public ICommand TimeSub0Command { get; private set; }
        public ICommand TimeAdd1Command { get; private set; }
        public ICommand TimeSub1Command { get; private set; }
        public ICommand TimeAdd2Command { get; private set; }
        public ICommand TimeSub2Command { get; private set; }
        public ICommand TimeAdd3Command { get; private set; }
        public ICommand TimeSub3Command { get; private set; }
        public ICommand TimeAdd4Command { get; private set; }
        public ICommand TimeSub4Command { get; private set; }
        public ICommand TimeOkCommand { get; private set; }
        #endregion
        #region BootloaderCommand
        public ICommand UpdateCommand { get; private set; }
        public ICommand DownloadCommand { get; private set; }
        public ICommand ResetCommand { get; private set; }
        public ICommand BootloaderBackCommand { get; private set; }
        #endregion
        // public ICommand SettingsCommand { get; private set; }
        #endregion

        public EthernetEntities EthernetEntities { get; set; }

        public TcpClientService CTcpClientService { get; set; }

        public ModesEntities CModesEntities { get; set; }

        public PicturesSet CPictureSet { get; set; }

        public ActivePagesEntities CActivePagesEntities { get; set; }

        private FileSystemService _fileSystemService { get; set; }

        public FBs CFBs { get; set; }

        private MenusEntities _menuesEntities { get; set; }

        private MathService _mathService { get; set; }

        public FilesEntities CFilesEntities { get; set; }

        private IDispatcherTimer _fileResultTimer { get; set; }
        public MainPageViewModel()
        {
            EthernetEntities = DIContainer.Resolve<EthernetEntities>();
            CTcpClientService = DIContainer.Resolve<TcpClientService>();
            CModesEntities = DIContainer.Resolve<ModesEntities>();
            CActivePagesEntities = DIContainer.Resolve<ActivePagesEntities>();
            CPictureSet = DIContainer.Resolve<PicturesSet>();
            CFBs = DIContainer.Resolve<FBs>();
            CFilesEntities = DIContainer.Resolve<FilesEntities>();
            _fileSystemService = DIContainer.Resolve<FileSystemService>();
            _menuesEntities = DIContainer.Resolve<MenusEntities>();
            _mathService = DIContainer.Resolve<MathService>();
            //CFBs.CUpdater.FWVerWord
            // CFBs.CUpdater.AutoUpdList
            //CFBs.CUpdater.AutoUpdIndex
#if ANDROID
            AndroidEntity.WifiStateChanged -= EthernetEntities.WifiStateChangeCallback;
            AndroidEntity.WifiStateChanged += EthernetEntities.WifiStateChangeCallback;
#endif
            StartPageConnectCommand = new Command(ExecuteConnect);
            ConnectCommand = new Command(ExecuteConnect);
            DisconnectCommand = new Command(ExecuteDisconnect);
            SetSPCommand = new Command(ExecuteSetSP);
            SetCorrSPCommand = new Command(ExecuteSetCorrSP);
            ChooseModeCommand = new Command(ExecuteChooseMode);
            SettingsCommand = new Command(ExecuiteSettings);
            MinModeCommand = new Command(ExecuteMinMode);
            NormalModeCommand = new Command(ExecuteNormal);
            MaxModeCommand = new Command(ExecuteMaxMode);
            KitchenModeCommand = new Command(ExecuteKitchenMode);
            IsShedulerCommand = new Command(ExecuteIsSheduler);
            VacationModeCommand = new Command(ExecuteVacationMode);
            TurnOffModeCommand = new Command(ExecuteTurnOffMode);
            JournalCommand = new Command(ExecuteJournal);
            HelperCommand = new Command(ExecuteHelper);
            ShedulerTableCommand = new Command(ExecuteShedulerTable);
            HomeCommand = new Command(ExecuteHomeCommand);
            ResetJournalCommand = new Command(ExecuteResetJournal);
            SetOtherSettingsCommand = new Command(ExecuteSetOtherSettings);
            JournalReturnCommand = new Command(ExecuteJournalReturn);
            ContactArrLeftCommand = new Command(ExecuteContactArrLeft);
            ContactArrRightCommand = new Command(ExecuteContactArrRight);
            TimeReturnCommand = new Command(ExecuteTimeReturn);
            ChangeFilterCommand = new Command(ExecuteChangeFilter);
            UpdateCommand = new Command(ExecuteUpdate);
            DownloadCommand = new Command(ExecuteDownload);
            ResetCommand = new Command(ExecuteReset);
            BootloaderSetCommand = new Command(ExecuteBootloaderSet);
            BootloaderBackCommand = new Command(ExecuteBootLoaderBack);
            TimeBuffer = new();
            Value = 15;
            #region Kitchen timer commands
            UpMInutesCommand = new Command(ExecuteUpMinutes);
            DnMinutesCommand = new Command(ExecuteDnMinutes);
            KitchenOkCommand = new Command(ExecuteKitchenOk);
            KitchenCancelCommand = new Command(ExecuteKitchenCancel);
            #endregion
            #region Loading commands
            LoadingReturnCommand = new Command(ExecuteLoadingReturn);

            #endregion
            #region Set points commands
            NextSetPointsCommand = new Command(ExecuteNextSetPoints);
            PrevSetPointsCommand = new Command(ExecutePrevSetPoints);
            SPAdd0Command = new Command(ExecuteSPAdd0);
            SPSub0Command = new Command(ExecuteSPSub0);
            SPAdd1Command = new Command(ExecuteSPAdd1);
            SPSub1Command = new Command(ExecuteSPSub1);
            SPAdd2Command = new Command(ExecuteSPAdd2);
            SPSub2Command = new Command(ExecuteSPSub2);
            SPAdd3Command = new Command(ExecuteSPAdd3);
            SPSub3Command = new Command(ExecuteSPSub3);
            SPAdd4Command = new Command(ExecuteSPAdd4);
            SPSub4Command = new Command(ExecuteSPSub4);
            SPOkCommand = new Command(ExecuteSPSOK);
            SPReturnCommand = new Command(ExecuteSPReturn);
            #endregion
            #region Set points timers
            AddBut0Timer = new Timer(150);
            AddBut0Timer.Elapsed -= AddBut0Timer_Elapsed;
            AddBut0Timer.Elapsed += AddBut0Timer_Elapsed;
            SubBut0Timer = new Timer(150);
            SubBut0Timer.Elapsed -= SubBut0Timer_Elapsed;
            SubBut0Timer.Elapsed += SubBut0Timer_Elapsed;
            AddBut1Timer = new Timer(150);
            AddBut1Timer.Elapsed -= AddBut1Timer_Elapsed;
            AddBut1Timer.Elapsed += AddBut1Timer_Elapsed;
            SubBut1Timer = new Timer(150);
            SubBut1Timer.Elapsed -= SubBut1Timer_Elapsed;
            SubBut1Timer.Elapsed += SubBut1Timer_Elapsed;
            AddBut2Timer = new Timer(150);
            AddBut2Timer.Elapsed -= AddBut2Timer_Elapsed;
            AddBut2Timer.Elapsed += AddBut2Timer_Elapsed;
            SubBut2Timer = new Timer(150);
            SubBut2Timer.Elapsed -= SubBut2Timer_Elapsed;
            SubBut2Timer.Elapsed += SubBut2Timer_Elapsed;
            AddBut3Timer = new Timer(150);
            AddBut3Timer.Elapsed -= AddBut3Timer_Elapsed;
            AddBut3Timer.Elapsed += AddBut3Timer_Elapsed;
            SubBut3Timer = new Timer(150);
            SubBut3Timer.Elapsed -= SubBut3Timer_Elapsed;
            SubBut3Timer.Elapsed += SubBut3Timer_Elapsed;
            AddBut4Timer = new Timer(150);
            AddBut4Timer.Elapsed -= AddBut4Timer_Elapsed;
            AddBut4Timer.Elapsed += AddBut4Timer_Elapsed;
            SubBut4Timer = new Timer(150);
            SubBut4Timer.Elapsed -= SubBut4Timer_Elapsed;
            SubBut4Timer.Elapsed += SubBut4Timer_Elapsed;

            #endregion
            #region Vac commands
            SetTDataCommand = new Command(ExecuteSetTData);
            #endregion
            #region TCommands
            TRetCommand = new Command(ExecuteTRet);
            #endregion
            #region TSet commands
            TSetOkCommand = new Command(TSetExecuteOK);
            TSetBtnLeftCommand0 = new Command(TSetExecuteBtnLeft0);
            TSetBtnRightCommand0 = new Command(TSetExecuteBtnRight0);
            TSetBtnLeftCommand1 = new Command(TSetExecuteBtnLeft1);
            TSetBtnRightCommand1 = new Command(TSetExecuteBtnRight1);
            TSetBtnLeftCommand2 = new Command(TSetExecuteBtnLeft2);
            TSetBtnRightCommand2 = new Command(TSetExecuteBtnRight2);
            TSetBtnLeftCommand3 = new Command(TSetExecuteBtnLeft3);
            TSetBtnRightCommand3 = new Command(TSetExecuteBtnRight3);
            TSetReturnCommand = new Command(TSetExecuteReturn);
            #endregion
            #region Settings commands
            OtherSettingsCommand = new Command(ExecuteOtherSettings);
            InformationCommand = new Command(ExecuteInformation);
            #endregion
            #region Other settings commands
            OtherSettingsReturnCommand = new Command(ExecuteOtherSettingsReturn);
            SetTimeCommand = new Command(ExecuteSetTime);
            HumidityBtnUpCommand = new Command(ExecuteHumidityBtnUp);
            HumidityBtnDnCommand = new Command(ExecuteHumidityBtnDn);
            IsSpecModeCommand = new Command(ExecuteIsSpecMode);
            #endregion
            #region Humidity commands
            OkHumidityCommand = new Command(ExecuteOkHumidity);
            CancelHumidityCommand = new Command(CancelHumidity);
            #endregion
            #region Time commands
            TimeAdd0Command = new Command(ExecuteTimeAdd0);
            TimeSub0Command = new Command(ExecuteTimeSub0);
            TimeAdd1Command = new Command(ExecuteTimeAdd1);
            TimeSub1Command = new Command(ExecuteTimeSub1);
            TimeAdd2Command = new Command(ExecuteTimeAdd2);
            TimeSub2Command = new Command(ExecuteTimeSub2);
            TimeAdd3Command = new Command(ExecuteTimeAdd3);
            TimeSub3Command = new Command(ExecuteTimeSub3);
            TimeAdd4Command = new Command(ExecuteTimeAdd4);
            TimeSub4Command = new Command(ExecuteTimeSub4);
            TimeOkCommand = new Command(ExecuteTimeOk);
            #endregion
            _fileSystemService.GetIPFromFile();
            SetTValuesByIndex(0, 0);//?????
            CTcpClientService.ClientDisconnected -= ClientDisceonnectedCallback;
            CTcpClientService.ClientDisconnected += ClientDisceonnectedCallback; ;
        }

        async private void ExecuteConnect()
        {

            EthernetEntities.ConnectIP =
                 $"{EthernetEntities.IP1}.{EthernetEntities.IP2}.{EthernetEntities.IP3}.{EthernetEntities.IP4}";
            EthernetEntities.SystemMessage = "Check";
            if (!CTcpClientService.IsConnecting)
            {
                await CTcpClientService.Connect();
                if (EthernetEntities.IsConnected)
                {
                    CModesEntities.ShedCountQueues = 0;
                    CActivePagesEntities.SetActivePageState(ActivePageState.MainPage);
                    CPictureSet.SetPicureSetIfNeed(CPictureSet.LinkHeader, CPictureSet.LinkHeader.Selected);
                    await _fileSystemService.SaveToFileAsync("ConnectIP", EthernetEntities.ConnectIP);
                    CTcpClientService.SendRecieveTask();
                    // TcpClientService.SendRecieveTask("137,4");ё
                }
                else
                {
                    CActivePagesEntities.SetActivePageState(ActivePageState.StartPage);
                }
            }
            else
            {
                CPictureSet.SetPicureSetIfNeed(CPictureSet.LinkHeader, CPictureSet.LinkHeader.Default);
                EthernetEntities.SystemMessage = "В данный момент подключаемся";
            }
        }

        #region Main page execute methods
        public void SetActivePageIfNeed()
        {
            if (!EthernetEntities.IsConnected)
            {
                CActivePagesEntities.SetActivePageState(ActivePageState.StartPage);
            }
            else
            {
                CActivePagesEntities.SetActivePageState(ActivePageState.MainPage);
            }
            //CActivePagesEntities.SetActivePageState(CActivePagesEntities.LastActivePageState);
        }
        void ExecuteSetSP(object obj)
        {
            SetM1ValuesByIndex(CModesEntities.CMode1.Num);
            CActivePagesEntities.SetActivePageState(ActivePageState.SetPointsPage);
        }

        private void ExecuteDisconnect()
        {
            CTcpClientService.Disconnect();
            CPictureSet.SetPicureSetIfNeed(CPictureSet.LinkHeader, CPictureSet.LinkHeader.Default);
        }


        private void ExecuiteSettings(object obj)
        {
            // CPictureSet.AlarmMainIcon.Current
            CActivePagesEntities.SetActivePageState(ActivePageState.SettingsPage);
        }

        private void ExecuteChooseMode(object obj)
        {
            if (CModesEntities.CMode1.Num == 7)
            {
                CActivePagesEntities.SetActivePageState(ActivePageState.JournalPage);
            }
            else
            {
                CActivePagesEntities.SetActivePageState(ActivePageState.ChooseModePage);
                COtherSettings.IsScheduler = CFBs.OtherSettings.IsScheduler;
                CPictureSet.SelectModesPics[6].Current = COtherSettings.IsScheduler ? CPictureSet.SelectModesPics[6].Selected : CPictureSet.SelectModesPics[6].Default;
            }
        }
        #endregion

        #region ExecuteSetModes

        private void ExecuteTurnOffMode(object obj)
        {
            int[] index = { 0 };
            CTcpClientService.SetCommandToServer(19, index);
            CActivePagesEntities.SetActivePageState(ActivePageState.MainPage);
        }
        private void ExecuteMinMode(object obj)
        {
            int[] index = { 1 };
            CTcpClientService.SetCommandToServer(19, index);
            CActivePagesEntities.SetActivePageState(ActivePageState.MainPage);
        }

        private void ExecuteNormal(object obj)
        {
            int[] index = { 2 };
            CTcpClientService.SetCommandToServer(19, index);
            CActivePagesEntities.SetActivePageState(ActivePageState.MainPage);
        }

        private void ExecuteMaxMode(object obj)
        {
            int[] index = { 3 };
            CTcpClientService.SetCommandToServer(19, index);
            CActivePagesEntities.SetActivePageState(ActivePageState.MainPage);
        }

        private void ExecuteKitchenMode(object obj)
        {
            int[] index = { 4 };
            CTcpClientService.SetCommandToServer(19, index);
            CActivePagesEntities.SetActivePageState(ActivePageState.MainPage);
        }

        private void ExecuteVacationMode(object obj)
        {
            int[] index = { 5 };
            CTcpClientService.SetCommandToServer(19, index);
            CActivePagesEntities.SetActivePageState(ActivePageState.MainPage);
        }

        private void ExecuteIsSheduler(object obj)
        {
            COtherSettings.IsScheduler = !CFBs.OtherSettings.IsScheduler;
            CPictureSet.SelectModesPics[6].Current = COtherSettings.IsScheduler ? CPictureSet.SelectModesPics[6].Selected : CPictureSet.SelectModesPics[6].Default;
            int indexBuf = COtherSettings.IsScheduler ? 1 : 0;
            int[] index = { indexBuf };
            CTcpClientService.SetCommandToServer(24, index);
            // CActivePagesEntities.SetActivePageState(ActivePageState.MainPage);
        }

        void ExecuteHomeCommand(object obj)
        {
            CActivePagesEntities.SetActivePageState(ActivePageState.MainPage);
        }
        #endregion

        #region Kithen timer execute commands

        private void ExecuteDnMinutes(object obj)
        {

            if (CModesEntities.Mode2ValuesList[1].TimeModeValues[0].Minute >= 10)
            {
                CModesEntities.Mode2ValuesList[1].TimeModeValues[0].Minute -= 10;
            }
            else
            {
                CModesEntities.Mode2ValuesList[1].TimeModeValues[0].Minute = 0;
            }
        }

        private void ExecuteUpMinutes(object obj)
        {
            if (CModesEntities.Mode2ValuesList[1].TimeModeValues[0].Minute <= 500)
            {
                CModesEntities.Mode2ValuesList[1].TimeModeValues[0].Minute += 10;
            }
            else
            {
                CModesEntities.Mode2ValuesList[1].TimeModeValues[0].Minute = 500;
            }
        }

        private void ExecuteKitchenCancel(object obj)
        {
            int[] values = { CModesEntities.Mode2ValuesList[1].TimeModeValues[0].Minute, 0, 1 };
            CTcpClientService.SetCommandToServer(144, values);
            CActivePagesEntities.SetActivePageState(ActivePageState.MainPage); ;
        }

        private void ExecuteKitchenOk(object obj)
        {
            int[] values = { CModesEntities.Mode2ValuesList[1].TimeModeValues[0].Minute, 1, 0 };
            CTcpClientService.SetCommandToServer(144, values);
            CActivePagesEntities.SetActivePageState(ActivePageState.MainPage);
        }
        #endregion

        #region SetPoints execute commands
        private void ExecuteSPAdd0(object obj)
        {
            if (M1Values != null)
                M1Values.SupplySP.Value = M1Values.SupplySP.Value + 1 < 100 ? M1Values.SupplySP.Value + 1 : 100;
        }
        private void ExecuteSPSub0(object obj)
        {
            if (M1Values != null)
                M1Values.SupplySP.Value = M1Values.SupplySP.Value - 1 > 20 ? M1Values.SupplySP.Value - 1 : 20;
        }

        private void ExecuteSPAdd1(object obj)
        {
            if (M1Values != null)
                M1Values.ExhaustDisb.Value = M1Values.ExhaustDisb.Value + 1 < 30 ? M1Values.ExhaustDisb.Value + 1 : 30;
        }
        private void ExecuteSPSub1(object obj)
        {
            if (M1Values != null)
                M1Values.ExhaustDisb.Value = M1Values.ExhaustDisb.Value - 1 > -30 ? M1Values.ExhaustDisb.Value - 1 : -30;
        }

        private void ExecuteSPAdd2(object obj)
        {
            if (M1Values != null)
            {
                M1Values.TempSP.Value = M1Values.TempSP.Value + 1 < 30 ? M1Values.TempSP.Value + 1 : 30;
                if (M1Values.TempSP.Value < 16) M1Values.TempSP.Value = 16;

            }
        }
        private void ExecuteSPSub2(object obj)
        {
            if (M1Values != null)
                M1Values.TempSP.Value = M1Values.TempSP.Value - 1 > 16 ? M1Values.TempSP.Value - 1 : 16;
        }

        private void ExecuteSPSub3(object obj)
        {
            if (M1Values != null)
                M1Values.SFanCorr.Value = M1Values.SFanCorr.Value - 1 > M1Values.SFanCorr.Min ? M1Values.SFanCorr.Value - 1 : M1Values.SFanCorr.Min;
        }
        private void ExecuteSPAdd3(object obj)
        {
            if (M1Values != null)
                M1Values.SFanCorr.Value = M1Values.SFanCorr.Value + 1 < M1Values.SFanCorr.Max ? M1Values.SFanCorr.Value + 1 : M1Values.SFanCorr.Max;
        }
        private void ExecuteSPSub4(object obj)
        {
            if (M1Values != null)
                M1Values.EFanCorr.Value = M1Values.EFanCorr.Value - 1 > M1Values.EFanCorr.Min ? M1Values.EFanCorr.Value - 1 : M1Values.EFanCorr.Min;
        }
        private void ExecuteSPAdd4(object obj)
        {
            if (M1Values != null)
                M1Values.EFanCorr.Value = M1Values.EFanCorr.Value + 1 < M1Values.EFanCorr.Max ? M1Values.EFanCorr.Value + 1 : M1Values.EFanCorr.Max;
        }

        private void ExecuteSPSOK(object obj)
        {
            if (M1Values != null)
            {
                int[] values = { M1Values.SupplySP.Value, M1Values.ExhaustDisb.Value, (int)M1Values.TempSP.Value,
                    M1Values.ThreshPerc.Value, M1Values.SFanCorr.Value, M1Values.EFanCorr.Value };
                CTcpClientService.SetCommandToServer(M1Values.StartAddress, values);
                // CActivePagesEntities.SetActivePageState(ActivePageState.SettingsPage);
            }
        }

        private void ExecuteNextSetPoints(object obj)
        {
            int ind = 0;
            if (M1Values.Num == 3)
            {
                ind = 1;
            }
            else
            if (M1Values.Num < 3)
            {
                ind = M1Values.Num + 1;
            }
            SetM1ValuesByIndex(ind);
        }

        private void ExecutePrevSetPoints(object obj)
        {
            int ind = 0;
            if (M1Values.Num == 1)
            {
                ind = 3;
            }
            else
            if (M1Values.Num > 0)
            {
                ind = M1Values.Num - 1;
            }
            SetM1ValuesByIndex(ind);
        }

        private void SetM1ValuesByIndex(int index)
        {

            index = index > 0 && index < 6 ? index : 1;
            Mode1Values bufVals = CModesEntities.Mode1ValuesList[index];
            M1Values = new Mode1Values(bufVals.Num, activeModePicture: bufVals.ActiveModePicture,
                      bufVals.SelectModePics,
                      bufVals.ModeIcons,
                      bufVals.ModeSettingsRoute,
                      bufVals.StartAddress, bufVals.MiniIcon);
            M1Values.SupplySP.Value = bufVals.SupplySP.Value;
            M1Values.ExhaustSP.Value = bufVals.ExhaustSP.Value;
            M1Values.ExhaustDisb.Value = bufVals.ExhaustDisb.Value;
            M1Values.TempSP.Value = bufVals.TempSP.Value;
            M1Values.ThreshPerc = bufVals.ThreshPerc;
            M1Values.SFanCorr.Value = bufVals.SFanCorr.Value;
            M1Values.EFanCorr.Value = bufVals.EFanCorr.Value;
        }

        private void ExecuteSPReturn(object obj)
        {
            if (CActivePagesEntities.IsCorrSetPointsPage)
                CActivePagesEntities.SetActivePageState(ActivePageState.OtherSettingsPage);
            else
                CActivePagesEntities.SetActivePageState(ActivePageState.SettingsPage);
        }

        #endregion

        #region Timers SetPoints callbacks 
        private void SubBut0Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ExecuteSPSub0(new object());
            TSetExecuteBtnLeft1(new object());
            ExecuteTimeSub0(new object());
        }

        private void AddBut0Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ExecuteSPAdd0(new object());
            TSetExecuteBtnRight1(new object());
            ExecuteTimeAdd0(new object());
        }
        private void SubBut1Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ExecuteSPSub1(new object());
            TSetExecuteBtnLeft2(new object());
            ExecuteTimeSub1(new object());
        }

        private void AddBut1Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ExecuteSPAdd1(new object());
            TSetExecuteBtnRight2(new object());
            ExecuteTimeAdd1(new object());
        }
        private void SubBut2Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ExecuteSPSub2(new object());
            TSetExecuteBtnLeft3(new object());
            ExecuteTimeSub2(new object());
        }

        private void AddBut2Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ExecuteSPAdd2(new object());
            TSetExecuteBtnRight3(new object());
            ExecuteTimeAdd2(new object());
        }

        private void SubBut3Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ExecuteSPSub3(new object());
            ExecuteTimeSub3(new object());
        }

        private void AddBut3Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ExecuteSPAdd3(new object());
            ExecuteTimeAdd3(new object());
        }

        private void SubBut4Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ExecuteSPSub4(new object());
            ExecuteTimeSub4(new object());
        }

        private void AddBut4Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ExecuteSPAdd4(new object());
            ExecuteTimeAdd4(new object());
        }
        #endregion

        #region Settings execute methods
        private void ExecuteJournal(object obj)
        {

            CActivePagesEntities.SetActivePageState(ActivePageState.JournalPage);
        }

        private void ExecuteShedulerTable(object obj)
        {
            CModesEntities.TTitle = "Расписание";
            for (int i = 0; i < CModesEntities.CTimeModeValues.Count; i++)
            {
                CModesEntities.CTimeModeValues[i].StrokeImg.Current =
               CModesEntities.CTimeModeValues[i].StrokeImg.Default;
            }
            CActivePagesEntities.SetActivePageState(ActivePageState.ShedulerPage);
            //int tIndex = 0;//(int)obj - 1;
            //int mode2Num = tIndex / 100;
            //int tNum = tIndex - mode2Num * 100;
            //SetTValuesByIndex(mode2Num, 3);
        }

        private void ExecuteOtherSettings(object obj)
        {
            HumiditySP = CFBs.CHumiditySP.SPPerc;
            ContactMode1Buf = CModesEntities.Mode2ValuesList[4].TimeModeValues[0].CMode1;
            COtherSettings.IsSpecMode = CFBs.OtherSettings.IsSpecMode;
            CPictureSet.SpecModeSwitch.Current = COtherSettings.IsSpecMode ? CPictureSet.SpecModeSwitch.Selected : CPictureSet.SpecModeSwitch.Default;
            CActivePagesEntities.SetActivePageState(ActivePageState.OtherSettingsPage);
        }

        private void ExecuteInformation(object obj)
        {
            CActivePagesEntities.SetActivePageState(ActivePageState.InformationPage);
        }

        private void ExecuteHelper(object obj)
        {
            CActivePagesEntities.SetActivePageState(ActivePageState.HelperPage);
        }
        #endregion

        #region Alarms execute methods
        private void ExecuteResetJournal(object obj)
        {

            int[] arr = { 1 };
            CTcpClientService.SetCommandToServer(64, arr);
        }
        #endregion

        #region Execute other settings
        private void ExecuteHumidityBtnUp(object obj)
        {
            HumiditySP = HumiditySP + 5 <= 40 ? HumiditySP + 5 : 40;
        }
        private void ExecuteHumidityBtnDn(object obj)
        {
            HumiditySP = HumiditySP - 5 >= 0 ? HumiditySP - 5 : 0;
        }

        private void ExecuteContactArrLeft(object obj)
        {
            int contactM1Num = ContactMode1Buf.Num;
            contactM1Num = contactM1Num > 0 ? contactM1Num - 1 : 0;
            ContactMode1Buf = CModesEntities.Mode1ValuesList[contactM1Num];
        }
        private void ExecuteContactArrRight(object obj)
        {
            int contactM1Num = ContactMode1Buf.Num;
            contactM1Num = contactM1Num < 3 ? contactM1Num + 1 : 3;
            ContactMode1Buf = CModesEntities.Mode1ValuesList[contactM1Num];
        }

        private void ExecuteIsSpecMode(object obj)
        {
            COtherSettings.IsSpecMode = !COtherSettings.IsSpecMode;
            CPictureSet.SpecModeSwitch.Current = COtherSettings.IsSpecMode ? CPictureSet.SpecModeSwitch.Selected : CPictureSet.SpecModeSwitch.Default;
        }

        private void ExecuteOtherSettingsReturn(object obj)
        {
            CActivePagesEntities.SetActivePageState(ActivePageState.SettingsPage);
        }

        private void ExecuteSetOtherSettings(object obj)
        {
            int isSpecMode = COtherSettings.IsSpecMode ? 1 : 0;
            int isScheduler = CFBs.OtherSettings.IsScheduler ? 1 : 0;
            int[] val = { HumiditySP, isScheduler, ContactMode1Buf.Num, isSpecMode };
            CTcpClientService.SetCommandToServer(23, val);
        }



        private void ExecuteSetCorrSP(object obj)
        {
            SetM1ValuesByIndex(CModesEntities.CMode1.Num);
            CActivePagesEntities.SetActivePageState(ActivePageState.CorrSetPointsPage);
        }
        #endregion

        #region Execute Updater
        private void ExecuteUpdate(object obj)
        {
            bool isRequiredState = CModesEntities.CMode1.Num == 0 || CModesEntities.CMode1.Num == 7;
            if (CFBs.CUpdater.IsUpdate == 0 && isRequiredState)
            {
                if (CFBs.CUpdater.BinaryData == null || CFBs.CUpdater.BinaryData.Length == 0)
                {
                    CFilesEntities.SystemMessage = ".bin файл не загружен!";
                    return;
                }
                //   CFBs.CUpdater.BinaryData = Task.Run(() => _fileSystemService.ReadBytes("gold.bin")).Result;
                //Определяем 2-мерный массив бинарных данных, с учетом что 1 байт==2 хекса.
                int dimention0 = CFBs.CUpdater.BinaryData.Length / CFBs.CUpdater.BinSize;
                int remainder = CFBs.CUpdater.BinaryData.Length % CFBs.CUpdater.HexSize;
                if (remainder > 0) dimention0 += 1;
                //2 байта под ID, 1 байт под CRC
                byte[,] buffer = new byte[dimention0, CFBs.CUpdater.BinSize + 3];
                byte[,] crcBuffer = new byte[dimention0, CFBs.CUpdater.BinSize];
                int index1 = 0, index2 = 0;
                int crcBuffersize = dimention0 * CFBs.CUpdater.BinSize;
                for (int i = 0; i < crcBuffersize; i++)
                {
                    if (index2 >= CFBs.CUpdater.BinSize)
                    {
                        index2 = 0;
                        index1 += 1;
                    }
                    if (i < CFBs.CUpdater.BinaryData.Length)
                    {
                        crcBuffer[index1, index2] = CFBs.CUpdater.BinaryData[i];
                    }
                    else
                    {
                        crcBuffer[index1, index2] = 255;
                    }
                    index2 += 1;
                }
                byte[,] useData = new byte[dimention0, CFBs.CUpdater.BinSize + 3];
                index1 = 0; index2 = 0;
                ushort id = 1;
                //Формируем пакет с ID и CRC
                for (int i = 0; i < crcBuffer.GetLength(0); i++)
                {
                    //Назначаем ID
                    useData[i, index2] = (byte)(id >> 8);
                    index2 += 1;
                    useData[i, index2] = (byte)id;
                    index2 += 1;
                    id += 1;
                    for (int j = 0; j < crcBuffer.GetLength(1); j++)
                    {
                        useData[i, index2] = crcBuffer[i, j];
                        index2 += 1;
                    }
                    byte[] crc = new byte[CFBs.CUpdater.BinSize];
                    for (int k = 0; k < crc.Length; k++)
                    {
                        crc[k] = crcBuffer[i, k];
                    }
                    byte crcResult = _mathService.CalculateChecksum(crc);
                    useData[i, CFBs.CUpdater.BinSize + 2] = crcResult;
                    index2 = 0;
                }
                char[,] hexResult = new char[useData.GetLength(0), useData.GetLength(1) * 2];

                for (int i = 0; i < useData.GetLength(0); i++)
                {
                    for (int j = 0; j < useData.GetLength(1); j++)
                    {
                        char[] charResult = _mathService.GetHexCharsFromByte(useData[i, j]);
                        hexResult[i, j * 2] = charResult[0];
                        hexResult[i, j * 2 + 1] = charResult[1];
                    }
                }
                CFBs.CUpdater.PacketsCount.Value = hexResult.GetLength(0);//3;
                CFBs.CUpdater.UseCharData = hexResult;
                int[] vals = { CFBs.CUpdater.PacketsCount.Value };
                CFBs.CUpdater.FileContent.Clear();
                for (int i = 0; i < hexResult.GetLength(0); i++)
                {
                    // if (i < 362)
                    // {
                    for (int j = 4; j < hexResult.GetLength(1) - 2; j++)
                    {
                        CFBs.CUpdater.FileContent.Append(hexResult[i, j]);
                    }

                    //}
                    //else
                    //{
                    //    for (int j = 0; j < 1024; j++)
                    //    {
                    //        CFBs.CUpdater.FileContent.Append("F");
                    //    }
                    //}

                }
                Task.Run(() => _fileSystemService.SaveToFileAsync("updater", CFBs.CUpdater.FileContent.ToString()));
                CTcpClientService.SetCommandToServer(167, vals);
            }
            else
            {

            }
        }

        private void ExecuteUpdateHex(object obj)
        {
            if (CFBs.CUpdater.IsUpdate == 0)
            {
                string result = _fileSystemService.GetUpdaterFromFile();
                string[] strokes = result.Split(':');
                int startAddress = 0;
                int lastAddress = 0;
                bool isRightData = true;
                List<HexStroke> hexList = new List<HexStroke>();
                for (int i = 1; i < strokes.Length; i++)
                {
                    byte[] strokeBytes = new byte[strokes[i].Length / 2 - 1];
                    int byteCounter = 0;
                    //Формируем байты из char
                    for (int j = 0; j < strokes[i].Length - 3; j += 2)
                    {
                        string byteStr = strokes[i][j].ToString() + strokes[i][j + 1];
                        if (byte.TryParse(byteStr, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out byte value))
                        {
                            strokeBytes[byteCounter] = value;
                        }
                        byteCounter += 1;
                    }
                    int length = strokeBytes[0];
                    int addr = strokeBytes[1] << 8 | strokeBytes[2];
                    int command = strokeBytes[3];
                    //байты данных для CRC
                    byte[] useData = new byte[length];
                    for (int j = 0; j < useData.Length; j++)
                    {
                        useData[j] = strokeBytes[j + 4];
                    }
                    //Символьные данные для передачи
                    char[] charUseData = new char[length * 2];
                    for (int j = 0; j < length * 2; j++)
                    {
                        charUseData[j] = strokes[i][j + 8];
                    }
                    //Формирование и сравнение CRC
                    byte[] crcData = new byte[strokeBytes.Length - 1];
                    for (int j = 0; j < crcData.Length; j++)
                    {
                        crcData[j] = strokeBytes[j];
                    }
                    byte crc = strokeBytes[4 + length];
                    byte countedCRC = _mathService.CalculateChecksum(crcData);
                    if (crc != countedCRC)
                    {
                        isRightData = false;
                        break;
                    }
                    if (command == 0)
                    {
                        //Умножаем, так как передаем char
                        int charAddr = addr * 2;
                        int charLength = length * 2;
                        startAddress = startAddress < charAddr ? startAddress : charAddr;
                        lastAddress = lastAddress > charAddr + charLength ? lastAddress : charAddr + charLength;
                        hexList.Add(new HexStroke() { CharLength = charLength, CharAddress = charAddr, UseData = useData, CharUseData = charUseData, CRC = crc });
                    }
                }
                if (isRightData)
                {
                    int dif = (lastAddress - startAddress) % 4096;
                    int charDataLength = (lastAddress - startAddress) / 4096;
                    if (dif != 0) charDataLength += 1;
                    charDataLength *= 4096;
                    char[] hexUseData = new char[charDataLength];
                    for (int i = 0; i < charDataLength; i++)
                    {
                        hexUseData[i] = 'F';
                    }
                    for (int j = 0; j < hexList.Count; j++)
                    {
                        for (int i = 0; i < hexList[j].CharUseData.Length; i++)
                        {
                            hexUseData[hexList[j].CharAddress + i - startAddress] = hexList[j].CharUseData[i];
                        }
                    }



                    int[] vals = { CFBs.CUpdater.PacketsCount.Value };
                    CFBs.CUpdater.PacketsCount.Value = CFBs.CUpdater.FileContentList.Count;
                    //CTcpClientService.SetCommandToServer(157 + _menuesEntities.WriteOffset, vals);
                }
            }
        }

        #region Set bootloader
        private void ExecuteReset(object obj)
        {
            int[] reset = { CFBs.CUpdater.AutoUpdIndex };
            CTcpClientService.SetCommandToServer(168, reset);
        }

        private void ExecuteDownload(object obj)
        {

            if (CFBs.CUpdater.IsUpdate != 1)
            {
                CTcpClientService.Disconnect();
                Task.Delay(200);
                var pickOptions = new PickOptions
                {
                    PickerTitle = "Выберите bin файл прошивки",
                };
                CFilesEntities.CFileResult = FilePicker.Default.PickAsync(pickOptions);
                // Создаем таймер
                _fileResultTimer = Dispatcher.GetForCurrentThread().CreateTimer();
                _fileResultTimer.Interval = TimeSpan.FromSeconds(1);
                _fileResultTimer.Tick -= OnTimerTick;
                _fileResultTimer.Tick += OnTimerTick;
                CFilesEntities.FileIsReading = true;
                _fileResultTimer.Start();
            }
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            if (CFilesEntities.CFileResult != null && CFilesEntities.CFileResult.IsCompleted)
            {
                try
                {
                    string path = CFilesEntities.CFileResult.Result.FullPath;
                    if (path.EndsWith(".bin"))
                    {
                        CFBs.CUpdater.BinaryData = _fileSystemService.ReadBytes(path);
                        CFilesEntities.SystemMessage = "Файл " + CFilesEntities.CFileResult.Result.FileName + " загружен";
                    }
                    else
                    {
                        CFilesEntities.SystemMessage = "Неверный формат файла";
                    }
                    CFilesEntities.SystemMessage = "Файл " + CFilesEntities.CFileResult.Result.FileName + " загружен";
                }
                catch
                {
                    CFilesEntities.SystemMessage = "Отмена загрузки";
                    _fileResultTimer.Stop();
                }
                finally
                {
                    CFilesEntities.FileIsReading = false;
                    //
                    ExecuteFileConnect();
                    // CActivePagesEntities.SetActivePageState(ActivePageState.OtherSettingsPage);
                    _fileResultTimer.Stop();
                }

            }
        }


        async private void ExecuteFileConnect()
        {

            EthernetEntities.ConnectIP =
                 $"{EthernetEntities.IP1}.{EthernetEntities.IP2}.{EthernetEntities.IP3}.{EthernetEntities.IP4}";
            EthernetEntities.SystemMessage = "Check";
            if (!CTcpClientService.IsConnecting)
            {
                await CTcpClientService.Connect();
                if (EthernetEntities.IsConnected)
                {
                    CModesEntities.ShedCountQueues = 0;
                    CActivePagesEntities.SetActivePageState(ActivePageState.BootloaderPage);
                    CPictureSet.SetPicureSetIfNeed(CPictureSet.LinkHeader, CPictureSet.LinkHeader.Selected);
                    await _fileSystemService.SaveToFileAsync("ConnectIP", EthernetEntities.ConnectIP);
                    CTcpClientService.SendRecieveTask();
                }
                else
                {
                    CActivePagesEntities.SetActivePageState(ActivePageState.StartPage);
                }
            }
            else
            {
                CPictureSet.SetPicureSetIfNeed(CPictureSet.LinkHeader, CPictureSet.LinkHeader.Default);
                EthernetEntities.SystemMessage = "В данный момент подключаемся";
            }
        }

        private void ExecuteBootLoaderBack(object obj)
        {
            CActivePagesEntities.SetActivePageState(ActivePageState.OtherSettingsPage);
        }
        #endregion

        private void ExecuteSetTime(object obj)
        {
            CActivePagesEntities.SetActivePageState(ActivePageState.TimePage);
            TimeBuffer = new();
            TimeBuffer.Year = CFBs.CTime.Year;
            TimeBuffer.Month = CFBs.CTime.Month;
            TimeBuffer.Day = CFBs.CTime.Day;
            TimeBuffer.Hour = CFBs.CTime.Hour;
            TimeBuffer.Minute = CFBs.CTime.Minute;
        }

        private void ExecuteChangeFilter(object obj)
        {
            int[] vals = { 1 };
            CTcpClientService.SetCommandToServer(163, vals);
        }

        private void ExecuteBootloaderSet(object obj)
        {
            CActivePagesEntities.SetActivePageState(ActivePageState.BootloaderPage);
        }
        #endregion

        #region TSettings  methods
        private void ExecuteSetTData(object obj)
        {
            CActivePagesEntities.SetActivePageState(ActivePageState.SetTSettingsPage);
            int tIndex = (int)obj - 1;
            int mode2Num = tIndex / 100;
            int tNum = tIndex - mode2Num * 100;
            SetTValuesByIndex(mode2Num, tNum);
        }

        private void ExecuteTRet(object obj)
        {
            CActivePagesEntities.SetActivePageState(ActivePageState.SettingsPage);
        }
        #endregion

        #region Execute loading
        private void ExecuteLoadingReturn(object obj)
        {
            CActivePagesEntities.SetActivePageState(ActivePageState.SettingsPage);
        }
        #endregion

        #region Execute journal
        private void ExecuteJournalReturn(object obj)
        {
            CActivePagesEntities.SetActivePageState(ActivePageState.SettingsPage);
        }
        #endregion

        #region TSet execute methods
        private void TSetExecuteBtnLeft0(object obj)
        {
            if (TValues != null)
                TValues.DayNum = TValues.DayNum - 1 >= 0 ? TValues.DayNum - 1 : 0;
        }
        private void TSetExecuteBtnRight0(object obj)
        {
            if (TValues != null)
                TValues.DayNum = TValues.DayNum + 1 <= 8 ? TValues.DayNum + 1 : 8;
        }

        private void TSetExecuteBtnLeft1(object obj)
        {
            if (TValues != null)
                TValues.Hour = TValues.Hour - 1 >= 0 ? TValues.Hour - 1 : 0;
        }
        private void TSetExecuteBtnRight1(object obj)
        {
            if (TValues != null)
                TValues.Hour = TValues.Hour + 1 <= 23 ? TValues.Hour + 1 : 23;
        }

        private void TSetExecuteBtnLeft2(object obj)
        {
            if (TValues != null)
                TValues.Minute = TValues.Minute - 1 >= 0 ? TValues.Minute - 1 : 0;

        }
        private void TSetExecuteBtnRight2(object obj)
        {
            if (TValues != null)
                TValues.Minute = TValues.Minute + 1 <= 60 ? TValues.Minute + 1 : 60;
        }

        private void TSetExecuteBtnLeft3(object obj)
        {
            if (TValues != null)
            {
                int mode1Num = TValues.CMode1.Num;
                mode1Num = mode1Num - 1 >= 0 ? mode1Num - 1 : 0;
                TValues.CMode1 = CModesEntities.Mode1ValuesList[mode1Num];
            }

        }
        private void TSetExecuteBtnRight3(object obj)
        {
            if (TValues != null)
            {
                int mode1Num = TValues.CMode1.Num;
                mode1Num = mode1Num + 1 <= 3 ? mode1Num + 1 : 3;
                TValues.CMode1 = CModesEntities.Mode1ValuesList[mode1Num];
            }
        }

        private void TSetExecuteReturn(object obj)
        {
            if (TValues != null)
            {
                int val = TValues.Mode2Num == 2 ? 0 : 1;
                CActivePagesEntities.SetActivePageState(ActivePageState.ShedulerPage, val);
            }
            for (int i = 0; i < CModesEntities.CTimeModeValues.Count; i++)
            {
                CModesEntities.CTimeModeValues[i].StrokeImg.Current =
               CModesEntities.CTimeModeValues[i].StrokeImg.Default;
            }

        }

        private void TSetExecuteOK(object obj)
        {
            if (TValues != null)
            {
                int[] values = { TValues.DayNum, TValues.Hour, TValues.Minute, TValues.CMode1.Num };
                CTcpClientService.SetCommandToServer(TValues.WriteAddress, values);
                int val = TValues.Mode2Num == 2 ? 0 : 1;
                CActivePagesEntities.SetActivePageState(ActivePageState.ShedulerPage, val);
            }
            for (int i = 0; i < CModesEntities.CTimeModeValues.Count; i++)
            {
                CModesEntities.CTimeModeValues[i].StrokeImg.Current =
               CModesEntities.CTimeModeValues[i].StrokeImg.Default;
            }
        }

        /// <summary>
        /// Промежуточное значение TValue
        /// </summary>
        /// <param name="index"></param>
        private void SetTValuesByIndex(int m2Num, int tModeNum)
        {
            TimeModeValues tVal = CModesEntities.Mode2ValuesList[m2Num].TimeModeValues[tModeNum];
            TValues = new TimeModeValues(tVal.TimeModeNum, tVal.CMode1, tVal.WriteAddress, tVal.TimeModeNum, m2Num);
            TValues.DayNum = tVal.DayNum;
            TValues.Hour = tVal.Hour;
            TValues.Minute = tVal.Minute;
        }
        #endregion

        #region Execute humidity
        private void CancelHumidity(object obj)
        {
            CActivePagesEntities.SetActivePageState(ActivePageState.OtherSettingsPage);
        }

        private void ExecuteOkHumidity(object obj)
        {
            int[] vals = { HumiditySP };
            CTcpClientService.SetCommandToServer(165, vals);
            CActivePagesEntities.SetActivePageState(ActivePageState.OtherSettingsPage);
        }
        #endregion

        #region Time
        private void ExecuteTimeAdd0(object obj)
        {
            if (TimeBuffer != null)
                TimeBuffer.Year = TimeBuffer.Year + 1 < 99 ? TimeBuffer.Year + 1 : 99;
        }
        private void ExecuteTimeSub0(object obj)
        {
            if (TimeBuffer != null)
                TimeBuffer.Year = TimeBuffer.Year - 1 > 0 ? TimeBuffer.Year - 1 : 0;
        }

        private void ExecuteTimeAdd1(object obj)
        {
            if (TimeBuffer != null)
                TimeBuffer.Month = TimeBuffer.Month + 1 < 12 ? TimeBuffer.Month + 1 : 12;
        }
        private void ExecuteTimeSub1(object obj)
        {
            if (TimeBuffer != null)
                TimeBuffer.Month = TimeBuffer.Month - 1 > 0 ? TimeBuffer.Month - 1 : 0;
        }

        private void ExecuteTimeAdd2(object obj)
        {
            if (TimeBuffer != null)
                TimeBuffer.Day = TimeBuffer.Day + 1 < 31 ? TimeBuffer.Day + 1 : 31;
        }
        private void ExecuteTimeSub2(object obj)
        {
            if (TimeBuffer != null)
                TimeBuffer.Day = TimeBuffer.Day - 1 > 0 ? TimeBuffer.Day - 1 : 0;
        }

        private void ExecuteTimeAdd3(object obj)
        {
            if (TimeBuffer != null)
                TimeBuffer.Hour = TimeBuffer.Hour + 1 < 60 ? TimeBuffer.Hour + 1 : 60;
        }
        private void ExecuteTimeSub3(object obj)
        {
            if (TimeBuffer != null)
                TimeBuffer.Hour = TimeBuffer.Hour - 1 > 0 ? TimeBuffer.Hour - 1 : 0;
        }

        private void ExecuteTimeAdd4(object obj)
        {
            if (TimeBuffer != null)
                TimeBuffer.Minute = TimeBuffer.Minute + 1 < 60 ? TimeBuffer.Minute + 1 : 60;
        }

        private void ExecuteTimeSub4(object obj)
        {
            if (TimeBuffer != null)
                TimeBuffer.Minute = TimeBuffer.Minute - 1 > 0 ? TimeBuffer.Minute - 1 : 0;
        }

        private void ExecuteTimeReturn(object obj)
        {
            CActivePagesEntities.SetActivePageState(ActivePageState.OtherSettingsPage);
        }

        private void ExecuteTimeOk(object obj)
        {
            int[] vals = { _timeBuffer.Year, _timeBuffer.Month, _timeBuffer.Day, _timeBuffer.Hour, _timeBuffer.Minute };
            CTcpClientService.SetCommandToServer(57, vals);
            CActivePagesEntities.SetActivePageState(ActivePageState.OtherSettingsPage);
        }
        #endregion

        private void ClientDisceonnectedCallback()
        {
            if (CActivePagesEntities != null)
            {
                CActivePagesEntities.SetActivePageState(ActivePageState.StartPage);
            }
        }

    }
}