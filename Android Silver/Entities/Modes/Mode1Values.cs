using Android_Silver.Entities.Visual;
using Android_Silver.ViewModels;

using Microsoft.Maui.Controls.PlatformConfiguration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Android_Silver.Entities.Modes
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

        /// <summary>
        /// Путь к странице настройки уставок
        /// </summary>
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



        #region Уставки
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
        #endregion


        #region Адреса картинок для текущего режима
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

        private string _modeMiniIcon;
        public string ModeMiniIcon
        {
            get { return _modeMiniIcon; }
            set {
                _modeMiniIcon = value;
                OnPropertyChanged(nameof(ModeMiniIcon));
            }
        }

        private string _miniIcon;

        public string MiniIcon
        {
            get { return _miniIcon; }
            set { 
                _miniIcon = value;
                OnPropertyChanged(nameof(MiniIcon));    
            }
        }


        #endregion

        /// <summary>
        /// Задаем текущему режиму соответветствующие значения.
        /// </summary>
        /// <param name="num"></param>
        /// <param name="activeModePics"></param>
        /// <param name="selectModePics"></param>
        /// <param name="modeIcons"></param>
        /// <param name="modeSettingsRoute"></param>
        /// <param name="startAddress"></param>
        public Mode1Values(int num, PicByStates activeModePics, PicByStates selectModePics, PicByStates modeIcons, string modeSettingsRoute, int startAddress, string miniIcon)
        {
            Num = num;
            ActiveModePics = activeModePics;
            SelectModePics = selectModePics;
            ModeIcons = modeIcons;
            ModeSettingsRoute = modeSettingsRoute;
            MiniIcon= miniIcon;
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
