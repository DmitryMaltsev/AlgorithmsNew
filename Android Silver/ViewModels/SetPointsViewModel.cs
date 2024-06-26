using Android_Silver.Entities;
using Android_Silver.Entities.Visual;
using Android_Silver.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Android_Silver.ViewModels
{
    public class SetPointsViewModel : BindableBase
    {
        public ModesEntities CModesEntities { get; set; }
        public PicturesSet CPictureSet { get; set; }

        private IEthernetEntities _ethernetEntities;

        private ITcpClientService _tcpClientService;

        public ICommand NextSetPointsCommand { get; private set; }
        public ICommand HomeCommand { get; private set; }
        public ICommand OkCommand { get; private set; }

        private Mode1Values _m1Values;
        public Mode1Values M1Values
        {
            get { return _m1Values; }

            set
            {
                _m1Values = value;
                OnPropertyChanged(nameof(M1Values));
            }
        }

        public SetPointsViewModel()
        {
            CModesEntities = DIContainer.Resolve<ModesEntities>();
            CPictureSet = DIContainer.Resolve<PicturesSet>();
            _ethernetEntities = DIContainer.Resolve<IEthernetEntities>();
            _tcpClientService = DIContainer.Resolve<ITcpClientService>();
            NextSetPointsCommand = new Command(ExecuteNextSetPoints);
            HomeCommand = new Command(ExecuteHome);
            OkCommand = new Command(ExecuteOk);
            SetM1ValuesByIndex(CModesEntities.CMode1.Num);

        }



        private void SetM1ValuesByIndex(int index)
        {

            if (index == 0 || index == 5)
            {
                index = 1;
            }
            else
            if (index < 5)
            {
                index += 1;
            }
            Mode1Values bufVals = CModesEntities.Mode1ValuesList[index];
            M1Values = new Mode1Values(bufVals.Num, bufVals.ActiveModePics,
                      bufVals.SelectModePics,
                      bufVals.ModeIcons,
                      bufVals.ModeSettingsRoute,
                      bufVals.StartAddress);
            M1Values.SypplySP = bufVals.SypplySP;
            M1Values.ExhaustSP = bufVals.ExhaustSP;
            M1Values.TempSP = bufVals.TempSP;
            M1Values.PowerLimitSP = bufVals.PowerLimitSP;
        }

        #region Execute commands

        private void ExecuteOk(object obj)
        {
            int[] values = { M1Values.SypplySP, M1Values.ExhaustSP, M1Values.TempSP, M1Values.PowerLimitSP };
            _tcpClientService.SetCommandToServer(M1Values.StartAddress, values);
        }

        private void ExecuteNextSetPoints(object obj)
        {
            SetM1ValuesByIndex(M1Values.Num);
        }

        async void ExecuteHome(object obj)
        {
            await Shell.Current.GoToAsync("mainPage");
        } 
        #endregion
    }
}
