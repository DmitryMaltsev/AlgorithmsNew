using Android_Silver.Entities.ValuesEntities;
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

        private FloatValue _spTemp;
        public FloatValue SPTempR
        {
            get { return _spTemp; }
            set
            {
                _spTemp = value;
                OnPropertyChanged(nameof(SPTempR));
            }
        }
        public FloatValue SPTempAlarm;
        public FloatValue SPTempMaxCh;
        public FloatValue SPTempMinCh;
        public IntValue TControlDelayS;
        public IntValue SeasonMode;
        public FloatValue SPSeason;
        public FloatValue HystSeason;
        public IntValue RoomSPPReg;
        public IntValue RoomSPIReg;
        public IntValue RoomSPDReg;
        public CommonSetPoints()
        {
            SPTempMaxCh = new FloatValue(0,100,1);
            SPTempMinCh = new FloatValue(0, 100, 1);
            SPTempAlarm = new FloatValue(0, 100, 1);
            TControlDelayS = new IntValue(0, 65535);
            SPSeason = new FloatValue(0, 30, 1);
            HystSeason = new FloatValue(0, 20, 1);
            SeasonMode = new IntValue(0, 4);
            RoomSPPReg = new IntValue(0, 10_000);
            RoomSPIReg = new IntValue(0, 10_000);
            RoomSPDReg = new IntValue(0, 10_000);
            SPTempR = new FloatValue(0, 100, 1);
        }
    }
}
