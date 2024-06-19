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
        private int _cMode1;
        public int CMode1
        {
            get { return _cMode1; }
            set
            {
                _cMode1 = value;
                OnPropertyChanged(nameof(CMode1));
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


        private List<string> _modeSettingsRoutes=new();

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
            _mode1Pics.Add("modes_main_0.jpg");
            _mode1Pics.Add("modes_main_1.jpg");
            _mode1Pics.Add("modes_main_2.jpg");
            _mode1Pics.Add("modes_main_3.jpg");
            _mode1Pics.Add("modes_main_4.jpg");
            _mode1Pics.Add("modes_main_5.jpg");
            _mode1Pics.Add("modes_main_6.jpg");
            _cMode1Pic = _mode1Pics[1];

            ModeSettingsRoutes.Add("settingsPage1");
            ModeSettingsRoutes.Add("settingsPage1");
            ModeSettingsRoutes.Add("settingsPage2");
            ModeSettingsRoutes.Add("settingsPage3");
            ModeSettingsRoutes.Add("settingsPage4");
            ModeSettingsRoutes.Add("settingsPage5");
            CModeSettingsRoute = ModeSettingsRoutes[1];
        }
    }
}
