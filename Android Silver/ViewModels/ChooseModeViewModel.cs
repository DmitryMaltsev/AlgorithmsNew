using Android_Silver.Entities;
using Android_Silver.Entities.Modes;
using Android_Silver.Entities.Visual;
using Android_Silver.Entities.Visual.Menus;
using Android_Silver.Services;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Android_Silver.ViewModels
{
    public class ChooseModeViewModel : BindableBase
    {
        public ICommand MinModeCommand { get; private set; }
        public ICommand NormalModeCommand { get; private set; }
        public ICommand MaxModeCommand { get; private set; }
        public ICommand KitchenModeCommand { get; private set; }
        public ICommand ShedulerModeCommand { get; private set; }
        public ICommand VacationModeCommand { get; private set; }
        public ICommand TurnOffModeCommand { get; private set; }
        public ICommand HomeCommand { get; private set; }
        public ModesEntities CModesEntities { get; set; }

        private MenusEntities _menuesEntities { get; set; }

        private TcpClientService _tcpClientService;
        public PicturesSet CPictureSet { get; set; }

        private EthernetEntities _ethernetEntities;
        public ChooseModeViewModel()
        {
            MinModeCommand = new Command(ExecuteMinMode);
            NormalModeCommand = new Command(ExecuteNormal);
            MaxModeCommand = new Command(ExecuteMaxMode);
            KitchenModeCommand = new Command(ExecuteKitchenMode);
            ShedulerModeCommand = new Command(ExecuteSheduler);
            VacationModeCommand = new Command(ExecuteVacationMode);
            TurnOffModeCommand = new Command(ExecuteTurnOffMode);
            HomeCommand = new Command(ExecuteHomeCommand);
            CModesEntities = DIContainer.Resolve<ModesEntities>();
            _tcpClientService = DIContainer.Resolve<TcpClientService>();
            CPictureSet = DIContainer.Resolve<PicturesSet>();
            _ethernetEntities = DIContainer.Resolve<EthernetEntities>();
            _menuesEntities= DIContainer.Resolve<MenusEntities>();
        }

        async void ExecuteHomeCommand(object obj)
        {
            await Shell.Current.GoToAsync("//mainPage", false);

        }

        #region Execute modes
        async private void ExecuteTurnOffMode(object obj)
        {
            int[] index = { 0 };
            _tcpClientService.SetCommandToServer(108 + _menuesEntities.WriteOffset, index);

            await Shell.Current.GoToAsync("//mainPage", false);
            await Shell.Current.Navigation.PopToRootAsync(false);
        }
        async private void ExecuteMinMode(object obj)
        {
            int[] index = { 1, 0 };
            _tcpClientService.SetCommandToServer(108 + _menuesEntities.WriteOffset, index);
            await Shell.Current.GoToAsync("//mainPage", false);

        }

        async private void ExecuteNormal(object obj)
        {
            int[] index = { 2, 0 };
            _tcpClientService.SetCommandToServer(108 + _menuesEntities.WriteOffset, index);
            //    _ethernetEntities.WriteMessageSended = true;
            //    _tcpClientService.SendData("108,02");
            await Shell.Current.GoToAsync("//mainPage", false);
        }

        async private void ExecuteMaxMode(object obj)
        {
            int[] index = { 3, 0 };
            _tcpClientService.SetCommandToServer(108 + _menuesEntities.WriteOffset, index);
            //   _tcpClientService.SendData("108,02");
            await Shell.Current.GoToAsync("//mainPage", false);
        }

        async private void ExecuteKitchenMode(object obj)
        {
            //  int[] index = { 4 };
            //  _tcpClientService.SetCommandToServer(308, index);
            await Shell.Current.GoToAsync("//mainPage", false);

        }

        async private void ExecuteVacationMode(object obj)
        {
            int[] index = { 2 };
            _tcpClientService.SetCommandToServer(109+_menuesEntities.WriteOffset, index);
            await Shell.Current.GoToAsync("//mainPage", false);

        }

        async private void ExecuteSheduler(object obj)
        {
            int[] index = { 3 };
            _tcpClientService.SetCommandToServer(109 +  _menuesEntities.WriteOffset, index);
            await Shell.Current.GoToAsync("//mainPage", false);
        }

        #endregion
    }
}
