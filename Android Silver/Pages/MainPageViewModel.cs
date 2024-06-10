using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Android_Silver.Pages
{
    public class MainPageViewModel : INotifyPropertyChanged
    {

        private string _systemMessage="Проверка";
        public string SystemMessage
        {
            get { return _systemMessage; }
            set
            {
                _systemMessage = value;
                OnPropertyChanged(nameof(SystemMessage));
            }
        }

        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
