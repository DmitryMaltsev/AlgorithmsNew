using Android_Silver.ViewModels;

namespace Android_Silver.Pages;

public partial class LoadingPage : ContentPage
{
	LoadingPageViewModel _loadingPageViewModel;
	public LoadingPage()
	{
		InitializeComponent();
        _loadingPageViewModel=new LoadingPageViewModel();	
        BindingContext = _loadingPageViewModel;
    }
}