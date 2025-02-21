using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace BlueToothThm.ViewModels
{
    public class MainPageViewModel : BindableBase
    {
        private int _value = 125;
        public int Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged(nameof(Value));
            }
        }
    }
}
