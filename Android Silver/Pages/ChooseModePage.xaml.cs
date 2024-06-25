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

    private void Home_Pressed(object sender, EventArgs e)
    {
        _chooseModeViewModel.CPictureSet.HomeButton.Current = _chooseModeViewModel.CPictureSet.HomeButton.Selected;
    }
    private void Home_Released(object sender, EventArgs e)
    {
        _chooseModeViewModel.CPictureSet.HomeButton.Current = _chooseModeViewModel.CPictureSet.HomeButton.Default;
    }

    private void Min_Pressed(object sender, EventArgs e)
    {
        _chooseModeViewModel.CPictureSet.SelectModesPicks[1].Current = _chooseModeViewModel.CPictureSet.SelectModesPicks[1].Selected;
    }
    private void Min_Released(object sender, EventArgs e)
    {
        _chooseModeViewModel.CPictureSet.SelectModesPicks[1].Current = _chooseModeViewModel.CPictureSet.SelectModesPicks[1].Default;
    }

    private void Norm_Pressed(object sender, EventArgs e)
    {
        _chooseModeViewModel.CPictureSet.SelectModesPicks[2].Current = _chooseModeViewModel.CPictureSet.SelectModesPicks[2].Selected;
    }
    private void Norm_Released(object sender, EventArgs e)
    {
        _chooseModeViewModel.CPictureSet.SelectModesPicks[2].Current = _chooseModeViewModel.CPictureSet.SelectModesPicks[2].Default;
    }

    private void Max_Pressed(object sender, EventArgs e)
    {
        _chooseModeViewModel.CPictureSet.SelectModesPicks[3].Current = _chooseModeViewModel.CPictureSet.SelectModesPicks[3].Selected;
    }
    private void Max_Released(object sender, EventArgs e)
    {
        _chooseModeViewModel.CPictureSet.SelectModesPicks[3].Current = _chooseModeViewModel.CPictureSet.SelectModesPicks[3].Default;
    }

    private void Kitchen_Pressed(object sender, EventArgs e)
    {
        _chooseModeViewModel.CPictureSet.SelectModesPicks[4].Current = _chooseModeViewModel.CPictureSet.SelectModesPicks[4].Selected;
    }
    private void Kitchen_Released(object sender, EventArgs e)
    {
        _chooseModeViewModel.CPictureSet.SelectModesPicks[4].Current = _chooseModeViewModel.CPictureSet.SelectModesPicks[4].Default;
    }

    private void Vac_Pressed(object sender, EventArgs e)
    {
        _chooseModeViewModel.CPictureSet.SelectModesPicks[5].Current = _chooseModeViewModel.CPictureSet.SelectModesPicks[5].Selected;
    }
    private void Vac_Released(object sender, EventArgs e)
    {
        _chooseModeViewModel.CPictureSet.SelectModesPicks[5].Current = _chooseModeViewModel.CPictureSet.SelectModesPicks[5].Default;
    }

    private void Shed_Pressed(object sender, EventArgs e)
    {
        _chooseModeViewModel.CPictureSet.SelectModesPicks[6].Current = _chooseModeViewModel.CPictureSet.SelectModesPicks[6].Selected;
    }
    private void Shed_Released(object sender, EventArgs e)
    {
        _chooseModeViewModel.CPictureSet.SelectModesPicks[6].Current = _chooseModeViewModel.CPictureSet.SelectModesPicks[6].Default;
    }

    private void TurnOff_Pressed(object sender, EventArgs e)
    {
        _chooseModeViewModel.CPictureSet.SelectModesPicks[0].Current = _chooseModeViewModel.CPictureSet.SelectModesPicks[0].Selected;
    }
    private void TurnOff_Released(object sender, EventArgs e)
    {
        _chooseModeViewModel.CPictureSet.SelectModesPicks[0].Current = _chooseModeViewModel.CPictureSet.SelectModesPicks[0].Default;
    }

}