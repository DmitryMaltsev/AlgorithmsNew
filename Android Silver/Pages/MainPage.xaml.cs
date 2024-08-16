

using Android_Silver.Entities;

using Microsoft.Maui.Animations;

using System.Diagnostics;

namespace Android_Silver.Pages
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        MainPage _mp;
        public MainPageViewModel ViewModel { get; set; }
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
            ViewModel = new MainPageViewModel();
            BindingContext = ViewModel;
          //  _viewModel.Init();
            _mp = this;
            var swipeLeft = new SwipeGestureRecognizer();
            swipeLeft.Swiped += OnSwipeLeft;

            var swipeRight = new SwipeGestureRecognizer();
            swipeRight.Swiped += OnSwipeRight;
      
            // Add the gestures to your content 
            //  ContentView.GestureRecognizers.Add(swipeLeft);
            //  ContentView.GestureRecognizers.Add(swipeRight)
        }

        private void OnSwipeRight(object sender, SwipedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnSwipeLeft(object sender, SwipedEventArgs e)
        {
            throw new NotImplementedException();
        }

        #region Settings
        private void Settings_Pressed(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.SettingsButton.Current = ViewModel.CPictureSet.SettingsButton.Selected;
        }

        private void Settings_Released(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.SettingsButton.Current = ViewModel.CPictureSet.SettingsButton.Default;
        }

        #endregion

        private void Substrate_Pressed(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.Substrate.Current = ViewModel.CPictureSet.Substrate.Selected;
        }

        private void Substrate_Released(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.Substrate.Current = ViewModel.CPictureSet.Substrate.Default;
        }

        private void ActiveMode_Pressed(object sender, EventArgs e)
        {
            ViewModel.CModesEntities.CMode1.ActiveModePics.Current =
                ViewModel.CModesEntities.CMode1.ActiveModePics.Selected;
        }

        private void ActiveMode_Released(object sender, EventArgs e)
        {
            ViewModel.CModesEntities.CMode1.ActiveModePics.Current =
                ViewModel.CModesEntities.CMode1.ActiveModePics.Default;
        }

        private void BackButton_Pressed(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.BackButton.Current = ViewModel.CPictureSet.BackButton.Selected;
        }

        private void BackButton_Released(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.BackButton.Current = ViewModel.CPictureSet.BackButton.Default;
        }

        #region Choose modes callbacks
        private void Home_Pressed(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.HomeButton.Current = ViewModel.CPictureSet.HomeButton.Selected;
        }
        private void Home_Released(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.HomeButton.Current = ViewModel.CPictureSet.HomeButton.Default;
        }

        private void Min_Pressed(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.SelectModesPics[1].Current = ViewModel.CPictureSet.SelectModesPics[1].Selected;
        }
        private void Min_Released(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.SelectModesPics[1].Current = ViewModel.CPictureSet.SelectModesPics[1].Default;
        }

        private void Norm_Pressed(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.SelectModesPics[2].Current = ViewModel.CPictureSet.SelectModesPics[2].Selected;
        }
        private void Norm_Released(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.SelectModesPics[2].Current = ViewModel.CPictureSet.SelectModesPics[2].Default;
        }

        private void Max_Pressed(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.SelectModesPics[3].Current = ViewModel.CPictureSet.SelectModesPics[3].Selected;
        }
        private void Max_Released(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.SelectModesPics[3].Current = ViewModel.CPictureSet.SelectModesPics[3].Default;
        }

        private void Kitchen_Pressed(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.SelectModesPics[4].Current = ViewModel.CPictureSet.SelectModesPics[4].Selected;
        }
        private void Kitchen_Released(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.SelectModesPics[4].Current = ViewModel.CPictureSet.SelectModesPics[4].Default;
        }

        private void Vac_Pressed(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.SelectModesPics[5].Current = ViewModel.CPictureSet.SelectModesPics[5].Selected;
        }
        private void Vac_Released(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.SelectModesPics[5].Current = ViewModel.CPictureSet.SelectModesPics[5].Default;
        }

        private void Shed_Pressed(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.SelectModesPics[6].Current = ViewModel.CPictureSet.SelectModesPics[6].Selected;
        }
        private void Shed_Released(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.SelectModesPics[6].Current = ViewModel.CPictureSet.SelectModesPics[6].Default;
        }

        private void TurnOff_Pressed(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.SelectModesPics[0].Current = ViewModel.CPictureSet.SelectModesPics[0].Selected;
        }
        private void TurnOff_Released(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.SelectModesPics[0].Current = ViewModel.CPictureSet.SelectModesPics[0].Default;
        } 
        #endregion

        #region Kitchen callbacks
        private void UpDigit_Pressed(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.KitchenButtonUp.Current = ViewModel.CPictureSet.KitchenButtonUp.Selected;
        }

        private void UpDigit_Released(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.KitchenButtonUp.Current = ViewModel.CPictureSet.KitchenButtonUp.Default;
        }

        private void DnDigit_Pressed(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.KitchenButtonDn.Current = ViewModel.CPictureSet.KitchenButtonDn.Selected;
        }

        private void DnDigit_Released(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.KitchenButtonDn.Current = ViewModel.CPictureSet.KitchenButtonDn.Default;
        }

        private void Next_Pressed(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.NextButton.Current = ViewModel.CPictureSet.NextButton.Selected;
        }

        private void Next_Released(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.NextButton.Current = ViewModel.CPictureSet.NextButton.Default;
        }

        private void HomeButton_Pressed(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.HomeButton.Current = ViewModel.CPictureSet.HomeButton.Selected;
        }

        private void HomeButton_Released(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.HomeButton.Current = ViewModel.CPictureSet.HomeButton.Default;
        }

        private void AcceptButton_Pressed(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.AcceptButton.Current = ViewModel.CPictureSet.AcceptButton.Selected;
        }

        private void AcceptButton_Released(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.AcceptButton.Current = ViewModel.CPictureSet.AcceptButton.Default;
        }

        private void CancelButton_Pressed(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.CancelButton.Current = ViewModel.CPictureSet.CancelButton.Selected;
        }

        private void CancelButton_Released(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.CancelButton.Current = ViewModel.CPictureSet.CancelButton.Default;
        }
        #endregion

        #region SetPointsCallbacks

        private void UpDigit_Pressed0(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.DigitalButtonsUp[0].Current = ViewModel.CPictureSet.DigitalButtonsUp[0].Selected;
        }

        private void UpDigit_Released0(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.DigitalButtonsUp[0].Current = ViewModel.CPictureSet.DigitalButtonsUp[0].Default;
        }

        private void UpDigit_Pressed1(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.DigitalButtonsUp[1].Current = ViewModel.CPictureSet.DigitalButtonsUp[1].Selected;
        }

        private void UpDigit_Released1(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.DigitalButtonsUp[1].Current = ViewModel.CPictureSet.DigitalButtonsUp[1].Default;
        }

        private void UpDigit_Pressed2(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.DigitalButtonsUp[2].Current = ViewModel.CPictureSet.DigitalButtonsUp[2].Selected;
        }

        private void UpDigit_Released2(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.DigitalButtonsUp[2].Current = ViewModel.CPictureSet.DigitalButtonsUp[2].Default;
        }

        private void UpDigit_Pressed3(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.DigitalButtonsUp[3].Current = ViewModel.CPictureSet.DigitalButtonsUp[3].Selected;
        }

        private void UpDigit_Released3(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.DigitalButtonsUp[3].Current = ViewModel.CPictureSet.DigitalButtonsUp[3].Default;
        }

        private void UpDigit_Pressed4(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.DigitalButtonsUp[4].Current = ViewModel.CPictureSet.DigitalButtonsUp[3].Selected;
        }

        private void UpDigit_Released4(object sender, EventArgs e)
        {
            Button button = new Button();
            ViewModel.CPictureSet.DigitalButtonsUp[4].Current = ViewModel.CPictureSet.DigitalButtonsUp[3].Default;
        }

        private void DnDigit_Pressed0(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.DigitalButtonsDn[0].Current = ViewModel.CPictureSet.DigitalButtonsDn[0].Selected;
        }

        private void DnDigit_Released0(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.DigitalButtonsDn[0].Current = ViewModel.CPictureSet.DigitalButtonsDn[0].Default;
        }


        private void DnDigit_Pressed1(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.DigitalButtonsDn[1].Current = ViewModel.CPictureSet.DigitalButtonsDn[1].Selected;
        }

        private void DnDigit_Released1(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.DigitalButtonsDn[1].Current = ViewModel.CPictureSet.DigitalButtonsDn[1].Default;
        }


        private void DnDigit_Pressed2(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.DigitalButtonsDn[2].Current = ViewModel.CPictureSet.DigitalButtonsDn[2].Selected;
        }

        private void DnDigit_Released2(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.DigitalButtonsDn[2].Current = ViewModel.CPictureSet.DigitalButtonsDn[2].Default;
        }


        private void DnDigit_Pressed3(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.DigitalButtonsDn[3].Current = ViewModel.CPictureSet.DigitalButtonsDn[3].Selected;
        }

        private void DnDigit_Released3(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.DigitalButtonsDn[3].Current = ViewModel.CPictureSet.DigitalButtonsDn[3].Default;
        }

        private void DnDigit_Pressed4(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.DigitalButtonsDn[4].Current = ViewModel.CPictureSet.DigitalButtonsDn[3].Selected;
        }

        private void DnDigit_Released4(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.DigitalButtonsDn[4].Current = ViewModel.CPictureSet.DigitalButtonsDn[3].Default;
        }

        private void Next_SPPressed(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.NextButton.Current = ViewModel.CPictureSet.NextButton.Selected;
        }

        private void Next_SPReleased(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.NextButton.Current = ViewModel.CPictureSet.NextButton.Default;
        }

        private void HomeButton_SPPressed(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.HomeButton.Current = ViewModel.CPictureSet.HomeButton.Selected;
        }

        private void HomeButton_SPReleased(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.HomeButton.Current = ViewModel.CPictureSet.HomeButton.Default;
        }

        private void OK_SPPressed(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.OkButton.Current = ViewModel.CPictureSet.OkButton.Selected;
        }

        private void Ok_SPReleased(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.OkButton.Current = ViewModel.CPictureSet.OkButton.Default;
        }


        #endregion

        #region SETTINGS PAGE
        private void Journal_Pressed(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.JournalBut.Current = ViewModel.CPictureSet.JournalBut.Selected;
        }

        private void Journal_Released(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.JournalBut.Current = ViewModel.CPictureSet.JournalBut.Default;
        }

        private void OtherSettings_Pressed(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.SettingsOtherBut.Current = ViewModel.CPictureSet.SettingsOtherBut.Selected;
        }

        private void OtherSettings_Released(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.SettingsOtherBut.Current = ViewModel.CPictureSet.SettingsOtherBut.Default;
        }

        private void SPSettings_Pressed(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.ModesSetBut.Current = ViewModel.CPictureSet.ModesSetBut.Selected;
        }

        private void SPSettings_Released(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.ModesSetBut.Current = ViewModel.CPictureSet.ModesSetBut.Default;
        }

        private void ShedSettings_Pressed(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.ShedSetBut.Current = ViewModel.CPictureSet.ShedSetBut.Selected;
        }

        private void ShedSettings_Released(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.ShedSetBut.Current = ViewModel.CPictureSet.ShedSetBut.Default;
        }

        private void VacSettings_Pressed(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.VacSetBut.Current = ViewModel.CPictureSet.VacSetBut.Selected;
        }

        private void VacSettings_Released(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.VacSetBut.Current = ViewModel.CPictureSet.VacSetBut.Default;
        }
        #endregion
      
        #region  JOURNAL PAGE
        private void ResetJournal_Pressed(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.ResetButton.Current = ViewModel.CPictureSet.VacSetBut.Selected;
        }

        private void ResetJournal_Released(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.ResetButton.Current = ViewModel.CPictureSet.ResetButton.Default;
        }
        #endregion

        #region VacPage
        private void VacPage_Pressed(object sender, EventArgs e)
        {
            for (int i = 0; i < ViewModel.CModesEntities.Mode2ValuesList[2].TimeModeValues.Count; i++)
            {
                ViewModel.CModesEntities.Mode2ValuesList[2].TimeModeValues[i].StrokeImg.Current =
               ViewModel.CModesEntities.Mode2ValuesList[2].TimeModeValues[1].StrokeImg.Default;
            }
            ImageButton clickedButton = (ImageButton)sender;
            int index =(int)clickedButton.CommandParameter;
            ViewModel.CModesEntities.Mode2ValuesList[2].TimeModeValues[index - 1].StrokeImg.Current =
                 ViewModel.CModesEntities.Mode2ValuesList[2].TimeModeValues[index - 1].StrokeImg.Selected;
        }
        #endregion

        #region OtherSettings Page
        private void FilterChanged_Pressed(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.FilterChangedBut.Current = ViewModel.CPictureSet.FilterChangedBut.Selected;
        }

        private void FilterChanged_Released(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.FilterChangedBut.Current = ViewModel.CPictureSet.FilterChangedBut.Default;
        }
        private void Time_Pressed(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.TimeBut.Current = ViewModel.CPictureSet.TimeBut.Selected;
        }

        private void Time_Released(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.TimeBut.Current = ViewModel.CPictureSet.TimeBut.Default;
        }

        private void Humidity_Pressed(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.HumidityBut.Current = ViewModel.CPictureSet.HumidityBut.Selected;
        }

        private void Humidity_Released(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.HumidityBut.Current = ViewModel.CPictureSet.HumidityBut.Default;
        }

        private void ArrowButLeft_Pressed(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.ArrowButLeft.Current = ViewModel.CPictureSet.ArrowButLeft.Selected;
        }

        private void ArrowButLeft_Released(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.ArrowButLeft.Current = ViewModel.CPictureSet.ArrowButLeft.Default;
        }

        private void ArrowButRight_Pressed(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.ArrowButRight.Current = ViewModel.CPictureSet.ArrowButRight.Selected;
        }

        private void ArrowButRight_Released(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.ArrowButRight.Current = ViewModel.CPictureSet.ArrowButRight.Default;
        }

        #endregion

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private void OnSwiped(object sender, SwipedEventArgs e)
        {

        }
    }
}