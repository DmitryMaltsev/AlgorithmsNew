using Android_Silver.Entities.Visual;
using Android_Silver.ViewModels;

using Microsoft.Maui.Controls.PlatformConfiguration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Android_Silver.Entities
{
    public class Mode1Values : BindableBase
    {
        public int StartAddress = 0;

        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

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

        private PicByStates _activeModePics;
        public PicByStates ActiveModePics
        {
            get { return _activeModePics; }
            set
            {
                _activeModePics = value;
                OnPropertyChanged(nameof(ActiveModePics));
            }
        }

        private PicByStates _selectModePics;

        public PicByStates SelectModePics
        {
            get { return _selectModePics; }
            set
            {
                _selectModePics = value;
                OnPropertyChanged(nameof(SelectModePics));
            }
        }

        private PicByStates _modeIcons;

        public PicByStates ModeIcons
        {
            get { return _modeIcons; }
            set
            {
                _modeIcons = value;
                OnPropertyChanged(nameof(ModeIcons));
            }
        }

        private string _modeSettingsRoute;

        public string ModeSettingsRoute
        {
            get { return _modeSettingsRoute; }
            set
            {
                _modeSettingsRoute = value;
                OnPropertyChanged(nameof(ModeSettingsRoute));
            }
        }

        public Mode1Values(int num, PicByStates activeModePics, PicByStates selectModePicks, PicByStates modeIcons, string modeSettingsRoute, int startAddress)
        {
            Num = num;
            ActiveModePics = activeModePics;
            SelectModePics = selectModePicks;
            ModeIcons = modeIcons;
            ModeSettingsRoute = modeSettingsRoute;
            switch (num)
            {
                case 0:
                    {

                        Title = "Выключен";
                    }
                    break;
                case 1:
                    {
                        Title = "Минимальный";

                    }
                    break;
                case 2:
                    {
                        Title = "Нормальный";

                    }
                    break;
                case 3:
                    {
                        Title = "Максимальный";
                    }
                    break;
                case 4:
                    {
                        Title = "Кухня";
                    }
                    break;
                case 5:
                    {
                        Title = "Отпуск";
                    }
                    break;
                default:
                    {
                        Title = "";
                    }
                    break;
            }
            StartAddress = startAddress;
        }

    }
}
