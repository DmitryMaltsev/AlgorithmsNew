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

        public ModesEntities Modes { get; set; }

        public ChooseModeViewModel()
        {
            MinModeCommand = new Command(ExecuteMinMode);
            NormalModeCommand = new Command(ExecuteNormal);
            MaxModeCommand = new Command(ExecuteMaxMode);
            KitchenModeCommand = new Command(ExecuteKitchenMode);
            ShedulerModeCommand = new Command(ExecuteSheduler);
            VacationModeCommand = new Command(ExecuteVacationMode);
            TurnOffModeCommand = new Command(ExecuteTurnOffMode);
            Modes=DIContainer.Resolve<ModesEntities>();
        }

        #region Execute modes
       async private void ExecuteTurnOffMode(object obj)
        {
            Modes.CMode1 = 0;
            Modes.CMode1Pic = Modes.Mode1Pics[Modes.CMode1];
            Modes.CModeSettingsRoute = Modes.ModeSettingsRoutes[Modes.CMode1];
            await  Shell.Current.GoToAsync("mainPage");
        }
        async private void ExecuteMinMode(object obj)
        {
            Modes.CMode1 = 1;
            Modes.CMode1Pic = Modes.Mode1Pics[Modes.CMode1];
            Modes.CModeSettingsRoute = Modes.ModeSettingsRoutes[Modes.CMode1];
            await Shell.Current.GoToAsync("mainPage");
        }

        async private void ExecuteNormal(object obj)
        {
            Modes.CMode1 = 2;
            Modes.CMode1Pic = Modes.Mode1Pics[Modes.CMode1];
            Modes.CModeSettingsRoute = Modes.ModeSettingsRoutes[Modes.CMode1];
            await Shell.Current.GoToAsync("mainPage");
        }

        async private void ExecuteMaxMode(object obj)
        {
            Modes.CMode1 = 3;
            Modes.CMode1Pic = Modes.Mode1Pics[Modes.CMode1];
            Modes.CModeSettingsRoute = Modes.ModeSettingsRoutes[Modes.CMode1];
            await Shell.Current.GoToAsync("mainPage");
        }

        async private void ExecuteKitchenMode(object obj)
        {
            Modes.CMode1 = 4;
            Modes.CMode1Pic = Modes.Mode1Pics[Modes.CMode1];
            Modes.CModeSettingsRoute = Modes.ModeSettingsRoutes[Modes.CMode1];
            await Shell.Current.GoToAsync("kitchenTimerPage");
        }

        async private void ExecuteVacationMode(object obj)
        {
            Modes.CMode1 = 5;
            Modes.CMode1Pic = Modes.Mode1Pics[Modes.CMode1];
            Modes.CModeSettingsRoute = Modes.ModeSettingsRoutes[Modes.CMode1];
            await Shell.Current.GoToAsync("mainPage");
        }

        async private void ExecuteSheduler(object obj)
        {
            Modes.CMode1 = 6;
            Modes.CMode1Pic = Modes.Mode1Pics[Modes.CMode1];
            Modes.CModeSettingsRoute = Modes.ModeSettingsRoutes[Modes.CMode1];
            await Shell.Current.GoToAsync("mainPage");
        }
        #endregion
    }
}
