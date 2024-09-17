using Android_Silver.Entities.Visual.Menus;
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
        Button item = sender as Button;
        MItem mItem = item.BindingContext as MItem;
        _servicePageViewModel.CPictureSet.BaseSettings1ButCollection[mItem.ID-1].Current = _servicePageViewModel.CPictureSet.BaseSettings1ButCollection[mItem.ID - 1].Selected;
    }

    private void BaseSettings1Released(object sender, EventArgs e)
    {
        Button item = sender as Button;
        MItem mItem = item.BindingContext as MItem;
        _servicePageViewModel.CPictureSet.BaseSettings1ButCollection[mItem.ID - 1].Current = _servicePageViewModel.CPictureSet.BaseSettings1ButCollection[mItem.ID - 1].Default;
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

    private void Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        Picker picker = sender as Picker;
        picker.Unfocus();
     
    }
}