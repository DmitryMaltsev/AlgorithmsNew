using Android_Silver.Entities;

namespace Android_Silver
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            DIContainer.RegisterDependencies();
            MainPage = new AppShell();
        }
    }
}