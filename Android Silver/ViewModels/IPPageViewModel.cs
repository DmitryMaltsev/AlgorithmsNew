using Android_Silver.Entities;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Android_Silver.Pages
{
    public class IPPageViewModel
        : INotifyPropertyChanged
    {
        public EthernetEntities EthernetEntities { get; set; }



        public IPPageViewModel()
        {
            EthernetEntities = DIContainer.Resolve<EthernetEntities>();
        }






        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
