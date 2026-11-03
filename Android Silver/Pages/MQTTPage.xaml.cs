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
    private void IPButton_Pressed(object sender, EventArgs e)
    {
        ViewModel.CPictureSet.IPBut.Current = ViewModel.CPictureSet.IPBut.Selected;
    }

    private void IPButton_Released(object sender, EventArgs e)
    {
        ViewModel.CPictureSet.IPBut.Current = ViewModel.CPictureSet.IPBut.Default;
    }
}