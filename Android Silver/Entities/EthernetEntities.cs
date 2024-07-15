using Android_Silver.ViewModels;

using System.ComponentModel;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace Android_Silver.Entities
{
    public class EthernetEntities : BindableBase
    {

        public TcpClient Client { get; set; }

        private string _connectIP = "192.168.0.11";
        public string ConnectIP
        {
            get { return _connectIP; }
            set
            {
                _connectIP = value;
                OnPropertyChanged(nameof(ConnectIP));
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

        private string _ip = String.Empty;
        public string IP
        {
            get { return _ip; }
            set
            {
                _ip = value;
                OnPropertyChanged(nameof(IP));
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
            set {
                if(_isConnected!=value)
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
            set { 
                _systemMessage = value;
                OnPropertyChanged(nameof(SystemMessage));
            }
        }



        public Response ResponseValue { get; set; }



        private bool _loaded=false;

        public bool Loaded
        {
            get { return _loaded; }
            set {
                _loaded = value;
                OnPropertyChanged(nameof(Loaded));
            }
        }

        public bool WriteMessageSended { get; set; }
    }
}
