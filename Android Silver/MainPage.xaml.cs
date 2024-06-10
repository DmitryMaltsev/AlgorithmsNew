using Android_Silver.Pages;

namespace Android_Silver
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        private MainPageViewModel _viewModel;
        public MainPage()
        {
            InitializeComponent();
            _viewModel = new MainPageViewModel();
            BindingContext = _viewModel;
        }

    }
}