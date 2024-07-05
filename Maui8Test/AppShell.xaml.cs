namespace Maui8Test
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("sPage", typeof(SecontPage));
        }
    }
}
