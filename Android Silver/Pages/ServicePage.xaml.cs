using Android_Silver.ViewModels;

namespace Android_Silver.Pages;

public partial class ServicePage : ContentPage
{
    ServicePageViewModel _servicePageViewModel;
	public ServicePage()
	{
		InitializeComponent();
        _servicePageViewModel = new ServicePageViewModel();
        BindingContext = _servicePageViewModel;
    }




    private void BackButton_Pressed(object sender, EventArgs e)
    {
        _servicePageViewModel.CPictureSet.BackButton.Current = _servicePageViewModel.CPictureSet.BackButton.Selected;
    }

    private void BackButton_Released(object sender, EventArgs e)
    {
        _servicePageViewModel.CPictureSet.BackButton.Current = _servicePageViewModel.CPictureSet.BackButton.Default;
    }

    private void HomeButton_Pressed(object sender, EventArgs e)
    {
        _servicePageViewModel.CPictureSet.HomeButton.Current = _servicePageViewModel.CPictureSet.HomeButton.Selected;
    }

    private void HomeButton_Released(object sender, EventArgs e)
    {
        _servicePageViewModel.CPictureSet.HomeButton.Current = _servicePageViewModel.CPictureSet.HomeButton.Default;
    }


    private void IPButton_Pressed(object sender, EventArgs e)
    {
        _servicePageViewModel.CPictureSet.IPBut.Current = _servicePageViewModel.CPictureSet.IPBut.Selected;
    }

    private void IPButton_Released(object sender, EventArgs e)
    {
        _servicePageViewModel.CPictureSet.IPBut.Current = _servicePageViewModel.CPictureSet.IPBut.Default;
    }

    #region Основные настойки
    private void BaseSettings1Pressed(object sender, EventArgs e)
    {
        _servicePageViewModel.CPictureSet.BaseSettings1But.Current = _servicePageViewModel.CPictureSet.BaseSettings1But.Selected;
    }

    private void BaseSettings1Released(object sender, EventArgs e)
    {
        _servicePageViewModel.CPictureSet.BaseSettings1But.Current = _servicePageViewModel.CPictureSet.BaseSettings1But.Default;
    }

    private void BaseSettings2Pressed(object sender, EventArgs e)
    {
        _servicePageViewModel.CPictureSet.BaseSettings2But.Current = _servicePageViewModel.CPictureSet.BaseSettings2But.Selected;
    }

    private void BaseSettings2Released(object sender, EventArgs e)
    {
        _servicePageViewModel.CPictureSet.BaseSettings2But.Current = _servicePageViewModel.CPictureSet.BaseSettings2But.Default;
    }

    #endregion

}