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

        public Mode1Values CMode1 { get; set; }

        public List<Mode1Values> Mode1ValuesList { get; set; } = new List<Mode1Values>();

        public Mode2Values CMode2 { get; set; }

        public List<Mode2Values> Mode2ValuesList { get; set; } = new List<Mode2Values>();

        private PicturesSet _cPicturesSet { get; set; }

        public ModesEntities()
        {
            _cPicturesSet = DIContainer.Resolve<PicturesSet>();
            _cPicturesSet.Init(PicturesSetStates.Base);
            Mode1ValuesList = new List<Mode1Values>();
            for (int i = 0; i < 9; i++)
            {
                Mode1ValuesList.Add(new Mode1Values(i, _cPicturesSet.ActiveModesPicks[i],
            _cPicturesSet.SelectModesPicks[i], _cPicturesSet.IconsPics[i], "settingsPage", 306 + i * 4));
            }
            CMode1 = Mode1ValuesList[0];
            Mode2ValuesList = new List<Mode2Values>();
            //Стандартный режим, работает всегда
            Mode2ValuesList.Add(new Mode2Values(1));
            //Режим кухни
            Mode2ValuesList.Add(new Mode2Values(1));
            //Режим отпуска
            Mode2ValuesList.Add(new Mode2Values(4));
            //Режим  календаря
            Mode2ValuesList.Add(new Mode2Values(28));
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
    }
}
