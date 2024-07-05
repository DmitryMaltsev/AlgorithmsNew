using Android_Silver.Entities;
using Android_Silver.ViewModels;

namespace Android_Silver.Pages;

public partial class LoadingPage : ContentPage
{
	LoadingPageViewModel _loadingPageViewModel;
	public LoadingPage()
	{
		InitializeComponent();
        _loadingPageViewModel = new LoadingPageViewModel();//IContainer.Resolve<LoadingPageViewModel>();
        BindingContext = _loadingPageViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
       // BindingContext = null;
    }
}