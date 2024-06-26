using Android_Silver.Pages;
using Android_Silver.Pages.ModesSettings;

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
            Routing.RegisterRoute("setPointsPage", typeof(SetPointsPage));
            #endregion
            Routing.RegisterRoute("settingsPage", typeof(SettingsPage));
            Routing.RegisterRoute("kitchenTimerPage", typeof(KitchenTimerPage));
        }
    }
}