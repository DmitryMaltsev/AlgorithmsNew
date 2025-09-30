using Android_Silver.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Android_Silver.Entities.FBEntities
{
    public class EHSetPoints:BindableBase
    {
        private int _cPower;
        public int CPower
        {
            get { return _cPower; }
            set
            {
                _cPower = value;
                OnPropertyChanged(nameof(CPower));
            }
        }
        public int NomPowerVT;
        public int PReg;
        public int IReg;
        public int DReg;
        public int BlowDownTime;
    }
}
