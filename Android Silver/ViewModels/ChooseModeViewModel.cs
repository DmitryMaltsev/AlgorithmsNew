using Android_Silver.Entities;

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
        }

        async void ExecuteHomeCommand(object obj)
        {
            await Shell.Current.GoToAsync("mainPage");
        }

        #region Execute modes
        async private void ExecuteTurnOffMode(object obj)
        {
            int index = 0;
            SetMode1ValuesByIndex(index);
            await Shell.Current.GoToAsync("mainPage");
        }
        async private void ExecuteMinMode(object obj)
        {
            int index = 1;
            SetMode1ValuesByIndex(index);
            await Shell.Current.GoToAsync("mainPage");
        }

        async private void ExecuteNormal(object obj)
        {
            int index = 2;
            SetMode1ValuesByIndex(index);
            await Shell.Current.GoToAsync("mainPage");
        }

        async private void ExecuteMaxMode(object obj)
        {
            int index = 3;
            SetMode1ValuesByIndex(index);
            await Shell.Current.GoToAsync("mainPage");
        }

        async private void ExecuteKitchenMode(object obj)
        {
            int index = 4;
            SetMode1ValuesByIndex(index);
            await Shell.Current.GoToAsync("kitchenTimerPage");
        }

        async private void ExecuteVacationMode(object obj)
        {
            int index = 5;
            SetMode1ValuesByIndex(index);
            await Shell.Current.GoToAsync("mainPage");
        }

        async private void ExecuteSheduler(object obj)
        {
            int index =1;
            SetMode1ValuesByIndex(index);
            await Shell.Current.GoToAsync("mainPage");
        }

        private void SetMode1ValuesByIndex(int index)
        {
            CModesEntities.CMode1 = CModesEntities.Mode1ValuesList[index];
            CModesEntities.CMode1Pic = CModesEntities.Mode1Pics[index];
            CModesEntities.CModeSettingsRoute = CModesEntities.ModeSettingsRoutes[index];
        }
        #endregion
    }
}
