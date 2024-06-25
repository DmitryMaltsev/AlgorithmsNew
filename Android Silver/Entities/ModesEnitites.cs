using Android_Silver.Entities.Visual;
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

        private PicturesSet _cPictures;
        public PicturesSet CPictures
        {
            get { return _cPictures; }
            set
            {
                _cPictures = value;
                OnPropertyChanged($"{nameof(CPictures)}");
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
          
            Mode1ValuesList = new List<Mode1Values>
            {
            new Mode1Values() { Num = 0, ModePics = new("", ""), ModeSettingsRoute = "settingsPage1"},
            new Mode1Values() { Num = 1, ModePics = new("", ""), ModeSettingsRoute = "settingsPage1"},
            new Mode1Values() { Num = 2, ModePics = new("", ""), ModeSettingsRoute = "settingsPage2"},
            new Mode1Values() { Num = 3, ModePics = new("", ""), ModeSettingsRoute = "settingsPage3"},
            new Mode1Values() { Num = 4, ModePics = new("", ""), ModeSettingsRoute = "settingsPage4"},
            new Mode1Values() { Num = 5, ModePics = new("", ""), ModeSettingsRoute = "settingsPage5"},
            new Mode1Values() { Num = 6, ModePics = new("", ""), ModeSettingsRoute = "settingsPage6"}
        };

          //  CModeSettingsRoute = ModeSettingsRoutes[0];
        }

    public void SetMode1ValuesByIndex(int index)
    {
        if (CMode1 != Mode1ValuesList[index])
        {
            CMode1 = Mode1ValuesList[index];
           // CMode1Pic = Mode1Pics[index];
           // CModeSettingsRoute = ModeSettingsRoutes[index];

        }
    }
}
}
