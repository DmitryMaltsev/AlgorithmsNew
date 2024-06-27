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

    private void UpDigit_Pressed(object sender, EventArgs e)
    {
        _kitchentTimerVM.CPictureSet.KitchenButtonUp.Current = _kitchentTimerVM.CPictureSet.KitchenButtonUp.Selected;
    }

    private void UpDigit_Released(object sender, EventArgs e)
    {
        _kitchentTimerVM.CPictureSet.KitchenButtonUp.Current = _kitchentTimerVM.CPictureSet.KitchenButtonUp.Default;
    }

    private void DnDigit_Pressed(object sender, EventArgs e)
    {
        _kitchentTimerVM.CPictureSet.KitchenButtonDn.Current = _kitchentTimerVM.CPictureSet.KitchenButtonDn.Selected;
    }

    private void DnDigit_Released(object sender, EventArgs e)
    {
        _kitchentTimerVM.CPictureSet.KitchenButtonDn.Current = _kitchentTimerVM.CPictureSet.KitchenButtonDn.Default;
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

    private void AcceptButton_Pressed(object sender, EventArgs e)
    {
        _kitchentTimerVM.CPictureSet.AcceptButton.Current = _kitchentTimerVM.CPictureSet.AcceptButton.Selected;
    }

    private void AcceptButton_Released(object sender, EventArgs e)
    {
        _kitchentTimerVM.CPictureSet.AcceptButton.Current = _kitchentTimerVM.CPictureSet.AcceptButton.Default;
    }

    private void CancelButton_Pressed(object sender, EventArgs e)
    {
        _kitchentTimerVM.CPictureSet.CancelButton.Current = _kitchentTimerVM.CPictureSet.CancelButton.Selected;
    }

    private void CancelButton_Released(object sender, EventArgs e)
    {
        _kitchentTimerVM.CPictureSet.CancelButton.Current = _kitchentTimerVM.CPictureSet.CancelButton.Default;
    }
}