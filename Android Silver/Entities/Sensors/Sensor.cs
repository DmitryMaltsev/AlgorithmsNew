using Android_Silver.Entities.ValuesEntities;
using Android_Silver.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Android_Silver.Entities.Srs
{
    public class Sensor:BindableBase
    {

        private FloatValue _value;
        public FloatValue Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                OnPropertyChanged(nameof(Value));
            }
        }

        public Sensor(int min, int max, byte numChr){
            Value = new FloatValue(min, max, numChr);
        }
    }
}
