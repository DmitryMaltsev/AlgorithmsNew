using Android_Silver.ViewModels;

namespace Android_Silver.Pages;

public partial class KitchenTimerPage : ContentPage
{
	private KitchenTimerViewModel _kitchentTimerVM;
	public KitchenTimerPage()
	{
		InitializeComponent();
		_kitchentTimerVM = new KitchenTimerViewModel();
		this.BindingContext = _kitchentTimerVM;
	}

  
    private void Next_Pressed(object sender, EventArgs e)
    {
        _kitchentTimerVM.CPictureSet.NextButton.Current = _kitchentTimerVM.CPictureSet.NextButton.Selected;
    }

    private void Next_Released(object sender, EventArgs e)
    {
        _kitchentTimerVM.CPictureSet.NextButton.Current = _kitchentTimerVM.CPictureSet.NextButton.Default;
    }

    private void HomeButton_Pressed(object sender, EventArgs e)
    {
        _kitchentTimerVM.CPictureSet.HomeButton.Current = _kitchentTimerVM.CPictureSet.HomeButton.Selected;
    }

    private void HomeButton_Released(object sender, EventArgs e)
    {
        _kitchentTimerVM.CPictureSet.HomeButton.Current = _kitchentTimerVM.CPictureSet.HomeButton.Default;
    }
}