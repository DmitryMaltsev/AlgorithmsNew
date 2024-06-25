using Android_Silver.Entities.Visual;
using Android_Silver.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Android_Silver.Entities
{
    public class Mode1Values : BindableBase
    {
        private int _num;

        public int Num
        {
            get { return _num; }
            set { _num = value; }
        }



        private int _supplySP;

        public int SypplySP
        {
            get { return _supplySP; }
            set
            {
                _supplySP = value;
                OnPropertyChanged(nameof(SypplySP));
            }
        }

        private int _exhaustSP;

        public int ExhaustSP
        {
            get { return _exhaustSP; }
            set
            {
                _exhaustSP = value;
                OnPropertyChanged(nameof(ExhaustSP));
            }
        }

        private int _tempSP;

        public int TempSP
        {
            get { return _tempSP; }
            set
            {
                _tempSP = value;
                OnPropertyChanged(nameof(TempSP));
            }
        }
        private int _powerLimitSP;

        public int PowerLimitSP
        {
            get { return _powerLimitSP; }
            set
            {
                _powerLimitSP = value;
                OnPropertyChanged(nameof(PowerLimitSP));
            }
        }

        private PicByStates  _modePics;

        public PicByStates ModePics
        {
            get { return _modePics; }
            set { _modePics = value; 
                OnPropertyChanged(nameof(ModePics));
            }
        }

        private PicByStates _modeIcons;

        public PicByStates ModeIcons
        {
            get { return _modeIcons; }
            set { 
                _modeIcons = value;
                OnPropertyChanged(nameof(ModeIcons));
            }
        }

        private string _modeSettingsRoute;

        public string ModeSettingsRoute
        {
            get { return _modeSettingsRoute; }
            set { 
                _modeSettingsRoute = value; 
                OnPropertyChanged(nameof(ModeSettingsRoute));
            }
        }



    }
}
