using Android_Silver.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Android_Silver.Entities.MQTT
{
    public class MQTTEntity:BindableBase
    {
        public int DeviceID { get; set; }
        public int Tag { get; set; }

        public int MQTTInited;
        public string server { get; set; }
        public int port { get; set; }
        public byte CRC { get; set; }
        private string _token { get; set; } = "gTKZKsSHJeTdbmEWOpNEF4Ntk81YJf4Hhtv2LaYUBxkTcocCzEWM4MQc";
        public string Token
        {
            get{ return _token;}
            set { _token = value;
                OnPropertyChanged(nameof(Token));
            }
        }

        public string user { get; set; }    
        public string password { get; set; }
        public MQTTRoot Root { get; set; }
        public MQTTEntity()
        {
            Root = new MQTTRoot();
        }

    }
}
