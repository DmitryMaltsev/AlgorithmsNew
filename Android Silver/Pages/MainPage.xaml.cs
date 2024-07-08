

using Android_Silver.Entities;

namespace Android_Silver.Pages
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        MainPage _mp;
        private MainPageViewModel _viewModel;
        public MainPage()
        {
          
            InitializeComponent();
           /* im1But.Pressed -= Settings_Pressed;
            im1But.Released -= Settings_Released;
            im2But.Pressed -= ActiveMode_Pressed;
            im2But.Released -= ActiveMode_Released;
            im3But.Pressed -= Substrate_Pressed;
            im3But.Released -= Substrate_Released;
           /* im1But.Pressed += Settings_Pressed;
            im1But.Released += Settings_Released;
            im2But.Pressed += ActiveMode_Pressed;
            im2But.Released += ActiveMode_Released;
            im3But.Pressed += Substrate_Pressed;
            im3But.Released += Substrate_Released;*/
            _viewModel = new MainPageViewModel();
            BindingContext = _viewModel;
          //  _viewModel.Init();
            _mp = this;
        }

        #region Settings
        private void Settings_Pressed(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.SettingsButton.Current = _viewModel.CPictureSet.SettingsButton.Selected;
        }

        private void Settings_Released(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.SettingsButton.Current = _viewModel.CPictureSet.SettingsButton.Default;
        }

        #endregion

        private void Substrate_Pressed(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.Substrate.Current = _viewModel.CPictureSet.Substrate.Selected;
        }

        private void Substrate_Released(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.Substrate.Current = _viewModel.CPictureSet.Substrate.Default;
        }

        private void ActiveMode_Pressed(object sender, EventArgs e)
        {
            _viewModel.CModesEntities.CMode1.ActiveModePics.Current =
                _viewModel.CModesEntities.CMode1.ActiveModePics.Selected;
        }

        private void ActiveMode_Released(object sender, EventArgs e)
        {
            _viewModel.CModesEntities.CMode1.ActiveModePics.Current =
                _viewModel.CModesEntities.CMode1.ActiveModePics.Default;
        }

        private void Home_Pressed(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.HomeButton.Current = _viewModel.CPictureSet.HomeButton.Selected;
        }
        private void Home_Released(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.HomeButton.Current = _viewModel.CPictureSet.HomeButton.Default;
        }

        private void Min_Pressed(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.SelectModesPicks[1].Current = _viewModel.CPictureSet.SelectModesPicks[1].Selected;
        }
        private void Min_Released(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.SelectModesPicks[1].Current = _viewModel.CPictureSet.SelectModesPicks[1].Default;
        }

        private void Norm_Pressed(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.SelectModesPicks[2].Current = _viewModel.CPictureSet.SelectModesPicks[2].Selected;
        }
        private void Norm_Released(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.SelectModesPicks[2].Current = _viewModel.CPictureSet.SelectModesPicks[2].Default;
        }

        private void Max_Pressed(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.SelectModesPicks[3].Current = _viewModel.CPictureSet.SelectModesPicks[3].Selected;
        }
        private void Max_Released(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.SelectModesPicks[3].Current = _viewModel.CPictureSet.SelectModesPicks[3].Default;
        }

        private void Kitchen_Pressed(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.SelectModesPicks[4].Current = _viewModel.CPictureSet.SelectModesPicks[4].Selected;
        }
        private void Kitchen_Released(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.SelectModesPicks[4].Current = _viewModel.CPictureSet.SelectModesPicks[4].Default;
        }

        private void Vac_Pressed(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.SelectModesPicks[5].Current = _viewModel.CPictureSet.SelectModesPicks[5].Selected;
        }
        private void Vac_Released(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.SelectModesPicks[5].Current = _viewModel.CPictureSet.SelectModesPicks[5].Default;
        }

        private void Shed_Pressed(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.SelectModesPicks[6].Current = _viewModel.CPictureSet.SelectModesPicks[6].Selected;
        }
        private void Shed_Released(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.SelectModesPicks[6].Current = _viewModel.CPictureSet.SelectModesPicks[6].Default;
        }

        private void TurnOff_Pressed(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.SelectModesPicks[0].Current = _viewModel.CPictureSet.SelectModesPicks[0].Selected;
        }
        private void TurnOff_Released(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.SelectModesPicks[0].Current = _viewModel.CPictureSet.SelectModesPicks[0].Default;
        }

        #region Kitchen callbacks
        private void UpDigit_Pressed(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.KitchenButtonUp.Current = _viewModel.CPictureSet.KitchenButtonUp.Selected;
        }

        private void UpDigit_Released(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.KitchenButtonUp.Current = _viewModel.CPictureSet.KitchenButtonUp.Default;
        }

        private void DnDigit_Pressed(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.KitchenButtonDn.Current = _viewModel.CPictureSet.KitchenButtonDn.Selected;
        }

        private void DnDigit_Released(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.KitchenButtonDn.Current = _viewModel.CPictureSet.KitchenButtonDn.Default;
        }

        private void Next_Pressed(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.NextButton.Current = _viewModel.CPictureSet.NextButton.Selected;
        }

        private void Next_Released(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.NextButton.Current = _viewModel.CPictureSet.NextButton.Default;
        }

        private void HomeButton_Pressed(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.HomeButton.Current = _viewModel.CPictureSet.HomeButton.Selected;
        }

        private void HomeButton_Released(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.HomeButton.Current = _viewModel.CPictureSet.HomeButton.Default;
        }

        private void AcceptButton_Pressed(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.AcceptButton.Current = _viewModel.CPictureSet.AcceptButton.Selected;
        }

        private void AcceptButton_Released(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.AcceptButton.Current = _viewModel.CPictureSet.AcceptButton.Default;
        }

        private void CancelButton_Pressed(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.CancelButton.Current = _viewModel.CPictureSet.CancelButton.Selected;
        }

        private void CancelButton_Released(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.CancelButton.Current = _viewModel.CPictureSet.CancelButton.Default;
        } 
        #endregion

        protected override void OnAppearing()
        {
            base.OnAppearing();
          
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            mainPageGrid.Resources.Clear();
        }
    }
}