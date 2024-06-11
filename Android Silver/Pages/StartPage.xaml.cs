namespace Android_Silver.Pages;

public partial class StartPage : ContentPage
{
    private StartPageViewModel _viewModel;
    public StartPage()
	{
		InitializeComponent();
        _viewModel = new StartPageViewModel();
        BindingContext = _viewModel;
    }
}