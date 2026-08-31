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




   


    private void Next_Pressed(object sender, EventArgs e)
    {
        _spViewModel.CPictureSet.NextButton.Current = _spViewModel.CPictureSet.NextButton.Selected;
    }

    private void Next_Released(object sender, EventArgs e)
    {
        _spViewModel.CPictureSet.NextButton.Current = _spViewModel.CPictureSet.NextButton.Default;
    }

    private void HomeButton_Pressed(object sender, EventArgs e)
    {
        _spViewModel.CPictureSet.HomeButton.Current = _spViewModel.CPictureSet.HomeButton.Selected;
    }

    private void HomeButton_Released(object sender, EventArgs e)
    {
        _spViewModel.CPictureSet.HomeButton.Current = _spViewModel.CPictureSet.HomeButton.Default;
    }


}