using Android_Silver.ViewModels;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Android_Silver.Entities
{
    public class ModesEntities : BindableBase
    {
        private Mode1Values _cMode1 = new();
        public Mode1Values CMode1
        {
            get { return _cMode1; }
            set
            {
                if (_cMode1 != value)
                {
                    _cMode1 = value;
                    OnPropertyChanged(nameof(CMode1));
                    // CMode1.SypplySP = value.SypplySP;
                    // CMode1.ExhaustSP = value.ExhaustSP;
                    // CMode1.TempSP = value.TempSP;
                    // CMode1.PowerLimitSP = value.PowerLimitSP;
                }


            }
        }

        private int _cMode2;
        public int CMode2
        {
            get { return _cMode2; }
            set
            {
                _cMode2 = value;
                OnPropertyChanged($"{nameof(CMode2)}");
            }
        }


        public List<Mode1Values> Mode1ValuesList { get; set; } = new List<Mode1Values>();




        private string _cMode1Pic;
        public string CMode1Pic
        {
            get { return _cMode1Pic; }
            set
            {
                _cMode1Pic = value;
                OnPropertyChanged(nameof(CMode1Pic));
            }
        }



        private string _cModeSettingsRoute;
        public string CModeSettingsRoute
        {
            get { return _cModeSettingsRoute; }
            set
            {
                _cModeSettingsRoute = value;
                OnPropertyChanged($"{nameof(CModeSettingsRoute)}");
            }
        }


        private List<string> _mode1Pics = new List<string>();
        public List<string> Mode1Pics
        {

            get { return _mode1Pics; }
            set
            {
                _mode1Pics = value;
            }
        }

        private List<string> _mode2Pics = new List<string>();
        public List<string> Mode2Pics
        {
            get { return _mode2Pics; }
            set { _mode2Pics = value; }
        }


        private List<string> _modeSettingsRoutes = new();

        public List<string> ModeSettingsRoutes
        {
            get { return _modeSettingsRoutes; }
            set { _modeSettingsRoutes = value; }
        }

        private string _cMode2Pic;
        public string CMode2Pic
        {
            get { return _cMode2Pic; }
            set
            {
                _cMode2Pic = value;
                OnPropertyChanged(nameof(CMode2Pic));
            }
        }

        public ModesEntities()
        {
            for (int i = 0; i < 9; i++)
            {
                Mode1ValuesList.Add(new Mode1Values() { Num=i});
            }
            //Иконка режима
            _mode1Pics.Add("main_mode_0.png");
            _mode1Pics.Add("main_mode_1.png");
            _mode1Pics.Add("main_mode_2.png");
            _mode1Pics.Add("main_mode_3.png");
            _mode1Pics.Add("main_mode_4.png");
            _mode1Pics.Add("main_mode_5.png");
            _mode1Pics.Add("main_mode_6.png");
            _mode1Pics.Add("main_mode_7.png");
            _mode1Pics.Add("main_mode_8.png");
            _cMode1Pic = _mode1Pics[0];

            //Маршрут настройки режима
            ModeSettingsRoutes.Add("settingsPage1");
            ModeSettingsRoutes.Add("settingsPage1");
            ModeSettingsRoutes.Add("settingsPage2");
            ModeSettingsRoutes.Add("settingsPage3");
            ModeSettingsRoutes.Add("settingsPage4");
            ModeSettingsRoutes.Add("settingsPage5");
            ModeSettingsRoutes.Add("settingsPage1");
            CModeSettingsRoute = ModeSettingsRoutes[0];
        }


        public void SetMode1ValuesByIndex(int index)
        {
            if (CMode1 != Mode1ValuesList[index])
            {
                CMode1 = Mode1ValuesList[index];
                CMode1Pic = Mode1Pics[index];
                CModeSettingsRoute = ModeSettingsRoutes[index];

            }
        }
    }
}
