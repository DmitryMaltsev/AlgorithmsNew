using Android_Silver.Entities;
using Android_Silver.Entities.FBEntities;
using Android_Silver.Entities.Modes;
using Android_Silver.Entities.Visual;
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

        public string MessageToServer
        {
            get { return _messageToServer; }
            set
            {
                _messageToServer = value;
                OnPropertyChanged(nameof(MessageToServer));
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

        private string _currentTime;
        public string CurrentTime
        {
            get { return _currentTime; }
            set
            {
                _currentTime = value;
                OnPropertyChanged(nameof(CurrentTime));
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

        #endregion

        #region Commands
        #region MainPageCommands
        public ICommand ConnectCommand { get; private set; }
        public ICommand DisconnectCommand { get; private set; }
        public ICommand GetIPCommand { get; private set; }
        public ICommand SPCommand { get; private set; }
        public ICommand SendSPCommand { get; private set; }
        public ICommand SendFloatCommand { get; private set; }
        public ICommand SettingsCommand { get; private set; }
        public ICommand ChooseModeCommand { get; private set; }
        public ICommand GoToPageCommand { get; private set; }
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

        #region SettingsCommands
        public ICommand JournalCommand { get; private set; }
        #endregion

        #region ResetCommand
        public ICommand ResetJournalCommand { get; private set; }

        #endregion
        // public ICommand SettingsCommand { get; private set; }
        #endregion

        public EthernetEntities EthernetEntities { get; set; }

        public SensorsEntities CSensorsEntities { get; set; }

        public TcpClientService TcpClientService { get; set; }


        public ModesEntities CModesEntities { get; set; }

        public PicturesSet CPictureSet { get; set; }

        public SetPoints CSetPoints { get; set; }

        public ActivePagesEntities CActivePagesEntities { get; set; }

        public Alarms CAlarms { get; set; }


        NetworkStream _stream;
        int counter = 0;
        string _cData;
        public MainPageViewModel()
        {
            EthernetEntities = DIContainer.Resolve<EthernetEntities>();
            CSensorsEntities = DIContainer.Resolve<SensorsEntities>();
            TcpClientService = DIContainer.Resolve<TcpClientService>();
            CSetPoints = DIContainer.Resolve<SetPoints>();
            CModesEntities = DIContainer.Resolve<ModesEntities>();
            CActivePagesEntities = DIContainer.Resolve<ActivePagesEntities>();
            CPictureSet = DIContainer.Resolve<PicturesSet>();
            CAlarms=DIContainer.Resolve<Alarms>();
             ConnectCommand = new Command(ExecuteConnect);
            DisconnectCommand = new Command(ExecuteDisconnect);
            SendSPCommand = new Command(ExecuteSendSP);
            SendFloatCommand = new Command(ExecuteSendFloat);
            SPCommand = new Command(ExecuteSetSP);
            ChooseModeCommand = new Command(ExecuteChooseMode);
            SettingsCommand = new Command(ExecuiteSettings);
            Value = 15;
            CActivePagesEntities.SetActivePageState(ActivePageState.JournalPage);
            MinModeCommand = new Command(ExecuteMinMode);
            NormalModeCommand = new Command(ExecuteNormal);
            MaxModeCommand = new Command(ExecuteMaxMode);
            KitchenModeCommand = new Command(ExecuteKitchenMode);
            ShedulerModeCommand = new Command(ExecuteSheduler);
            VacationModeCommand = new Command(ExecuteVacationMode);
            TurnOffModeCommand = new Command(ExecuteTurnOffMode);
            JournalCommand = new Command(ExecuteJournal);
            HomeCommand = new Command(ExecuteHomeCommand);
            ResetJournalCommand = new Command(ExecuteResetJournal);// ExecuteResetJournal();
            // StartTimer();
            #region Kitchen timer commands
            UpMInutesCommand = new Command(ExecuteUpMinutes);
            DnMinutesCommand = new Command(ExecuteDnMinutes);
            HomeCommand = new Command(ExecuteHomeCommand);
            KitchenOkCommand = new Command(ExecuteKitchenOk);
            KitchenCancelCommand = new Command(ExecuteKitchenCancel);
            StartTimer();
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
            #endregion
        }

        private void ExecuteResetJournal(object obj)
        {
            CAlarms.AlarmsCollection.Clear();
        }

        async private void ExecuteConnect()
        {
            EthernetEntities.SystemMessage = "Check";
            if (!TcpClientService.IsConnecting)
            {
                await TcpClientService.Connect();
                if (EthernetEntities.IsConnected)
                {
                    //TcpClientService.SendRecieveTask("100,08");
                    TcpClientService.SendRecieveTask("108,27");
                }
            }
            else
            {
                EthernetEntities.SystemMessage = "В данный момент подключаемся";
            }
        }

        #region Main page execute methods
        void ExecuteSetSP(object obj)
        {
            SetM1ValuesByIndex(CModesEntities.CMode1.Num);
            CActivePagesEntities.SetActivePageState(ActivePageState.SetPointsPage);
            
        }

        private void ExecuteDisconnect()
        {
            TcpClientService.Disconnect();
        }
        private Task sendBufTask;

        private void ExecuteSendSP(object obj)
        {
            EthernetEntities.MessageToServer = $"300,01,{(int)(CSetPoints.SetPoint1 * 10)}";
        }

        private void ExecuteSendFloat(object obj)
        {
            EthernetEntities.MessageToServer = $"303,01,{(int)(CSetPoints.SetPointF * 10)}";
        }

         private void ExecuiteSettings(object obj)
        {
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
            CActivePagesEntities.SetActivePageState(ActivePageState.ChooseModePage);
            //  await Shell.Current.GoToAsync("chooseModePage", false);
        }
        #endregion

        #region Execute modes

        private void ExecuteTurnOffMode(object obj)
        {
            int[] index = { 0 };
            TcpClientService.SetCommandToServer(308, index);
            CActivePagesEntities.SetActivePageState(ActivePageState.MainPage);
        }
        private void ExecuteMinMode(object obj)
        {
            int[] index = { 1, 0 };
            TcpClientService.SetCommandToServer(308, index);
            CActivePagesEntities.SetActivePageState(ActivePageState.MainPage);
        }

        private void ExecuteNormal(object obj)
        {
            int[] index = { 2, 0 };
            TcpClientService.SetCommandToServer(308, index);
            CActivePagesEntities.SetActivePageState(ActivePageState.MainPage);
        }

        private void ExecuteMaxMode(object obj)
        {
            int[] index = { 3, 0 };
            TcpClientService.SetCommandToServer(308, index);
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
            TcpClientService.SetCommandToServer(309, index);
            CActivePagesEntities.SetActivePageState(ActivePageState.MainPage);
        }

        private void ExecuteSheduler(object obj)
        {
            int[] index = { 3 };
            TcpClientService.SetCommandToServer(309, index);
            CActivePagesEntities.SetActivePageState(ActivePageState.LoadingPage);
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
            if (CModesEntities.Mode2ValuesList[1].TimeModeValues[0].Minute < 600)
            {
                CModesEntities.Mode2ValuesList[1].TimeModeValues[0].Minute += 10;
            }
            else
            {
                CModesEntities.Mode2ValuesList[1].TimeModeValues[0].Minute = 600;
            }
        }

        private void ExecuteKitchenCancel(object obj)
        {
            int[] values = { CModesEntities.Mode2ValuesList[1].TimeModeValues[0].Minute, 0, 1 };
            TcpClientService.SetCommandToServer(334, values);
            CActivePagesEntities.SetActivePageState(ActivePageState.MainPage); ;
        }

        private void ExecuteKitchenOk(object obj)
        {
            int[] values = { CModesEntities.Mode2ValuesList[1].TimeModeValues[0].Minute, 1, 0 };
            TcpClientService.SetCommandToServer(334, values);
            CActivePagesEntities.SetActivePageState(ActivePageState.MainPage);
        }
        #endregion

        #region SetPoints execute commands

        private void ExecuteBtnUP0(object obj)
        {

            M1Values.SypplySP = M1Values.SypplySP + 5 < 100 ? M1Values.SypplySP + 5 : 100;
        }
        private void ExecuteBtnDn0(object obj)
        {
            M1Values.SypplySP = M1Values.SypplySP - 5 > 0 ? M1Values.SypplySP - 5 : 0;
        }

        private void ExecuteBtnUP1(object obj)
        {
            M1Values.ExhaustSP = M1Values.ExhaustSP + 5 < 100 ? M1Values.ExhaustSP + 5 : 100;
        }
        private void ExecuteBtnDn1(object obj)
        {
            M1Values.ExhaustSP = M1Values.ExhaustSP - 5 > 0 ? M1Values.ExhaustSP - 5 : 0;
        }

        private void ExecuteBtnUP2(object obj)
        {
            M1Values.TempSP = M1Values.TempSP + 1 < 35 ? M1Values.TempSP + 1 : 35;
            if (M1Values.TempSP < 16) M1Values.TempSP = 16;
        }
        private void ExecuteBtnDn2(object obj)
        {
            M1Values.TempSP = M1Values.TempSP - 1 > 16 ? M1Values.TempSP - 1 : 16;
        }

        private void ExecuteBtnUP3(object obj)
        {
            M1Values.PowerLimitSP = M1Values.PowerLimitSP + 5 < 100 ? M1Values.PowerLimitSP + 5 : 100;
        }
        private void ExecuteBtnDn3(object obj)
        {
            M1Values.PowerLimitSP = M1Values.PowerLimitSP - 5 > 0 ? M1Values.PowerLimitSP - 5 : 0;
        }

        private void ExecuteSPSOK(object obj)
        {
            int[] values = { M1Values.SypplySP, M1Values.ExhaustSP, M1Values.TempSP, M1Values.PowerLimitSP };
            TcpClientService.SetCommandToServer(M1Values.StartAddress, values);
            CActivePagesEntities.SetActivePageState(ActivePageState.MainPage);
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
            index = index > 0 ? index : 1;

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
        #endregion

        #region Settings execute methods
        private void ExecuteJournal(object obj)
        {
            CActivePagesEntities.SetActivePageState(ActivePageState.JournalPage);    
        }
        #endregion


        Timer timer;
        private void StartTimer()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                timer = new Timer(obj =>
                {
                    MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        EthernetEntities.SystemMessage = $"Счетчик принятий ={TcpClientService.ResieveCounter}";
                    });
                },
                null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
            });
        }
    }
}