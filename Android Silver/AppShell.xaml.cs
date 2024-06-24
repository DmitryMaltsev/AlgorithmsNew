using Android_Silver.Pages;
using Android_Silver.Pages.ModesSettings;
using Android_Silver.Pages.Settings;

namespace Android_Silver
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();



            Routing.RegisterRoute("mainPage", typeof(MainPage));
            Routing.RegisterRoute("startPage", typeof(StartPage));
            Routing.RegisterRoute("chooseModePage", typeof(ChooseModePage));

            Routing.RegisterRoute("minSettingsPage", typeof(MinSettingsPage));
            Routing.RegisterRoute("normalSettingsPage", typeof(NormalSettingsPage));
            Routing.RegisterRoute("maxSettingsPage", typeof(MaxSettingsPage));
            Routing.RegisterRoute("vacationSettingsPage", typeof(VacationSettingsPage));
            Routing.RegisterRoute("shedulerSettingsPage", typeof(ShedulerSettingsPage));

            #region ModesSettings pages
            Routing.RegisterRoute("settingsPage1", typeof(SettingsPage1));
            Routing.RegisterRoute("settingsPage2", typeof(SettingsPage2));
            Routing.RegisterRoute("settingsPage3", typeof(SettingsPage3));
            Routing.RegisterRoute("settingsPage4", typeof(SettingsPage4));
            Routing.RegisterRoute("settingsPage5", typeof(SettingsPage5));
            Routing.RegisterRoute("setPointsPage", typeof(SetPointsPage));
            #endregion
            Routing.RegisterRoute("settingsPage", typeof(SettingsPage));
            Routing.RegisterRoute("kitchenTimerPage", typeof(KitchenTimerPage));
        }
    }
}