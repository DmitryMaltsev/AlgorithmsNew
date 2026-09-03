using Android_Silver.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Android_Silver.Entities.FBEntities
{
    public class FreonCoolerSP:BindableBase
    {
        private ushort _valPerc;

        public ushort ValPerc
        {
            get { return _valPerc; }
            set { 
                _valPerc = value; 
                OnPropertyChanged(nameof(ValPerc));
            }
        }

        public int Stage1OnS;
        public int Stage1OffS;
        public int PReg;
        public int IReg;
        public int DReg;
        public int Hyst;
    }
}
