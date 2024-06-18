using Android_Silver.Pages;

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
            Routing.RegisterRoute("kitchenSettingsPage", typeof(KitchenSettingsPage));
            Routing.RegisterRoute("vacationSettingsPage", typeof(VacationSettingsPage));
            Routing.RegisterRoute("shedulerSettingsPage", typeof(ShedulerSettingsPage));
        }
    }
}