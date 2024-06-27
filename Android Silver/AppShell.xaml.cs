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
            Routing.RegisterRoute("kitchenTimerPage", typeof(KitchenTimerPage));
            Routing.RegisterRoute("vacationSettingsPage", typeof(VacationSettingsPage));

            #region ModesSettings pages
            Routing.RegisterRoute("setPointsPage", typeof(SetPointsPage));
            #endregion
            Routing.RegisterRoute("settingsPage", typeof(SettingsPage));
        }
    }
}