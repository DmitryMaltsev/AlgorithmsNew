using Android_Silver.Entities;
using Android_Silver.Entities.FBEntities;
using Android_Silver.Entities.Modes;
using Android_Silver.Entities.Visual;
using Android_Silver.Entities.Visual.Menus;
using Android_Silver.Services;
using Android_Silver.ViewModels;

using System.Net.Sockets;
using System.Windows.Input;

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

        private string _messageToServer = "Test";

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



        #endregion

        #region Commands
        public ICommand StartPageConnectCommand { get; private set; }
        #region MainPageCommands

        #region Loading command
        public ICommand LoadingReturnCommand { get; private set; }
        #endregion

        public ICommand ConnectCommand { get; private set; }
        public ICommand DisconnectCommand { get; private set; }
        public ICommand GetIPCommand { get; private set; }
        public ICommand SPCommand { get; private set; }
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
        public ICommand ShedulerModeCommand { get; private set; }
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
        public ICommand SPReturnCommand { get; private set; }
        public ICommand SPOkCommand { get; private set; }
        public ICommand BtnUpCommand0 { get; private set; }
        public ICommand BtnDnCommand0 { get; private set; }
        public ICommand BtnUpCommand1 { get; private set; }
        public ICommand BtnDnCommand1 { get; private set; }

        public ICommand BtnUpCommand2 { get; private set; }
        public ICommand BtnDnCommand2 { get; private set; }
        public ICommand BtnUpCommand3 { get; private set; }
        public ICommand BtnDnCommand3 { get; private set; }
        #endregion
        #region Settings commands
        public ICommand OtherSettingsCommand { get; private set; }
        public ICommand JournalCommand { get; private set; }
        public ICommand VacationTableCommand { get; private set; }
        public ICommand ShedulerTableCommand { get; private set; }
        #endregion
        #region TSettingsCommands
        public ICommand TRetCommand { get; private set; }
        #endregion
        #region SetTSettingsCommands
        public ICommand TSetReturnCommand { get; private set; }
        public ICommand TSetOkCommand { get; private set; }
        public ICommand TSetBtnUpCommand0 { get; private set; }
        public ICommand TSetBtnDnCommand0 { get; private set; }
        public ICommand TSetBtnUpCommand1 { get; private set; }
        public ICommand TSetBtnDnCommand1 { get; private set; }
        public ICommand TSetBtnUpCommand2 { get; private set; }
        public ICommand TSetBtnDnCommand2 { get; private set; }
        public ICommand TSetBtnUpCommand3 { get; private set; }
        public ICommand TSetBtnDnCommand3 { get; private set; }
        #endregion
        #region Other settings commands
        public ICommand OtherSettingsReturnCommand { get; private set; }
        public ICommand NextOtherSettingsCommand { get; private set; }
        public ICommand ContactArrLeftCommand { get; private set; }
        public ICommand ContactArrRightCommand { get; private set; }
        public ICommand HumidityCommand { get; private set; }
        public ICommand SetTimeCommand { get; private set; }
        public ICommand UpdateCommand { get; private set; }
        public ICommand ChangeFilterCommand { get; private set; }
        #endregion
        #region Humidity commands
        public ICommand HumidityReturnCommand { get; private set; }
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
        public ICommand TimeBtnUpCommand0 { get; private set; }
        public ICommand TimeBtnDnCommand0 { get; private set; }
        public ICommand TimeBtnUpCommand1 { get; private set; }
        public ICommand TimeBtnDnCommand1 { get; private set; }
        public ICommand TimeBtnUpCommand2 { get; private set; }
        public ICommand TimeBtnDnCommand2 { get; private set; }
        public ICommand TimeBtnUpCommand3 { get; private set; }
        public ICommand TimeBtnDnCommand3 { get; private set; }
        public ICommand TimeBtnUpCommand4 { get; private set; }
        public ICommand TimeBtnDnCommand4 { get; private set; }
        public ICommand TimeOkCommand { get; private set; }
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
       
        public MainPageViewModel()
        {
            EthernetEntities = DIContainer.Resolve<EthernetEntities>();
            CTcpClientService = DIContainer.Resolve<TcpClientService>();
            CModesEntities = DIContainer.Resolve<ModesEntities>();
            CActivePagesEntities = DIContainer.Resolve<ActivePagesEntities>();
            CPictureSet = DIContainer.Resolve<PicturesSet>();
            CFBs = DIContainer.Resolve<FBs>();
            _fileSystemService = DIContainer.Resolve<FileSystemService>();
            _menuesEntities = DIContainer.Resolve<MenusEntities>();
#if ANDROID
            AndroidEntity.WifiStateChanged -= EthernetEntities.WifiStateChangeCallback;
            AndroidEntity.WifiStateChanged += EthernetEntities.WifiStateChangeCallback;
#endif
            StartPageConnectCommand = new Command(ExecuteConnect);
            ConnectCommand = new Command(ExecuteConnect);
            DisconnectCommand = new Command(ExecuteDisconnect);
            SPCommand = new Command(ExecuteSetSP);
            ChooseModeCommand = new Command(ExecuteChooseMode);
            SettingsCommand = new Command(ExecuiteSettings);
            MinModeCommand = new Command(ExecuteMinMode);
            NormalModeCommand = new Command(ExecuteNormal);
            MaxModeCommand = new Command(ExecuteMaxMode);
            KitchenModeCommand = new Command(ExecuteKitchenMode);
            ShedulerModeCommand = new Command(ExecuteSheduler);
            VacationModeCommand = new Command(ExecuteVacationMode);
            TurnOffModeCommand = new Command(ExecuteTurnOffMode);
            JournalCommand = new Command(ExecuteJournal);
            VacationTableCommand = new Command(ExecuteVacationTable);
            ShedulerTableCommand = new Command(ExecuteShedulerTable);
            HomeCommand = new Command(ExecuteHomeCommand);
            ResetJournalCommand = new Command(ExecuteResetJournal);
            NextOtherSettingsCommand = new Command(ExecuteNextOtherSetiings);
            JournalReturnCommand = new Command(ExecuteJournalReturn);
            ContactArrLeftCommand = new Command(ExecuteContactArrLeft);
            ContactArrRightCommand = new Command(ExecuteContactArrRight);
            HumidityCommand = new Command(ExecuteHumidity);
            TimeReturnCommand = new Command(ExecuteTimeReturn);
            ChangeFilterCommand = new Command(ExecuteChangeFilter);
            UpdateCommand = new Command(ExecuteUpdate);
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
            #region Set ponts commands
            NextSetPointsCommand = new Command(ExecuteNextSetPoints);
            SPOkCommand = new Command(ExecuteSPSOK);
            BtnUpCommand0 = new Command(ExecuteBtnUP0);
            BtnDnCommand0 = new Command(ExecuteBtnDn0);
            BtnUpCommand1 = new Command(ExecuteBtnUP1);
            BtnDnCommand1 = new Command(ExecuteBtnDn1);
            BtnUpCommand2 = new Command(ExecuteBtnUP2);
            BtnDnCommand2 = new Command(ExecuteBtnDn2);
            BtnUpCommand3 = new Command(ExecuteBtnUP3);
            BtnDnCommand3 = new Command(ExecuteBtnDn3);
            SPReturnCommand = new Command(ExecuteSPReturn);
            #endregion
            #region Vac commands
            SetTDataCommand = new Command(ExecuteSetTData);
            #endregion
            #region TCommands
            TRetCommand = new Command(ExecuteTRet);
            #endregion
            #region TSet commands
            TSetOkCommand = new Command(TSetExecuteOK);
            TSetBtnUpCommand0 = new Command(TSetExecuteBtnUP0);
            TSetBtnDnCommand0 = new Command(TSetExecuteBtnDn0);
            TSetBtnUpCommand1 = new Command(TSetExecuteBtnUP1);
            TSetBtnDnCommand1 = new Command(TSetExecuteBtnDn1);
            TSetBtnUpCommand2 = new Command(TSetExecuteBtnUP2);
            TSetBtnDnCommand2 = new Command(TSetExecuteBtnDn2);
            TSetBtnUpCommand3 = new Command(TSetExecuteBtnUP3);
            TSetBtnDnCommand3 = new Command(TSetExecuteBtnDn3);
            TSetReturnCommand = new Command(TSetExecuteReturn);
            #endregion
            #region Settings commands
            OtherSettingsCommand = new Command(ExecuteOtherSettings);
            #endregion
            #region Other settings commands
            OtherSettingsReturnCommand = new Command(ExecuteOtherSettingsReturn);
            SetTimeCommand = new Command(ExecuteSetTime);
            #endregion
            #region Humidity commands
            HumidityReturnCommand = new Command(ExecuteHumidityReturn);
            OkHumidityCommand = new Command(ExecuteOkHumidity);
            CancelHumidityCommand = new Command(CancelHumidity);
            HumidityBtnUpCommand = new Command(ExecuteHumidityBtnUp);
            HumidityBtnDnCommand = new Command(ExecuteHumidityBtnDn);
            #endregion
            #region Time commands
            TimeBtnUpCommand0 = new Command(ExecuteTimeBtnUp0);
            TimeBtnDnCommand0 = new Command(ExecuteTimeBtnDn0);
            TimeBtnUpCommand1 = new Command(ExecuteTimeBtnUp1);
            TimeBtnDnCommand1 = new Command(ExecuteTimeBtnDn1);
            TimeBtnUpCommand2 = new Command(ExecuteTimeBtnUp2);
            TimeBtnDnCommand2 = new Command(ExecuteTimeBtnDn2);
            TimeBtnUpCommand3 = new Command(ExecuteTimeBtnUp3);
            TimeBtnDnCommand3 = new Command(ExecuteTimeBtnDn3);
            TimeBtnUpCommand4 = new Command(ExecuteTimeBtnUp4);
            TimeBtnDnCommand4 = new Command(ExecuteTimeBtnDn4);
            TimeOkCommand = new Command(ExecuteTimeOk);
            #endregion


            _fileSystemService.GetIPFromFile();
            SetTValuesByIndex(0, 0);//?????
            CTcpClientService.ClientDisconnected -= ClientDisceonnectedCallback;
            CTcpClientService.ClientDisconnected += ClientDisceonnectedCallback;
            // StartTimer();
            // CModesEntities.Mode2ValuesList[2].TimeModeValues[2].CMode1.MiniIconV
            //ContactModeImg = CModesEntities.Mode2ValuesList[4].TimeModeValues[0].CMode1.MiniIcon;
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
                    CTcpClientService.SendRecieveTask("0100,057");
                    // TcpClientService.SendRecieveTask("137,4");ё
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
            /* Page[] stack = Shell.Current.Navigation.NavigationStack.ToArray();
             for (int i = stack.Length - 1; i > 0; i--)
             {
                 Shell.Current.Navigation.RemovePage(stack[i]);
                 Shell.Current.Navigation.RemovePage(stack[i]);
             }
             await Task.Delay(1);
             await Shell.Current.Navigation.PopToRootAsync(false);
             await Task.Delay(1);*/
            if (CModesEntities.CMode1.Num == 7)
            {
                CActivePagesEntities.SetActivePageState(ActivePageState.JournalPage);
            }
            else
                CActivePagesEntities.SetActivePageState(ActivePageState.ChooseModePage);
            //  await Shell.Current.GoToAsync("chooseModePage", false);
        }
        #endregion

        #region Execute modes

        private void ExecuteTurnOffMode(object obj)
        {
            int[] index = { 0 };
            CTcpClientService.SetCommandToServer(108+ _menuesEntities.WriteOffset, index);
            CActivePagesEntities.SetActivePageState(ActivePageState.MainPage);
        }
        private void ExecuteMinMode(object obj)
        {
            int[] index = { 1, 0 };
            CTcpClientService.SetCommandToServer(108 + _menuesEntities.WriteOffset, index);
            CActivePagesEntities.SetActivePageState(ActivePageState.MainPage);
        }

        private void ExecuteNormal(object obj)
        {
            int[] index = { 2, 0 };
            CTcpClientService.SetCommandToServer(108 + _menuesEntities.WriteOffset, index);
            CActivePagesEntities.SetActivePageState(ActivePageState.MainPage);
        }

        private void ExecuteMaxMode(object obj)
        {
            int[] index = { 3, 0 };
            CTcpClientService.SetCommandToServer(108 + _menuesEntities.WriteOffset, index);
            CActivePagesEntities.SetActivePageState(ActivePageState.MainPage);
        }

        private void ExecuteKitchenMode(object obj)
        {
            //      int[] index = { 1 };
            //      TcpClientService.SetCommandToServer(309, index);
            CActivePagesEntities.SetActivePageState(ActivePageState.KithchenTimerPage);
        }

        private void ExecuteVacationMode(object obj)
        {
            int[] index = { 2 };
            CTcpClientService.SetCommandToServer(109 + _menuesEntities.WriteOffset, index);
            CActivePagesEntities.SetActivePageState(ActivePageState.MainPage);
        }

        private void ExecuteSheduler(object obj)
        {
            int[] index = { 3 };
            CTcpClientService.SetCommandToServer(109 + _menuesEntities.WriteOffset, index);
            CActivePagesEntities.SetActivePageState(ActivePageState.MainPage);
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
            CTcpClientService.SetCommandToServer(134 + _menuesEntities.WriteOffset, values);
            CActivePagesEntities.SetActivePageState(ActivePageState.MainPage); ;
        }

        private void ExecuteKitchenOk(object obj)
        {
            int[] values = { CModesEntities.Mode2ValuesList[1].TimeModeValues[0].Minute, 1, 0 };
            CTcpClientService.SetCommandToServer(134 + _menuesEntities.WriteOffset, values);
            CActivePagesEntities.SetActivePageState(ActivePageState.MainPage);
        }
        #endregion

        #region SetPoints execute commands

        private void ExecuteBtnUP0(object obj)
        {
            if (M1Values != null)
                M1Values.SypplySP = M1Values.SypplySP + 5 < 100 ? M1Values.SypplySP + 5 : 100;
        }
        private void ExecuteBtnDn0(object obj)
        {
            if (M1Values != null)
                M1Values.SypplySP = M1Values.SypplySP - 5 > 0 ? M1Values.SypplySP - 5 : 0;
        }

        private void ExecuteBtnUP1(object obj)
        {
            if (M1Values != null)
                M1Values.ExhaustSP = M1Values.ExhaustSP + 5 < 100 ? M1Values.ExhaustSP + 5 : 100;
        }
        private void ExecuteBtnDn1(object obj)
        {
            if (M1Values != null)
                M1Values.ExhaustSP = M1Values.ExhaustSP - 5 > 0 ? M1Values.ExhaustSP - 5 : 0;
        }

        private void ExecuteBtnUP2(object obj)
        {
            if (M1Values != null)
            {
                M1Values.TempSP = M1Values.TempSP + 1 < 34 ? M1Values.TempSP + 1 : 34;
                if (M1Values.TempSP < 16) M1Values.TempSP = 16;

            }
        }
        private void ExecuteBtnDn2(object obj)
        {
            if (M1Values != null)
                M1Values.TempSP = M1Values.TempSP - 1 > 16 ? M1Values.TempSP - 1 : 16;
        }

        private void ExecuteBtnUP3(object obj)
        {
            if (M1Values != null)
                M1Values.PowerLimitSP = M1Values.PowerLimitSP + 5 < 100 ? M1Values.PowerLimitSP + 5 : 100;
        }
        private void ExecuteBtnDn3(object obj)
        {
            if (M1Values != null)
                M1Values.PowerLimitSP = M1Values.PowerLimitSP - 5 > 0 ? M1Values.PowerLimitSP - 5 : 0;
        }

        private void ExecuteSPSOK(object obj)
        {
            if (M1Values != null)
            {
                int[] values = { M1Values.SypplySP, M1Values.ExhaustSP, M1Values.TempSP, M1Values.PowerLimitSP };
                CTcpClientService.SetCommandToServer(M1Values.StartAddress, values);
                CActivePagesEntities.SetActivePageState(ActivePageState.MainPage);
            }
        }

        private void ExecuteNextSetPoints(object obj)
        {
            int ind = 0;
            if (M1Values.Num == 0 || M1Values.Num == 5)
            {
                ind = 1;
            }
            else
            if (M1Values.Num < 5)
            {
                ind = M1Values.Num + 1;
            }
            SetM1ValuesByIndex(ind);
        }
        private void SetM1ValuesByIndex(int index)
        {
            index = index > 0 && index < 6 ? index : 1;

            Mode1Values bufVals = CModesEntities.Mode1ValuesList[index];
            M1Values = new Mode1Values(bufVals.Num, bufVals.ActiveModePics,
                      bufVals.SelectModePics,
                      bufVals.ModeIcons,
                      bufVals.ModeSettingsRoute,
                      bufVals.StartAddress, bufVals.MiniIcon);
            M1Values.SypplySP = bufVals.SypplySP;
            M1Values.ExhaustSP = bufVals.ExhaustSP;
            M1Values.TempSP = bufVals.TempSP;
            M1Values.PowerLimitSP = bufVals.PowerLimitSP;
        }

        private void ExecuteSPReturn(object obj)
        {

            CActivePagesEntities.SetActivePageState(ActivePageState.MainPage);
        }

        #endregion

        #region Settings execute methods
        private void ExecuteJournal(object obj)
        {
            CActivePagesEntities.SetActivePageState(ActivePageState.JournalPage);
        }

        private void ExecuteVacationTable(object obj)
        {
            // CActivePagesEntities.SetActivePageState(ActivePageState.TSettingsPage, 0);
            // TTitle = "Расписание для отпуска";
            CActivePagesEntities.SetActivePageState(ActivePageState.LoadingPage, 0);
        }

        private void ExecuteShedulerTable(object obj)
        {
            //  if (CModesEntities.ShedCountQueues == 0)
            // {
            CActivePagesEntities.SetActivePageState(ActivePageState.LoadingPage, 1);
            // CModesEntities.ShedCountQueues += 1;
            //}
            //  else
            /*   {
                   for (int i = 0; i < CModesEntities.Mode2ValuesList[3].TimeModeValues.Count; i++)
                   {
                       CModesEntities.Mode2ValuesList[3].TimeModeValues[i].StrokeImg.Current =
                      CModesEntities.Mode2ValuesList[3].TimeModeValues[1].StrokeImg.Default;
                   }
                   CModesEntities.CTimeModeValues = CModesEntities.Mode2ValuesList[3].TimeModeValues;
                   CActivePagesEntities.SetActivePageState(ActivePageState.TSettingsPage);
               }*/
        }

        private void ExecuteOtherSettings(object obj)
        {
            CActivePagesEntities.SetActivePageState(ActivePageState.OtherSettingsPage);
        }

        #endregion

        #region Alarms execute methods
        private void ExecuteResetJournal(object obj)
        {

            int[] arr = { 1 };
            CTcpClientService.SetCommandToServer(137 + _menuesEntities.WriteOffset, arr);
        }
        #endregion

        #region Execute other settings
        private void ExecuteContactArrLeft(object obj)
        {
            var contactTMode = CModesEntities.Mode2ValuesList[4].TimeModeValues[0];
            int contactM1Num = contactTMode.CMode1.Num;
            contactM1Num = contactM1Num > 0 ? contactM1Num - 1 : 0;
            contactTMode.CMode1 = CModesEntities.Mode1ValuesList[contactM1Num];
            int[] vals = { contactM1Num };
            CTcpClientService.SetCommandToServer(154 + _menuesEntities.WriteOffset, vals);
        }
        private void ExecuteContactArrRight(object obj)
        {
            var contactTMode = CModesEntities.Mode2ValuesList[4].TimeModeValues[0];
            int contactM1Num = contactTMode.CMode1.Num;
            contactM1Num = contactM1Num < 5 ? contactM1Num + 1 : 5;
            contactTMode.CMode1 = CModesEntities.Mode1ValuesList[contactM1Num];
            int[] vals = { contactM1Num };
            CTcpClientService.SetCommandToServer(154 + _menuesEntities.WriteOffset, vals);
        }

        private void ExecuteOtherSettingsReturn(object obj)
        {
            CActivePagesEntities.SetActivePageState(ActivePageState.SettingsPage);
        }

        private void ExecuteNextOtherSetiings(object obj)
        {
            throw new NotImplementedException();
        }

        private void ExecuteHumidity(object obj)
        {
            CActivePagesEntities.SetActivePageState(ActivePageState.HumidityPage);
            HumiditySP = CFBs.CHumiditySPS.HumiditySP;
        }

        private void ExecuteUpdate(object obj)
        {
           
        }

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
            CTcpClientService.SetCommandToServer(163 + _menuesEntities.WriteOffset, vals);
        }
        #endregion

        #region TSettings execute methods
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
        private void TSetExecuteBtnUP0(object obj)
        {
            if (TValues != null)
                TValues.DayNum = TValues.DayNum + 1 <= 8 ? TValues.DayNum + 1 : 8;
        }
        private void TSetExecuteBtnDn0(object obj)
        {
            if (TValues != null)
                TValues.DayNum = TValues.DayNum - 1 >= 0 ? TValues.DayNum - 1 : 0;
        }

        private void TSetExecuteBtnUP1(object obj)
        {
            if (TValues != null)
                TValues.Hour = TValues.Hour + 1 <= 23 ? TValues.Hour + 1 : 23;
        }
        private void TSetExecuteBtnDn1(object obj)
        {
            if (TValues != null)
                TValues.Hour = TValues.Hour - 1 >= 0 ? TValues.Hour - 1 : 0;
        }

        private void TSetExecuteBtnUP2(object obj)
        {
            if (TValues != null)
                TValues.Minute = TValues.Minute + 1 <= 60 ? TValues.Minute + 1 : 60;
        }
        private void TSetExecuteBtnDn2(object obj)
        {
            if (TValues != null)
                TValues.Minute = TValues.Minute - 1 >= 0 ? TValues.Minute - 1 : 0;
        }

        private void TSetExecuteBtnUP3(object obj)
        {
            if (TValues != null)
            {
                int mode1Num = TValues.CMode1.Num;
                mode1Num = mode1Num + 1 <= 5 ? mode1Num + 1 : 5;
                TValues.CMode1 = CModesEntities.Mode1ValuesList[mode1Num];
            }
        }
        private void TSetExecuteBtnDn3(object obj)
        {
            if (TValues != null)
            {
                int mode1Num = TValues.CMode1.Num;
                mode1Num = mode1Num - 1 >= 0 ? mode1Num - 1 : 0;
                TValues.CMode1 = CModesEntities.Mode1ValuesList[mode1Num];
            }
        }

        private void TSetExecuteReturn(object obj)
        {
            if (TValues != null)
            {
                int val = TValues.Mode2Num == 2 ? 0 : 1;
                CActivePagesEntities.SetActivePageState(ActivePageState.TSettingsPage, val);
            }
        }

        private void TSetExecuteOK(object obj)
        {
            if (TValues != null)
            {
                int[] values = { TValues.DayNum, TValues.Hour, TValues.Minute, TValues.CMode1.Num };
                CTcpClientService.SetCommandToServer(TValues.WriteAddress, values);
                int val = TValues.Mode2Num == 2 ? 0 : 1;
                CActivePagesEntities.SetActivePageState(ActivePageState.TSettingsPage, val);
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
        private void ExecuteHumidityReturn(object obj)
        {
            CActivePagesEntities.SetActivePageState(ActivePageState.OtherSettingsPage);
            HumiditySP = CFBs.CHumiditySPS.HumiditySP;
        }

        private void CancelHumidity(object obj)
        {
            CActivePagesEntities.SetActivePageState(ActivePageState.OtherSettingsPage);
        }

        private void ExecuteOkHumidity(object obj)
        {
            int[] vals = { HumiditySP };
            CTcpClientService.SetCommandToServer(155 + _menuesEntities.WriteOffset, vals);
            CActivePagesEntities.SetActivePageState(ActivePageState.OtherSettingsPage);
        }

        private void ExecuteHumidityBtnUp(object obj)
        {
            HumiditySP = HumiditySP + 5 <= 40 ? HumiditySP + 5 : 40;
        }
        private void ExecuteHumidityBtnDn(object obj)
        {
            HumiditySP = HumiditySP - 5 >= 0 ? HumiditySP - 5 : 0;
        }
        #endregion

        #region Time
        private void ExecuteTimeBtnUp0(object obj)
        {
            if (TimeBuffer != null)
                TimeBuffer.Year = TimeBuffer.Year + 1 < 99 ? TimeBuffer.Year + 1 : 99;
        }
        private void ExecuteTimeBtnDn0(object obj)
        {
            if (TimeBuffer != null)
                TimeBuffer.Year = TimeBuffer.Year - 1 > 0 ? TimeBuffer.Year - 1 : 0;
        }

        private void ExecuteTimeBtnUp1(object obj)
        {
            if (TimeBuffer != null)
                TimeBuffer.Month = TimeBuffer.Month + 1 < 12 ? TimeBuffer.Month + 1 : 12;
        }
        private void ExecuteTimeBtnDn1(object obj)
        {
            if (TimeBuffer != null)
                TimeBuffer.Month = TimeBuffer.Month - 1 > 0 ? TimeBuffer.Month - 1 : 0;
        }

        private void ExecuteTimeBtnUp2(object obj)
        {
            if (TimeBuffer != null)
                TimeBuffer.Day = TimeBuffer.Day + 1 < 31 ? TimeBuffer.Day + 1 : 31;
        }
        private void ExecuteTimeBtnDn2(object obj)
        {
            if (TimeBuffer != null)
                TimeBuffer.Day = TimeBuffer.Day - 1 > 0 ? TimeBuffer.Day - 1 : 0;
        }

        private void ExecuteTimeBtnUp3(object obj)
        {
            if (TimeBuffer != null)
                TimeBuffer.Hour = TimeBuffer.Hour + 1 < 60 ? TimeBuffer.Hour + 1 : 60;
        }
        private void ExecuteTimeBtnDn3(object obj)
        {
            if (TimeBuffer != null)
                TimeBuffer.Hour = TimeBuffer.Hour - 1 > 0 ? TimeBuffer.Hour - 1 : 0;
        }

        private void ExecuteTimeBtnUp4(object obj)
        {
            if (TimeBuffer != null)
                TimeBuffer.Minute = TimeBuffer.Minute + 1 < 60 ? TimeBuffer.Minute + 1 : 60;
        }

        private void ExecuteTimeBtnDn4(object obj)
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
            CTcpClientService.SetCommandToServer(158 + _menuesEntities.WriteOffset, vals);
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

        Timer timer;
        private void StartTimer()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                timer = new Timer(obj =>
                {
                    MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        EthernetEntities.SystemMessage = $"Счетчик принятий ={CTcpClientService.ResieveCounter}";
                    });
                },
                null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
            });
        }
    }
}