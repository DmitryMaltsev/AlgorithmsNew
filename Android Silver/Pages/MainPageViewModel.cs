using Android_Silver.Entities;
using Android_Silver.Services;

using System.ComponentModel;
using System.Globalization;
using System.Net.Sockets;
using System.Text;
using System.Windows.Input;
namespace Android_Silver.Pages
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        #region Rising properties
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

        #region Send commands
        public ICommand ConnectCommand { get; private set; }
        public ICommand SendCommand { get; private set; }
        public ICommand DisconnectCommand { get; private set; }
        public ICommand GetIPCommand { get; private set; }
        #endregion

        public IEthernetEntities EthernetEntities { get; set; }
        public SensorsEntities CSensorsEntities { get; set; }

        public ITcpClientService TcpClientService { get; set; }

        NetworkStream _stream;
        int counter = 0;
        string _cData;
        public MainPageViewModel()
        {
            EthernetEntities = DIContainer.Resolve<IEthernetEntities>();
            CSensorsEntities = DIContainer.Resolve<SensorsEntities>();
            TcpClientService = DIContainer.Resolve<ITcpClientService>();
            
            ConnectCommand = new Command(ExecuteConnect);
            SendCommand = new Command(ExecuteSendData);
            DisconnectCommand = new Command(ExecuteDisconnect);
            StartTimer();
        }

        async private void ExecuteConnect()
        {
            EthernetEntities.SystemMessage = "Check";
            if (!TcpClientService.IsConnecting)
            {
                await TcpClientService.Connect();
                if (EthernetEntities.IsConnected)
                {
                    TcpClientService.RecieveData("100,08");
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

         void ExecuteSendData()
        {
            EthernetEntities.SystemMessage = "";
           TcpClientService.SendData("300,03,-15,-20,-30");
        }
        #endregion


        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
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