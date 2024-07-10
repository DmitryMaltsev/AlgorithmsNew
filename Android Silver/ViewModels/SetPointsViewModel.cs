using Android_Silver.Entities;
using Android_Silver.Entities.Modes;
using Android_Silver.Entities.Visual;
using Android_Silver.Services;

using System;
using System.Windows.Input;

namespace Android_Silver.ViewModels
{
    public class SetPointsViewModel : BindableBase
    {
        public ModesEntities CModesEntities { get; set; }
        public PicturesSet CPictureSet { get; set; }

        private EthernetEntities _ethernetEntities;

        private TcpClientService _tcpClientService;

        #region Commands
        public ICommand NextSetPointsCommand { get; private set; }
        public ICommand HomeCommand { get; private set; }
        public ICommand OkCommand { get; private set; }

        public ICommand BtnUpCommand0 { get; private set; }
        public ICommand BtnDnCommand0 { get; private set; }
        public ICommand BtnUpCommand1 { get; private set; }
        public ICommand BtnDnCommand1 { get; private set; }

        public ICommand BtnUpCommand2 { get; private set; }
        public ICommand BtnDnCommand2 { get; private set; }
        public ICommand BtnUpCommand3 { get; private set; }
        public ICommand BtnDnCommand3 { get; private set; }
        #endregion

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
            _ethernetEntities = DIContainer.Resolve<EthernetEntities>();
            _tcpClientService = DIContainer.Resolve<TcpClientService>();
            NextSetPointsCommand = new Command(ExecuteNextSetPoints);
            HomeCommand = new Command(ExecuteHome);
            OkCommand = new Command(ExecuteOk);
            SetM1ValuesByIndex(CModesEntities.CMode1.Num);
            BtnUpCommand0 = new Command(ExecuteBtnUP0);
            BtnDnCommand0 = new Command(ExecuteBtnDn0);

            BtnUpCommand1 = new Command(ExecuteBtnUP1);
            BtnDnCommand1 = new Command(ExecuteBtnDn1);

            BtnUpCommand2 = new Command(ExecuteBtnUP2);
            BtnDnCommand2 = new Command(ExecuteBtnDn2);

            BtnUpCommand3 = new Command(ExecuteBtnUP3);
            BtnDnCommand3 = new Command(ExecuteBtnDn3);
        }

        private void SetM1ValuesByIndex(int index)
        {
            index = index > 0 ? index : 1;

            Mode1Values bufVals = CModesEntities.Mode1ValuesList[index];
            M1Values = new Mode1Values(bufVals.Num, bufVals.ActiveModePics,
                      bufVals.SelectModePics,
                      bufVals.ModeIcons,
                      bufVals.ModeSettingsRoute,
                      bufVals.StartAddress,
                      bufVals.MiniIcon);
            M1Values.SypplySP = bufVals.SypplySP;
            M1Values.ExhaustSP = bufVals.ExhaustSP;
            M1Values.TempSP = bufVals.TempSP;
            M1Values.PowerLimitSP = bufVals.PowerLimitSP;
        }

        #region ExecuteCommands

        async void ExecuteOk(object obj)
        {
            int[] values = { M1Values.SypplySP, M1Values.ExhaustSP, M1Values.TempSP, M1Values.PowerLimitSP };
            _tcpClientService.SetCommandToServer(M1Values.StartAddress, values);
            await Shell.Current.GoToAsync("mainPage");
        }

        private void ExecuteNextSetPoints(object obj)
        {
            int ind = 0;
            if (M1Values.Num == 0 || M1Values.Num == 5)
            {
                ind = 1;
            }
            else
            if (M1Values.Num < 5)
            {
                ind = M1Values.Num + 1;
            }
            SetM1ValuesByIndex(ind);
        }

        async void ExecuteHome(object obj)
        {
            await Shell.Current.GoToAsync("mainPage");
        }

        private void ExecuteBtnUP0(object obj)
        {

            M1Values.SypplySP = M1Values.SypplySP + 5 < 100 ? M1Values.SypplySP + 5 : 100;
        }
        private void ExecuteBtnDn0(object obj)
        {
            M1Values.SypplySP = M1Values.SypplySP - 5 > 0 ? M1Values.SypplySP - 5 : 0;
        }

        private void ExecuteBtnUP1(object obj)
        {
            M1Values.ExhaustSP = M1Values.ExhaustSP + 5 < 100 ? M1Values.ExhaustSP + 5 : 100;
        }
        private void ExecuteBtnDn1(object obj)
        {
            M1Values.ExhaustSP = M1Values.ExhaustSP - 5 > 0 ? M1Values.ExhaustSP - 5 : 0;
        }

        private void ExecuteBtnUP2(object obj)
        {
            M1Values.TempSP = M1Values.TempSP + 1 < 35 ? M1Values.TempSP + 1 : 35;
            if(M1Values.TempSP < 16) M1Values.TempSP = 16;
        }
        private void ExecuteBtnDn2(object obj)
        {
            M1Values.TempSP = M1Values.TempSP - 1 > 16 ? M1Values.TempSP - 1 : 16;
        }

        private void ExecuteBtnUP3(object obj)
        {
            M1Values.PowerLimitSP = M1Values.PowerLimitSP + 5 < 100 ? M1Values.PowerLimitSP + 5 : 100;
        }
        private void ExecuteBtnDn3(object obj)
        {
            M1Values.PowerLimitSP = M1Values.PowerLimitSP - 5 > 0 ? M1Values.PowerLimitSP - 5 : 0;
        }
        #endregion
    }
}
