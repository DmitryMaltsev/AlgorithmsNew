using Android_Silver.ViewModels;

namespace Android_Silver.Pages;

public partial class ServicePage : ContentPage
{
    ServicePageViewModel _servicePageViewModel;
	public ServicePage()
	{
		InitializeComponent();
        _servicePageViewModel = new ServicePageViewModel();
        BindingContext = _servicePageViewModel;
    }
}