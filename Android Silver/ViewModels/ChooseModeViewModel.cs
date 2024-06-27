using Android_Silver.Entities;
using Android_Silver.Entities.Modes;
using Android_Silver.Entities.Visual;
using Android_Silver.Services;

using System;
using System.Collections.Generic;
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

        private TcpClientService _tcpClientService;

        public PicturesSet CPictureSet { get; set; }
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
            
        }

        async void ExecuteHomeCommand(object obj)
        {
            await Shell.Current.GoToAsync("mainPage");
        }

        #region Execute modes
        async private void ExecuteTurnOffMode(object obj)
        {
            int[] index = { 0 };
            _tcpClientService.SetCommandToServer(308, index);
            await Shell.Current.GoToAsync("mainPage");
        }
        async private void ExecuteMinMode(object obj)
        {
            int[] index = { 1 };
            _tcpClientService.SetCommandToServer(308, index);
            await Shell.Current.GoToAsync("mainPage");
        }

        async private void ExecuteNormal(object obj)
        {
            int[] index = { 2 };
            _tcpClientService.SetCommandToServer(308, index);
            await Shell.Current.GoToAsync("mainPage");
        }

        async private void ExecuteMaxMode(object obj)
        {
            int[] index = { 3 };
            _tcpClientService.SetCommandToServer(308, index);
            await Shell.Current.GoToAsync("mainPage");
        }

        async private void ExecuteKitchenMode(object obj)
        {
          //  int[] index = { 4 };
          //  _tcpClientService.SetCommandToServer(308, index);
            await Shell.Current.GoToAsync("kitchenTimerPage");
        }

        async private void ExecuteVacationMode(object obj)
        {
            int[] index = { 5 };
            _tcpClientService.SetCommandToServer(308, index);
            await Shell.Current.GoToAsync("mainPage");
        }

        async private void ExecuteSheduler(object obj)
        {
            int[] index = { 1 };
            _tcpClientService.SetCommandToServer(308, index);
            await Shell.Current.GoToAsync("mainPage");
        }

        #endregion
    }
}
