using Android_Silver.ViewModels;

using System.ComponentModel;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Android_Silver.Entities
{
    public enum MessageStates
    {
        UserMessage,
        VacMessage,
        ShedMessage1,
        ShedMessage2,
        ServiceMessage1,
        ServiceMessage2
    }


    public class EthernetEntities : BindableBase
    {
        public int PagesTab;
        public MessageStates CMessageState;

        public TcpClient Client { get; set; }

        private string _connectIP = "192.168.0.134";
        public string ConnectIP
        {
            get { return _connectIP; }
            set
            {
                _connectIP = value;
                OnPropertyChanged(nameof(ConnectIP));
            }
        }

        private byte _ip1 = 192;
        public byte IP1
        {
            get { return _ip1; }
            set
            {
                _ip1 = value;
                OnPropertyChanged(nameof(IP1));
            }
        }

        private byte _ip2 = 168;
        public byte IP2
        {
            get { return _ip2; }
            set
            {
                _ip2 = value;
                OnPropertyChanged(nameof(IP2));
            }
        }
        private byte _ip3 = 0;
        public byte IP3
        {
            get { return _ip3; }
            set
            {
                _ip3 = value;
                OnPropertyChanged(nameof(IP3));
            }
        }

        private byte _ip4 = 105;
        public byte IP4
        {
            get { return _ip4; }
            set
            {
                _ip4 = value;
                OnPropertyChanged(nameof(IP4));
            }
        }
        private string _ethernetMessage = "Введите IP и нажмите подтвердить";
        public string EthernetMessage
        {
            get { return _ethernetMessage; }
            set
            {
                _ethernetMessage = value;
                OnPropertyChanged(nameof(EthernetMessage));
            }
        }

        private int _connectPort = 8887;
        public int ConnectPort
        {
            get { return _connectPort; }
            set
            {
                _connectPort = value;
                OnPropertyChanged(nameof(_connectPort));
            }
        }


        private string _subnet = String.Empty;
        public string Subnet
        {
            get { return _subnet; }
            set
            {
                _subnet = value;
                OnPropertyChanged(nameof(Subnet));
            }
        }

        private string _gateWay = String.Empty;
        public string GateWay
        {
            get { return _gateWay; }
            set
            {
                _gateWay = value;
                OnPropertyChanged(nameof(GateWay));
            }
        }

        private bool _isConnected;

        public bool IsConnected
        {
            get { return _isConnected; }
            set
            {
                if (_isConnected != value)
                {
                    _isConnected = value;
                    OnPropertyChanged(nameof(IsConnected));
                }
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

        private string _packetsMessage;
        public string PacketsMessage
        {
            get { return _packetsMessage; }
            set
            {
                _packetsMessage = value;
                OnPropertyChanged(nameof(PacketsMessage));
            }
        }

        public Response ResponseValue { get; set; }



        private bool _loaded = false;

        public bool Loaded
        {
            get { return _loaded; }
            set
            {
                _loaded = value;
                OnPropertyChanged(nameof(Loaded));
            }
        }

        public bool WriteMessageSended { get; set; }

        private bool IsValidIp(string ipAddress)
        {
            // Проверка на допустимый формат IPv4
            var regex = new Regex(@"^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|
                                                                                [01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$");
            return regex.IsMatch(ipAddress);
        }

        public void WifiStateChangeCallback(bool wifiState)
        {
            if (wifiState)
            {
                EthernetMessage = "Введите IP и нажмите подтвердить";
            }
            else
            {
                EthernetMessage = "Для работы приложения требуется включить Wi-Fi и " +
                                                    "соединиться с модемом";
            }
        }
    }
}
