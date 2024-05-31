using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using TCPTest.Entities;


namespace TCPTest
{
    public class TCPTestViewModel : INotifyPropertyChanged
    {
        #region Rising properties
        private string _serverIP = "192.168.0.12";
        public string ServerIP
        {
            get
            {
                return _serverIP;
            }
            set
            {
                _serverIP = value;
                OnPropertyChanged(nameof(ServerIP));
            }
        }


        private int _port = 8887;

        public int Port
        {
            get { return _port; }
            set
            {
                _port = value;
                OnPropertyChanged(nameof(Port));
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

        private string _systemMessage;

        public string SystemMessage
        {
            get { return _systemMessage; }
            set
            {
                _systemMessage = value;
                OnPropertyChanged(nameof(SystemMessage));
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

        public IEthernetEntities EthernetEntities { get; set; }
        #endregion

        #region Send commands
        private RelayCommand _connectCommand;
        public RelayCommand ConnectCommand
        {
            get
            {
                return _connectCommand ??
                    (_connectCommand = new RelayCommand(obj =>
                    {
                        Task.Run(() => ExecuteConnect());
                    }));
            }
        }

        private RelayCommand _sendCommand;
        public RelayCommand SendCommand
        {
            get
            {
                return _sendCommand ??
                    (_sendCommand = new RelayCommand(obj =>
                    {
                        Task.Run(() => ExecuteSendData());
                    }));
            }
        }

        private RelayCommand _disconnectCommand;
        public RelayCommand DisconnectCommand
        {
            get
            {
                return _disconnectCommand ??
                  (_disconnectCommand = new RelayCommand(obj =>
                  {
                      ExecuteDisconnect();
                  }));

            }
        }


        private RelayCommand _getIPCommand;
        public RelayCommand GetIPCommand
        {
            get
            {
                return _getIPCommand ??
                    (_getIPCommand = new RelayCommand(obj =>
                    {
                        ExecuteGetIPCommand();
                    }));
            }

        }


        private RelayCommand _getSubnetCommand;
        public RelayCommand GetSubnetCommand
        {
            get
            {
              return _getSubnetCommand ??
                    (_getSubnetCommand = new RelayCommand(obj =>
            {
                ExecuteGetSubnetCommand();
            }));
            }
        }

        private RelayCommand _getGatewayCommand;
        public RelayCommand GetGatewayCommand
        {
            get {
                return _getGatewayCommand ??
                    (_getGatewayCommand = new RelayCommand(obj =>
                    {
                        ExecuteGetGatewayCommand();
                    }));
                }
        }
        #endregion

        TcpClient _tcpClient;
        NetworkStream _stream;

        public TCPTestViewModel()
        {
            DIContainer.RegisterDependencies();
            EthernetEntities = DIContainer.Resolve<IEthernetEntities>();

            //   Task.Run(() => ExecuteSendData());
            DispatcherTimer t = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(100) };
            t.Tick += T_Tick;
            t.Start();
        }
        string _cData;
        int counter = 0;
        private void T_Tick(object? sender, EventArgs e)
        {
            // EthernetEntities.IP = "aseasd"+ counter;
            counter += 1;
            if (_cData != String.Empty)
            {
                MessageToClient = _cData;
            }
        }

        public void ExecuteConnect()
        {
            try
            {
                _tcpClient = new TcpClient();
                _tcpClient.ReceiveTimeout = 3000;
                _tcpClient.SendTimeout = 3000;
                SendIsActive = false;
                Connected = true;
                SystemMessage = "Попытка подключения";
                _tcpClient.Connect(ServerIP, Port);
                _tcpClient.ReceiveTimeout = 3000;
                SystemMessage = "Подключение прошло успешно";
                SendIsActive = true;
                Connected = true;
            }
            catch (Exception ex)
            {
                SendIsActive = false;
                Connected = false;
                SystemMessage = ex.Message;
                _tcpClient.Close();
                _tcpClient.Dispose();
            }
        }

        #region Execute methods
        int val = 135;
        private void ExecuteSendData()
        {
            SendIsActive = false;
            do
            {
                try
                {
                    _stream = _tcpClient.GetStream();
                    StreamWriter writer = new StreamWriter(_stream, Encoding.ASCII);
                    writer.WriteLine(val.ToString());
                    writer.Flush();
                    _cData = val.ToString();
                    byte[] data = new byte[20];
                    StringBuilder responseData = new StringBuilder();
                    int bytes = _stream.Read(data, 0, data.Length);
                    do
                    {
                        responseData.Append(Encoding.ASCII.GetString(data, 0, bytes));
                    }
                    while (_stream.DataAvailable);
                    if (responseData.ToString().Contains("OK/n"))
                    {
                        SendIsActive = true;
                        SystemMessage = $"Успешная передача {val}";
                        val += 1;
                    }
                }
                catch (Exception ex)
                {
                    SystemMessage = ex.Message;
                }
                Task.Delay(300);
            }
            while (!SendIsActive);
        }

        private void ExecuteDisconnect()
        {
            SendIsActive = false;
            Connected = false;
            SystemMessage = "Соединение разорвано";
            _tcpClient.Close();
            _tcpClient.Dispose();
        }

        private void ExecuteGetIPCommand()
        {
           Task.Run(()=>SendBuffer("GetIP"));
        }

        private void ExecuteGetSubnetCommand()
        {
           
        }

        private void ExecuteGetGatewayCommand()
        {
           
        }


        private void SendBuffer(string command)
        {
            SendIsActive = false;
            do
            {
                try
                {
                    _stream = _tcpClient.GetStream();
                    StreamWriter writer = new StreamWriter(_stream, Encoding.ASCII);
                    writer.WriteLine(command);
                    writer.Flush();
                    _cData = command.ToString();
                    byte[] data = new byte[20];
                    StringBuilder responseData = new StringBuilder();
                    int bytes = _stream.Read(data, 0, data.Length);
                    do
                    {
                        responseData.Append(Encoding.ASCII.GetString(data, 0, bytes));
                    }
                    while (_stream.DataAvailable);
                    if (responseData.ToString().Length>0)
                    {
                        EthernetEntities.IP = responseData.ToString();
                        SendIsActive = true;
                        SystemMessage = $"Успешная передача {val}";
                        val += 1;
                    }
                }
                catch (Exception ex)
                {
                    SystemMessage = ex.Message;
                }
                Task.Delay(300);
            }
            while (!SendIsActive);
        }
        #endregion

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
