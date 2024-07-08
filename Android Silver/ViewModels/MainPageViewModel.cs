using Android_Silver.Entities;
using Android_Silver.Entities.Modes;
using Android_Silver.Entities.Visual;
using Android_Silver.Services;
using Android_Silver.ViewModels;

using System;
using System.ComponentModel;
using System.Globalization;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
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

        #endregion

        #region Commands
        #region MainPageCommands
        public ICommand ConnectCommand { get; private set; }
        public ICommand DisconnectCommand { get; private set; }
        public ICommand GetIPCommand { get; private set; }
        public ICommand SettingsCommand { get; private set; }
        public ICommand SendSPCommand { get; private set; }
        public ICommand SendFloatCommand { get; private set; }
        public ICommand SetSettingsCommand { get; private set; }
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
        public ICommand OkCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        #endregion

        // public ICommand SettingsCommand { get; private set; }
        #endregion

        public IEthernetEntities EthernetEntities { get; set; }

        public SensorsEntities CSensorsEntities { get; set; }

        public ITcpClientService TcpClientService { get; set; }


        public ModesEntities CModesEntities { get; set; }

        public PicturesSet CPictureSet { get; set; }

        public SetPoints CSetPoints { get; set; }

        public ActivePagesEntities CActivePagesEntities { get; set; }

        private ICommand UpdateRouting;

        NetworkStream _stream;
        int counter = 0;
        string _cData;
        public MainPageViewModel()
        {

            EthernetEntities = DIContainer.Resolve<IEthernetEntities>();
            CSensorsEntities = DIContainer.Resolve<SensorsEntities>();
            TcpClientService = DIContainer.Resolve<ITcpClientService>();
            CSetPoints = DIContainer.Resolve<SetPoints>();
            CModesEntities = DIContainer.Resolve<ModesEntities>();
            CActivePagesEntities = DIContainer.Resolve<ActivePagesEntities>();
            CPictureSet = DIContainer.Resolve<PicturesSet>();

            ConnectCommand = new Command(ExecuteConnect);
            DisconnectCommand = new Command(ExecuteDisconnect);
            SendSPCommand = new Command(ExecuteSendSP);
            SendFloatCommand = new Command(ExecuteSendFloat);
            SettingsCommand = new Command(ExecuiteSettings);
            ChooseModeCommand = new Command(ExecuteChooseMode);
            SetSettingsCommand = new Command(ExecuteSetSettings);
            UpdateRouting = new Command(ExecuteUpdateRouting);
            Value = 15;
            CActivePagesEntities.SetActivePageState(ActivePageState.MainPage);

            MinModeCommand = new Command(ExecuteMinMode);
            NormalModeCommand = new Command(ExecuteNormal);
            MaxModeCommand = new Command(ExecuteMaxMode);
            KitchenModeCommand = new Command(ExecuteKitchenMode);
            ShedulerModeCommand = new Command(ExecuteSheduler);
            VacationModeCommand = new Command(ExecuteVacationMode);
            TurnOffModeCommand = new Command(ExecuteTurnOffMode);
            HomeCommand = new Command(ExecuteHomeCommand);
            // StartTimer();
            #region Kitchen timer commands
            UpMInutesCommand = new Command(ExecuteUpMinutes);
            DnMinutesCommand = new Command(ExecuteDnMinutes);
            HomeCommand = new Command(ExecuteHomeCommand);
            OkCommand = new Command(ExecuteOk);
            CancelCommand = new Command(ExecuteCancel); 
            #endregion

        }


        async void ExecuteUpdateRouting(object obj)
        {

            //  await Shell.Current.Navigation.PopToRootAsync(false);


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
                    TcpClientService.SendRecieveTask("108,26");
                }
            }
            else
            {
                EthernetEntities.SystemMessage = "В данный момент подключаемся";
            }
        }

        #region Main page execute methods
        void ExecuteSetSettings(object obj)
        {
            CActivePagesEntities.SetActivePageState(ActivePageState.ChooseModePage);
            // await Shell.Current.GoToAsync("setPointsPage", false);
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

        async private void ExecuiteSettings(object obj)
        {
            await Shell.Current.GoToAsync("settingsPage");
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
            CActivePagesEntities.SetActivePageState(ActivePageState.LoadingPage);
        }
        private void ExecuteMinMode(object obj)
        {
            int[] index = { 1, 0 };
            TcpClientService.SetCommandToServer(308, index);
            CActivePagesEntities.SetActivePageState(ActivePageState.LoadingPage);
        }

        private void ExecuteNormal(object obj)
        {
            int[] index = { 2, 0 };
            TcpClientService.SetCommandToServer(308, index);
            CActivePagesEntities.SetActivePageState(ActivePageState.LoadingPage);
        }

        private void ExecuteMaxMode(object obj)
        {
            int[] index = { 3, 0 };
            TcpClientService.SetCommandToServer(308, index);
            CActivePagesEntities.SetActivePageState(ActivePageState.LoadingPage);
        }

        private void ExecuteKitchenMode(object obj)
        {
              int[] index = { 1 };
              TcpClientService.SetCommandToServer(309, index);
            CActivePagesEntities.SetActivePageState(ActivePageState.KithchenTimerPage);
        }

        private void ExecuteVacationMode(object obj)
        {
            int[] index = { 2 };
            TcpClientService.SetCommandToServer(309, index);
            CActivePagesEntities.SetActivePageState(ActivePageState.LoadingPage);
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

            if (CModesEntities.Mode2ValuesList[1].TimeModeValues[0].Hour >= 10)
            {
                CModesEntities.Mode2ValuesList[1].TimeModeValues[0].Hour -= 10;
            }
            else
            {
                CModesEntities.Mode2ValuesList[1].TimeModeValues[0].Hour = 0;
            }
        }

        private void ExecuteUpMinutes(object obj)
        {
            if (CModesEntities.Mode2ValuesList[1].TimeModeValues[0].Hour < 600)
            {
                CModesEntities.Mode2ValuesList[1].TimeModeValues[0].Hour += 10;
            }
            else
            {
                CModesEntities.Mode2ValuesList[1].TimeModeValues[0].Hour = 600;
            }
        }

        async void ExecuteCancel(object obj)
        {
            await Shell.Current.GoToAsync("mainPage");
        }

        private void ExecuteOk(object obj)
        {
            int[] values = { CModesEntities.Mode2ValuesList[1].TimeModeValues[0].Hour };
            TcpClientService.SetCommandToServer(334, values);
            CActivePagesEntities.SetActivePageState(ActivePageState.LoadingPage);;
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