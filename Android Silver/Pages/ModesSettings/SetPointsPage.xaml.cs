using Android_Silver.ViewModels;

namespace Android_Silver.Pages.ModesSettings;

public partial class SetPointsPage : ContentPage
{
	SetPointsViewModel _spViewModel;
	public SetPointsPage()
	{
		InitializeComponent();
		_spViewModel = new SetPointsViewModel();
		BindingContext = _spViewModel;


	}
}