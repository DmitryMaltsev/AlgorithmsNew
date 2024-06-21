using Android_Silver.Entities;
using Android_Silver.Services;
using Android_Silver.ViewModels;

using System.ComponentModel;
using System.Globalization;
using System.Net.Sockets;
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
        public ICommand ConnectCommand { get; private set; }
        public ICommand DisconnectCommand { get; private set; }
        public ICommand GetIPCommand { get; private set; }
        public ICommand SettingsCommand { get; private set; }
        public ICommand SendSPCommand { get; private set; }
        public ICommand SendFloatCommand { get; private set; }
        public ICommand SetSettingsCommand { get; private set; }
        public ICommand ChooseModeCommand { get; private set; }

        // public ICommand SettingsCommand { get; private set; }
        #endregion

        public IEthernetEntities EthernetEntities { get; set; }
        public SensorsEntities CSensorsEntities { get; set; }

        public ITcpClientService TcpClientService { get; set; }


        private ModesEntities _cModesEntities;

        public ModesEntities CModesEntities { get; set; }



        public SetPoints CSetPoints { get; set; }



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
            ConnectCommand = new Command(ExecuteConnect);
            DisconnectCommand = new Command(ExecuteDisconnect);
            SendSPCommand = new Command(ExecuteSendSP);
            SendFloatCommand = new Command(ExecuteSendFloat);
            SettingsCommand = new Command(ExecuiteSettings);
            ChooseModeCommand = new Command(ExecuteChooseMode);
            SetSettingsCommand = new Command(ExecuteSetSettings);
            Value = 15;
            StartTimer();
        }

        async void ExecuteSetSettings(object obj)
        {
            await Shell.Current.GoToAsync(CModesEntities.CModeSettingsRoute);
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

        #region Execute methods
        int val = 135;

        private void ExecuteDisconnect()
        {
            TcpClientService.Disconnect();
        }
        private Task sendBufTask;


        private void ExecuteSendSP(object obj)
        {
            EthernetEntities.MessageToSend = $"300,01,{(int)(CSetPoints.SetPoint1)}";
        }

        private void ExecuteSendFloat(object obj)
        {
            EthernetEntities.MessageToSend = $"303,01,{(int)(CSetPoints.SetPointF * 10)}";

        }


        async private void ExecuiteSettings(object obj)
        {
            await Shell.Current.GoToAsync("settingsPage");
        }

        async private void ExecuteChooseMode(object obj)
        {
            await Shell.Current.GoToAsync("chooseModePage");
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