using Android_Silver.Entities.Visual;
using Android_Silver.ViewModels;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Android_Silver.Entities.Modes
{
    public class ModesEntities : BindableBase
    {
        private List<TimeModeValues> _cTimeModeValues;

        public List<TimeModeValues> CTimeModeValues
        {
            get { return _cTimeModeValues; }
            set { 
                _cTimeModeValues = value;
                OnPropertyChanged(nameof(CTimeModeValues));
            }
        }

        private Mode1Values _CMode1;

        public Mode1Values CMode1
        {
            get { return _CMode1; }
            set
            {
                _CMode1 = value;
                OnPropertyChanged(nameof(CMode1));
            }
        }

        public List<Mode1Values> Mode1ValuesList { get; set; } = new List<Mode1Values>();

        private Mode2Values _cMode2;

        public Mode2Values CMode2
        {
            get { return _cMode2; }
            set
            {
                _cMode2 = value;
                OnPropertyChanged(nameof(CMode2));
            }
        }

        public List<Mode2Values> Mode2ValuesList { get; set; } = new List<Mode2Values>();


        private PicturesSet _cPicturesSet { get; set; }

        public ModesEntities()
        {
            _cPicturesSet = DIContainer.Resolve<PicturesSet>();
            _cPicturesSet.Init(PicturesSetStates.Base);
            Mode1ValuesList = new List<Mode1Values>();
            for (int i = 0; i < 9; i++)
            {
                Mode1ValuesList.Add(new Mode1Values(i, _cPicturesSet.ActiveModesPics[i],
            _cPicturesSet.SelectModesPics[i], _cPicturesSet.IconsPics[i], "settingsPage", 306 + i * 4, _cPicturesSet.MiniIconsPics[i]));
            }
            CMode1 = Mode1ValuesList[0];
            Mode2ValuesList = new List<Mode2Values>();
            //Стандартный режим, работает всегда
            Mode2ValuesList.Add(new Mode2Values(0, 1, _cPicturesSet.ActiveModesPics[0].Default, Mode1ValuesList[0],0));
            //Режим кухни
            Mode2ValuesList.Add(new Mode2Values(1, 1, _cPicturesSet.ActiveModesPics[4].Default, Mode1ValuesList[0],0));
            //Режим отпуска
            Mode2ValuesList.Add(new Mode2Values(2, 4, _cPicturesSet.ActiveModesPics[5].Default, Mode1ValuesList[0], 338));
            //Режим  календаря
            Mode2ValuesList.Add(new Mode2Values(3, 28, _cPicturesSet.ActiveModesPics[6].Default, Mode1ValuesList[0],0));
            //Режим по контакту
            Mode2ValuesList.Add(new Mode2Values(4, 1, _cPicturesSet.ActiveModesPics[0].Default, Mode1ValuesList[0], 0));
            //Специальный режим
            Mode2ValuesList.Add(new Mode2Values(5, 1, _cPicturesSet.ActiveModesPics[0].Default, Mode1ValuesList[0], 0));
            CMode1 = Mode1ValuesList[0];
            CMode2 = Mode2ValuesList[0];
        }

        public void SetMode1ValuesByIndex(int index)
        {
            if (index < Mode1ValuesList.Count)
            {
                if (CMode1 != Mode1ValuesList[index])
                {
                    CMode1 = Mode1ValuesList[index];
                    // CMode1Pic = Mode1Pics[index];
                    // CModeSettingsRoute = ModeSettingsRoutes[index];
                }
            }
        }

        public void SetMode2ValuesByIndex(int index)
        {

            if (index < Mode2ValuesList.Count)
            {
                if (CMode2 != Mode2ValuesList[index])
                {
                    CMode2 = Mode2ValuesList[index];
                }
            }
        }
    }
}
