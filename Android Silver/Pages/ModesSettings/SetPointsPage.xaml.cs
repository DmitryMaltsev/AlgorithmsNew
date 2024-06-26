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

    private void UpDigit_Pressed0(object sender, EventArgs e)
    {
        _spViewModel.CPictureSet.DigitalButtonsUp[0].Current = _spViewModel.CPictureSet.DigitalButtonsUp[0].Selected;
    }

    private void UpDigit_Released0(object sender, EventArgs e)
    {
        _spViewModel.CPictureSet.DigitalButtonsUp[0].Current = _spViewModel.CPictureSet.DigitalButtonsUp[0].Default;
    }

    private void UpDigit_Pressed1(object sender, EventArgs e)
    {
        _spViewModel.CPictureSet.DigitalButtonsUp[1].Current = _spViewModel.CPictureSet.DigitalButtonsUp[1].Selected;
    }

    private void UpDigit_Released1(object sender, EventArgs e)
    {
        _spViewModel.CPictureSet.DigitalButtonsUp[1].Current = _spViewModel.CPictureSet.DigitalButtonsUp[1].Default;
    }

    private void UpDigit_Pressed2(object sender, EventArgs e)
    {
        _spViewModel.CPictureSet.DigitalButtonsUp[2].Current = _spViewModel.CPictureSet.DigitalButtonsUp[2].Selected;
    }

    private void UpDigit_Released2(object sender, EventArgs e)
    {
        _spViewModel.CPictureSet.DigitalButtonsUp[2].Current = _spViewModel.CPictureSet.DigitalButtonsUp[2].Default;
    }

    private void UpDigit_Pressed3(object sender, EventArgs e)
    {
        _spViewModel.CPictureSet.DigitalButtonsUp[3].Current = _spViewModel.CPictureSet.DigitalButtonsUp[3].Selected;
    }

    private void UpDigit_Released3(object sender, EventArgs e)
    {
        _spViewModel.CPictureSet.DigitalButtonsUp[3].Current = _spViewModel.CPictureSet.DigitalButtonsUp[3].Default;
    }



    private void DnDigit_Pressed0(object sender, EventArgs e)
    {
        _spViewModel.CPictureSet.DigitalButtonsDn[0].Current = _spViewModel.CPictureSet.DigitalButtonsDn[0].Selected;
    }

    private void DnDigit_Released0(object sender, EventArgs e)
    {
        _spViewModel.CPictureSet.DigitalButtonsDn[0].Current = _spViewModel.CPictureSet.DigitalButtonsDn[0].Default;
    }


    private void DnDigit_Pressed1(object sender, EventArgs e)
    {
        _spViewModel.CPictureSet.DigitalButtonsDn[1].Current = _spViewModel.CPictureSet.DigitalButtonsDn[1].Selected;
    }

    private void DnDigit_Released1(object sender, EventArgs e)
    {
        _spViewModel.CPictureSet.DigitalButtonsDn[1].Current = _spViewModel.CPictureSet.DigitalButtonsDn[1].Default;
    }


    private void DnDigit_Pressed2(object sender, EventArgs e)
    {
        _spViewModel.CPictureSet.DigitalButtonsDn[2].Current = _spViewModel.CPictureSet.DigitalButtonsDn[2].Selected;
    }

    private void DnDigit_Released2(object sender, EventArgs e)
    {
        _spViewModel.CPictureSet.DigitalButtonsDn[2].Current = _spViewModel.CPictureSet.DigitalButtonsDn[2].Default;
    }


    private void DnDigit_Pressed3(object sender, EventArgs e)
    {
        _spViewModel.CPictureSet.DigitalButtonsDn[3].Current = _spViewModel.CPictureSet.DigitalButtonsDn[3].Selected;
    }

    private void DnDigit_Released3(object sender, EventArgs e)
    {
        _spViewModel.CPictureSet.DigitalButtonsDn[3].Current = _spViewModel.CPictureSet.DigitalButtonsDn[3].Default;
    }
}