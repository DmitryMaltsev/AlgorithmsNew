namespace Android_Silver.Pages
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