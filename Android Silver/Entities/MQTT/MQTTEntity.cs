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
      
        public string Address { get; set; }
        public int Port { get; set; }

        private string _token { get; set; } = "gTKZKsSHJeTdbmEWOpNEF4Ntk81YJf4Hhtv2LaYUBxkTcocCzEWM4MQc";

        public string Token
        {
            get{ return _token;}
            set { _token = value;
                OnPropertyChanged(nameof(Token));
            }
        }

        public string User { get; set; }    
        public string Password { get; set; }
        public MQTTRoot Root { get; set; }
        public MQTTEntity()
        {
            Root = new MQTTRoot();
        }

    }
}
