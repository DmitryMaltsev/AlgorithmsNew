using Android_Silver.Entities;
using Android_Silver.ViewModels;

using System.Xml.Linq;

namespace Android_Silver.Pages;

public partial class ChooseModePage : ContentPage
{
	private ChooseModeViewModel _chooseModeViewModel;
    ChooseModePage _chPage;
    public ChooseModePage()
	{
		InitializeComponent();
        ImBut1.Pressed -= Home_Pressed;
        ImBut1.Released -= Home_Released;
        ImBut2.Pressed -= Min_Pressed;
        ImBut2.Released -= Min_Released;
        ImBut3.Pressed -= Norm_Pressed;
        ImBut3.Released -= Norm_Released;
        ImBut4.Pressed -= Max_Pressed;
        ImBut4.Released -= Max_Released;
        ImBut5.Pressed -= Kitchen_Pressed;
        ImBut5.Released -= Kitchen_Released;
        //  BindingContext = null;
        ImBut6.Pressed -= Vac_Pressed;
        ImBut6.Released -= Vac_Released;
        ImBut7.Pressed -= Shed_Pressed;
        ImBut7.Released -= Shed_Released;
        ImBut8.Pressed -= TurnOff_Pressed;
        ImBut8.Released -= TurnOff_Released;
        _chPage = this;
        _chooseModeViewModel =new ChooseModeViewModel();
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

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        chModeGrid.Resources.Clear();
    }
}
