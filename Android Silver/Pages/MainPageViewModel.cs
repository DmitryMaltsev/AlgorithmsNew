using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Android_Silver.Entities;

using Microsoft.Maui.Controls;



namespace Android_Silver.Pages
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
  
        #region Rising properties
        private string _serverIP = "192.168.0.14";
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
        public SensorsEntities CSensorsEntities { get; set; } = new();

        TcpClient _tcpClient;
        NetworkStream _stream;
        int counter = 0;
        string _cData;
        public MainPageViewModel()
        {
            EthernetEntities= DIContainer.Resolve<IEthernetEntities>();
            CSensorsEntities=DIContainer.Resolve<SensorsEntities>();
            ConnectCommand = new Command(ExecuteConnect);
            SendCommand = new Command(ExecuteSendData);
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
            SystemMessage = "";
            SendMessageToClient = "100,08";
            sendBufTask = new Task(() => SendBuffer(SendMessageToClient));
            sendBufTask.Start();
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
                    byte[] data = new byte[100];
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
                            SystemMessage = $"Получены данные {responseData}";
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
        #endregion
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

        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        public event PropertyChangedEventHandler PropertyChanged;



      /*  private void Timer()
        {
            var wifiManager = (WifiManager)Android.App.Application.Context.GetSystemService(Context.WifiService);

            MainThread.BeginInvokeOnMainThread(() =>
            {
                timer = new System.Threading.Timer(obj =>
                {
                    MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        wifiManager.StartScan();
                        if (wifiManager.ScanResults.Count > 0)
                        {
                            foreach (var item in wifiManager.ScanResults)
                            {
                                AnroidWIFIEntity.StrScans.Add(item.Bssid);
                            }

                        }

                    });
                },
                null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
            });
        }*/

    }
}
