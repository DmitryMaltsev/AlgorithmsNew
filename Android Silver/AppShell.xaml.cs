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
        }
    }
}