using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
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
        private string _serverIP = "192.168.0.225";
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

        private int _recieveCounter;

        public int RecieveCounter
        {
            get { return _recieveCounter; }
            set
            {
                _recieveCounter = value;
                OnPropertyChanged(nameof(RecieveCounter));
            }
        }
        private string _sRecieveCounter;

        public string SRecieveCounter
        {
            get { return _sRecieveCounter; }
            set { 
                _sRecieveCounter = value;
                OnPropertyChanged(nameof(SRecieveCounter));
            }
        }


        public IEthernetEntities EthernetEntities { get; set; }
        #endregion

        public SensorsEntities CSensorsEntities { get; set; } = new();
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
                        ExecuteSendData();
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
            DispatcherTimer t = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(200) };
            //SendRecieveTask("");
            t.Tick += T_Tick;
            t.Start();
        }
        string _cData;
        int counter = 0;

        private void T_Tick(object? sender, EventArgs e)
        {
            SRecieveCounter = RecieveCounter.ToString();
        }



        #region Execute methods
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
                SendRecieveTask("");
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



        int val = 135;

        private void ExecuteDisconnect()
        {
            SendIsActive = false;
            Connected = false;
            SystemMessage = "Соединение разорвано";
            _tcpClient.Close();
            _tcpClient.Dispose();
        }

        private Task sendBufTask;
        void ExecuteSendData()
        {
            SendMessageToClient = "103,05";
            sendBufTask = new Task(() => SendBuffer(SendMessageToClient));
            sendBufTask.Start();
        }

        StringBuilder sbResult = new StringBuilder();
        public void SendRecieveTask(string val)
        {
            string mess = String.Empty;
            for (int i = 0; i < 256; i++)
            {
                mess += 6;
            }
            Task.Run(() =>
            {
                while (Connected)
                {
                    sbResult = CyclicSendCommand(mess);
             
                    Task.Delay(10);
                }

            });
        }
        private StringBuilder CyclicSendCommand(string command)
        {

            bool IsSending = true;
            StringBuilder sbResult = new StringBuilder();
            int _trySendcounter = 0;
            do
            {
                try
                {
                    _stream = _tcpClient.GetStream();
                    StreamWriter writer = new StreamWriter(_stream, Encoding.ASCII);
                    writer.WriteLine(command);
                    writer.Flush();
                    byte[] data = new byte[700];
                    int bytes = _stream.Read(data, 0, data.Length);
                    do
                    {
                        sbResult.Append(Encoding.ASCII.GetString(data, 0, bytes));
                    }
                    while (_stream.DataAvailable);
                    IsSending = false;
                    RecieveCounter += 1;
                }
                catch (Exception ex)
                {
                    _trySendcounter += 1;
                    Task.Delay(5);
                    SystemMessage = $"количество попыток {_trySendcounter}";
                }
                // && _ethernetEntities.MessageToServer==String.Empty
            }
            while (IsSending && _trySendcounter < 10 && MessageToServer == String.Empty);
            IsSending = false;
            if (_trySendcounter == 10)
            {
                if (Connected == true)
                {
                    SystemMessage = "Превышено максимальное количество попыток - 10";
                    Connected = false;
                }
                else
                {
                    Connected = false;
                }
            }
            return sbResult;
        }

        private void SendBuffer(string command)
        {
            SendIsActive = false;
            do
            {
                try
                {
                    _stream = _tcpClient.GetStream();
                    StreamWriter writer = new StreamWriter(_stream, Encoding.
                        ASCII);
                    writer.WriteLine(command);
                    writer.Flush();
                    _cData = command.ToString();
                    byte[] data = new byte[250];
                    StringBuilder responseData = new StringBuilder();
                    int bytes = _stream.Read(data, 0, data.Length);
                    do
                    {
                        responseData.Append(Encoding.ASCII.GetString(data, 0, bytes));
                    }
                    while (_stream.DataAvailable);
                    if (responseData.Length > 1)
                    {
                        List<Response> responseList = new();
                        if (GetResponseData(responseData, responseList))
                        {
                            foreach (var response in responseList)
                            {
                                GetValueByTag(response);
                            }

                            SystemMessage = $"Получены данные{responseData}";
                            SendIsActive = true;
                        }
                    }
                    else
                    {
                        SystemMessage = "Данных не получено";
                    }
                }
                catch (Exception ex)
                {
                    SystemMessage = ex.Message;
                }
                Task.Delay(200);
            }
            while (!SendIsActive);
        }

        private bool GetResponseData(StringBuilder rSB, List<Response> response)
        {
            bool isRightResponse = true;
            string[] resultVals = rSB.ToString().Split(",");
            ushort startTag = ushort.Parse(resultVals[0]);

            for (ushort i = 2; i < resultVals.Length; i++)
            {
                response.Add(new Response() { Tag = (ushort)(startTag + i - 2), ValueString = resultVals[i] });
            }
            return isRightResponse;
        }

        void GetValueByTag(Response resp)
        {
            switch (resp.Tag)
            {
                case 100:
                    {
                        EthernetEntities.IP = resp.ValueString;
                    }
                    break;
                case 101:
                    {
                        EthernetEntities.Subnet = resp.ValueString;
                    }
                    break;
                case 102:
                    {
                        EthernetEntities.GateWay = resp.ValueString;
                    }
                    break;
                case 103:
                    {
                        CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                        ci.NumberFormat.CurrencyDecimalSeparator = ".";
                        CSensorsEntities.OutdoorTemp = float.Parse(resp.ValueString, NumberStyles.Any, ci);
                    }
                    break;
                case 104:
                    {
                        CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                        ci.NumberFormat.CurrencyDecimalSeparator = ".";
                        CSensorsEntities.SupplyTemp = float.Parse(resp.ValueString, NumberStyles.Any, ci);
                    }
                    break;
                case 105:
                    {
                        CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                        ci.NumberFormat.CurrencyDecimalSeparator = ".";
                        CSensorsEntities.ExhaustTemp = float.Parse(resp.ValueString, NumberStyles.Any, ci);
                    }
                    break;
                case 106:
                    {
                        CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                        ci.NumberFormat.CurrencyDecimalSeparator = ".";
                        CSensorsEntities.RoomTemp = float.Parse(resp.ValueString, NumberStyles.Any, ci);
                    }
                    break;
                case 107:
                    {
                        CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                        ci.NumberFormat.CurrencyDecimalSeparator = ".";
                        CSensorsEntities.ReturnWaterTemp = float.Parse(resp.ValueString, NumberStyles.Any, ci);
                    }
                    break;
            }
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
