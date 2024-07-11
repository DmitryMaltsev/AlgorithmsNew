

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

        #region Choose modes callbacks
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
            _viewModel.CPictureSet.SelectModesPics[1].Current = _viewModel.CPictureSet.SelectModesPics[1].Selected;
        }
        private void Min_Released(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.SelectModesPics[1].Current = _viewModel.CPictureSet.SelectModesPics[1].Default;
        }

        private void Norm_Pressed(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.SelectModesPics[2].Current = _viewModel.CPictureSet.SelectModesPics[2].Selected;
        }
        private void Norm_Released(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.SelectModesPics[2].Current = _viewModel.CPictureSet.SelectModesPics[2].Default;
        }

        private void Max_Pressed(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.SelectModesPics[3].Current = _viewModel.CPictureSet.SelectModesPics[3].Selected;
        }
        private void Max_Released(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.SelectModesPics[3].Current = _viewModel.CPictureSet.SelectModesPics[3].Default;
        }

        private void Kitchen_Pressed(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.SelectModesPics[4].Current = _viewModel.CPictureSet.SelectModesPics[4].Selected;
        }
        private void Kitchen_Released(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.SelectModesPics[4].Current = _viewModel.CPictureSet.SelectModesPics[4].Default;
        }

        private void Vac_Pressed(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.SelectModesPics[5].Current = _viewModel.CPictureSet.SelectModesPics[5].Selected;
        }
        private void Vac_Released(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.SelectModesPics[5].Current = _viewModel.CPictureSet.SelectModesPics[5].Default;
        }

        private void Shed_Pressed(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.SelectModesPics[6].Current = _viewModel.CPictureSet.SelectModesPics[6].Selected;
        }
        private void Shed_Released(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.SelectModesPics[6].Current = _viewModel.CPictureSet.SelectModesPics[6].Default;
        }

        private void TurnOff_Pressed(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.SelectModesPics[0].Current = _viewModel.CPictureSet.SelectModesPics[0].Selected;
        }
        private void TurnOff_Released(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.SelectModesPics[0].Current = _viewModel.CPictureSet.SelectModesPics[0].Default;
        } 
        #endregion

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


        #region SetPointsCallbacks

        private void UpDigit_Pressed0(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.DigitalButtonsUp[0].Current = _viewModel.CPictureSet.DigitalButtonsUp[0].Selected;
        }

        private void UpDigit_Released0(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.DigitalButtonsUp[0].Current = _viewModel.CPictureSet.DigitalButtonsUp[0].Default;
        }

        private void UpDigit_Pressed1(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.DigitalButtonsUp[1].Current = _viewModel.CPictureSet.DigitalButtonsUp[1].Selected;
        }

        private void UpDigit_Released1(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.DigitalButtonsUp[1].Current = _viewModel.CPictureSet.DigitalButtonsUp[1].Default;
        }

        private void UpDigit_Pressed2(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.DigitalButtonsUp[2].Current = _viewModel.CPictureSet.DigitalButtonsUp[2].Selected;
        }

        private void UpDigit_Released2(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.DigitalButtonsUp[2].Current = _viewModel.CPictureSet.DigitalButtonsUp[2].Default;
        }

        private void UpDigit_Pressed3(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.DigitalButtonsUp[3].Current = _viewModel.CPictureSet.DigitalButtonsUp[3].Selected;
        }

        private void UpDigit_Released3(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.DigitalButtonsUp[3].Current = _viewModel.CPictureSet.DigitalButtonsUp[3].Default;
        }



        private void DnDigit_Pressed0(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.DigitalButtonsDn[0].Current = _viewModel.CPictureSet.DigitalButtonsDn[0].Selected;
        }

        private void DnDigit_Released0(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.DigitalButtonsDn[0].Current = _viewModel.CPictureSet.DigitalButtonsDn[0].Default;
        }


        private void DnDigit_Pressed1(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.DigitalButtonsDn[1].Current = _viewModel.CPictureSet.DigitalButtonsDn[1].Selected;
        }

        private void DnDigit_Released1(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.DigitalButtonsDn[1].Current = _viewModel.CPictureSet.DigitalButtonsDn[1].Default;
        }


        private void DnDigit_Pressed2(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.DigitalButtonsDn[2].Current = _viewModel.CPictureSet.DigitalButtonsDn[2].Selected;
        }

        private void DnDigit_Released2(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.DigitalButtonsDn[2].Current = _viewModel.CPictureSet.DigitalButtonsDn[2].Default;
        }


        private void DnDigit_Pressed3(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.DigitalButtonsDn[3].Current = _viewModel.CPictureSet.DigitalButtonsDn[3].Selected;
        }

        private void DnDigit_Released3(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.DigitalButtonsDn[3].Current = _viewModel.CPictureSet.DigitalButtonsDn[3].Default;
        }


        private void Next_SPPressed(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.NextButton.Current = _viewModel.CPictureSet.NextButton.Selected;
        }

        private void Next_SPReleased(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.NextButton.Current = _viewModel.CPictureSet.NextButton.Default;
        }

        private void HomeButton_SPPressed(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.HomeButton.Current = _viewModel.CPictureSet.HomeButton.Selected;
        }

        private void HomeButton_SPReleased(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.HomeButton.Current = _viewModel.CPictureSet.HomeButton.Default;
        }

        private void OK_SPPressed(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.OkButton.Current = _viewModel.CPictureSet.OkButton.Selected;
        }

        private void Ok_SPReleased(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.OkButton.Current = _viewModel.CPictureSet.OkButton.Default;
        }


        #endregion

        #region SETTINGS PAGE
        private void Journal_Pressed(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.JournalBut.Current = _viewModel.CPictureSet.JournalBut.Selected;
        }

        private void Journal_Released(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.JournalBut.Current = _viewModel.CPictureSet.JournalBut.Default;
        }

        private void OtherSettings_Pressed(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.SettingsOtherBut.Current = _viewModel.CPictureSet.SettingsOtherBut.Selected;
        }

        private void OtherSettings_Released(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.SettingsOtherBut.Current = _viewModel.CPictureSet.SettingsOtherBut.Default;
        }

        private void SPSettings_Pressed(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.ModesSetBut.Current = _viewModel.CPictureSet.ModesSetBut.Selected;
        }

        private void SPSettings_Released(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.ModesSetBut.Current = _viewModel.CPictureSet.ModesSetBut.Default;
        }

        private void ShedSettings_Pressed(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.ShedSetBut.Current = _viewModel.CPictureSet.ShedSetBut.Selected;
        }

        private void ShedSettings_Released(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.ShedSetBut.Current = _viewModel.CPictureSet.ShedSetBut.Default;
        }

        private void VacSettings_Pressed(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.VacSetBut.Current = _viewModel.CPictureSet.VacSetBut.Selected;
        }

        private void VacSettings_Released(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.VacSetBut.Current = _viewModel.CPictureSet.VacSetBut.Default;
        }
        #endregion
      
        #region  JOURNAL PAGE
        private void ResetJournal_Pressed(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.ResetButton.Current = _viewModel.CPictureSet.VacSetBut.Selected;
        }

        private void ResetJournal_Released(object sender, EventArgs e)
        {
            _viewModel.CPictureSet.ResetButton.Current = _viewModel.CPictureSet.ResetButton.Default;
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