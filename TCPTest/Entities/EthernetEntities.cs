using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TCPTest.Entities
{
    public class EthernetEntities : INotifyPropertyChanged, IEthernetEntities
    {
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
