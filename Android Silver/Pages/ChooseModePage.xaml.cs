using Android_Silver.ViewModels;

namespace Android_Silver.Pages;

public partial class ChooseModePage : ContentPage
{
	private ChooseModeViewModel _chooseModeViewModel;
	public ChooseModePage()
	{
		InitializeComponent();
		_chooseModeViewModel = new ChooseModeViewModel();
		BindingContext = _chooseModeViewModel;
	}
}