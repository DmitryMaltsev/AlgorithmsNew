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
        protected override Window CreateWindow(IActivationState activationState)
        {
            var window = base.CreateWindow(activationState);

            const int newWidth = 600;
            const int newHeight = 800;

            window.Width = newWidth;
            window.Height = newHeight;

            return window;
        }

    }
}