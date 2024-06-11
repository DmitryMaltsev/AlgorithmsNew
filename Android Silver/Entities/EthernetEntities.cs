using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Android_Silver.Entities
{
    public class EthernetEntities : INotifyPropertyChanged, IEthernetEntities
    {
        private string _connectIP = "192.168.0.14";
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


        private string _ip=String.Empty;
        public string IP
        {
            get { return _ip; }
            set { _ip = value;
                OnPropertyChanged(nameof(IP));
            }
        }

        private string _subnet=String.Empty;
        public string Subnet
        {
            get { return _subnet; }
            set { _subnet = value;
                OnPropertyChanged(nameof(Subnet));
            }
        }

        private string _gateWay = String.Empty;
        public string GateWay
        {
            get { return _gateWay; }
            set { _gateWay = value;
                OnPropertyChanged(nameof(GateWay));
            }
        }

        public Response ResponseValue { get ; set ; }

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
