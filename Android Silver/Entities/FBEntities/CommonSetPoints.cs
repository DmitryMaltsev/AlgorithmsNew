using Android_Silver.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Android_Silver.Entities.FBEntities
{
    public class CommonSetPoints:BindableBase
    {

        private float _spTemp;
        public float SPTempR
        {
            get { return _spTemp; }
            set
            {
                _spTemp = value;
                OnPropertyChanged(nameof(SPTempR));
            }
        }
        public float  SPTempAlarm;
        public float SPTempMaxCh;
        public float SPTempMinCh;
        public int TControlDelayS;
        public int SeasonMode;
        public float SPSeason;
        public float HystSeason;
    }
}
