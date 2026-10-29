using Android_Silver.ViewModels;

namespace Android_Silver.Pages;

public partial class MQTTPage : ContentPage
{
    public MQTTViewModel ViewModel { get; set; }
    public MQTTPage()
	{
		InitializeComponent();
		ViewModel = new MQTTViewModel();
		BindingContext = ViewModel;
	}
}