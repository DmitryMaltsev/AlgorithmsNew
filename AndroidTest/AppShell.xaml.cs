using SilverAndroid.Pages;

namespace SilverAndroid
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("pageWithText", typeof(PageWithText));
            Routing.RegisterRoute("moonPhasePage", typeof(MoonPhasePage));
        }
    }
}