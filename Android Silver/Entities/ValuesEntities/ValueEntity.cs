using Android_Silver.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Android_Silver.Entities.ValuesEntities
{
    public class ValueEntity:BindableBase
    {
        private int _min;
        public int Min
        {
            get { return _min; }
            set { 
                _min = value;
                OnPropertyChanged(nameof(Min));
            }
        }

        private int _max;
        public int Max
        {
            get { return _max; }
            set { 
                _max = value;
                OnPropertyChanged(nameof(Max));
            }
        }

        private byte _numChr;

        public byte NumChr
        {
            get { return _numChr; }
            set { 
                _numChr = value; 
                OnPropertyChanged($"{nameof(NumChr)}");
            }
        }




    }
}
