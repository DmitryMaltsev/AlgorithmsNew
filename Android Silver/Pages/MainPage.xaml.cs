

using Android_Silver.Entities;

using Microsoft.Maui.Animations;

using System.Diagnostics;

namespace Android_Silver.Pages
{
    public partial class MainPage : ContentPage
    {
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
            // Add the gestures to your content 
            //  ContentView.GestureRecognizers.Add(swipeLeft);
            //  ContentView.GestureRecognizers.Add(swipeRight)

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

        private void ActiveMode_Pressed(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.Mode1Select.Current =
                ViewModel.CPictureSet.Mode1Select.Selected;
        }

        private void ActiveMode_Released(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.Mode1Select.Current =
               ViewModel.CPictureSet.Mode1Select.Default;
        }

        private void BackButton_Pressed(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.BackButton.Current = ViewModel.CPictureSet.BackButton.Selected;
        }

        private void BackButton_Released(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.BackButton.Current = ViewModel.CPictureSet.BackButton.Default;
        }
        #region Start page callbacks
        private void IPButton_Pressed(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.IPBut.Current = ViewModel.CPictureSet.IPBut.Selected;
        }

        private void IPButton_Released(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.IPBut.Current = ViewModel.CPictureSet.IPBut.Default;
        }



        #endregion

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

        private void TurnOff_Pressed(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.SelectModesPics[0].Current = ViewModel.CPictureSet.SelectModesPics[0].Selected;
        }
        private void TurnOff_Released(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.SelectModesPics[0].Current = ViewModel.CPictureSet.SelectModesPics[0].Default;
        }

        private void ShedSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            Microsoft.Maui.Controls.Switch thisSwitch = sender as Microsoft.Maui.Controls.Switch;
            thisSwitch.CancelAnimations();
            if (thisSwitch != null && thisSwitch.IsToggled)
                ViewModel.CPictureSet.SelectModesPics[6].Current = ViewModel.CPictureSet.SelectModesPics[6].Selected;
            else
                ViewModel.CPictureSet.SelectModesPics[6].Current = ViewModel.CPictureSet.SelectModesPics[6].Default;
        }

        private void Sheduler_Pressed(object sender, EventArgs e)
        {
            if (ViewModel.CPictureSet.SelectModesPics[6].Current == ViewModel.CPictureSet.SelectModesPics[6].Default)
                ViewModel.CPictureSet.SelectModesPics[6].Current = ViewModel.CPictureSet.SelectModesPics[6].Selected;
            else
                ViewModel.CPictureSet.SelectModesPics[6].Current = ViewModel.CPictureSet.SelectModesPics[6].Default;
        }
        #endregion

        #region Kitchen callbacks

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


        #endregion

        #region SetPointsCallbacks

        private void Next_SPPressed(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.NextButton.Current = ViewModel.CPictureSet.NextButton.Selected;
        }

        private void Next_SPReleased(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.NextButton.Current = ViewModel.CPictureSet.NextButton.Default;
        }

        private void Prev_SPPressed(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.PrevButton.Current = ViewModel.CPictureSet.PrevButton.Selected;
        }

        private void Prev_SPReleased(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.PrevButton.Current = ViewModel.CPictureSet.PrevButton.Default;
        }

        private void Next_SPPressed1(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.NextButton1.Current = ViewModel.CPictureSet.NextButton1.Selected;
        }

        private void Next_SPReleased1(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.NextButton1.Current = ViewModel.CPictureSet.NextButton1.Default;
        }

        private void Prev_SPPressed1(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.PrevButton1.Current = ViewModel.CPictureSet.PrevButton1.Selected;
        }

        private void Prev_SPReleased1(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.PrevButton1.Current = ViewModel.CPictureSet.PrevButton1.Default;
        }

        private void AddBut_Pressed0(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.AddButs[0].Current = ViewModel.CPictureSet.AddButs[0].Selected;
            ViewModel.AddBut0Timer.Start();
        }

        private void AddBut_Released0(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.AddButs[0].Current = ViewModel.CPictureSet.AddButs[0].Default;
            ViewModel.AddBut0Timer.Stop();
        }

        private void SubBut_Pressed0(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.SubButs[0].Current = ViewModel.CPictureSet.SubButs[0].Selected;
            ViewModel.SubBut0Timer.Start();
        }

        private void SubBut_Released0(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.SubButs[0].Current = ViewModel.CPictureSet.SubButs[0].Default;
            ViewModel.SubBut0Timer.Stop();
        }

        private void AddBut_Pressed1(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.AddButs[1].Current = ViewModel.CPictureSet.AddButs[1].Selected;
            ViewModel.AddBut1Timer.Start();
        }

        private void AddBut_Released1(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.AddButs[1].Current = ViewModel.CPictureSet.AddButs[1].Default;
            ViewModel.AddBut1Timer.Stop();
        }

        private void SubBut_Pressed1(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.SubButs[1].Current = ViewModel.CPictureSet.SubButs[1].Selected;
            ViewModel.SubBut1Timer.Start();
        }

        private void SubBut_Released1(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.SubButs[1].Current = ViewModel.CPictureSet.SubButs[1].Default;
            ViewModel.SubBut1Timer.Stop();
        }

        private void AddBut_Pressed2(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.AddButs[2].Current = ViewModel.CPictureSet.AddButs[2].Selected;
            ViewModel.AddBut2Timer.Start();
        }

        private void AddBut_Released2(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.AddButs[2].Current = ViewModel.CPictureSet.AddButs[2].Default;
            ViewModel.AddBut2Timer.Stop();
        }

        private void SubBut_Pressed2(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.SubButs[2].Current = ViewModel.CPictureSet.SubButs[2].Selected;
            ViewModel.SubBut2Timer.Start();
        }

        private void SubBut_Released2(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.SubButs[2].Current = ViewModel.CPictureSet.SubButs[2].Default;
            ViewModel.SubBut2Timer.Stop();
        }


        private void AddBut_Pressed3(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.AddButs[3].Current = ViewModel.CPictureSet.AddButs[3].Selected;
            ViewModel.AddBut3Timer.Start();
        }

        private void AddBut_Released3(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.AddButs[3].Current = ViewModel.CPictureSet.AddButs[3].Default;
            ViewModel.AddBut3Timer.Stop();
        }


        private void SubBut_Pressed3(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.SubButs[3].Current = ViewModel.CPictureSet.SubButs[3].Selected;
            ViewModel.SubBut3Timer.Start();
        }

        private void SubBut_Released3(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.SubButs[3].Current = ViewModel.CPictureSet.SubButs[3].Default;
            ViewModel.SubBut3Timer.Stop();
        }


        private void AddBut_Pressed4(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.AddButs[4].Current = ViewModel.CPictureSet.AddButs[4].Selected;
            ViewModel.AddBut4Timer.Start();
        }

        private void Addbut_Released4(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.AddButs[4].Current = ViewModel.CPictureSet.AddButs[4].Default;
            ViewModel.AddBut4Timer.Stop();
        }

        private void SubBut_Pressed4(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.SubButs[4].Current = ViewModel.CPictureSet.SubButs[4].Selected;
            ViewModel.SubBut4Timer.Start();
        }

        private void SubBut_Released4(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.SubButs[4].Current = ViewModel.CPictureSet.SubButs[4].Default;
            ViewModel.SubBut4Timer.Stop();
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

        private void ShedSettings_Pressed(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.ShedSetBut.Current = ViewModel.CPictureSet.ShedSetBut.Selected;
        }

        private void ShedSettings_Released(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.ShedSetBut.Current = ViewModel.CPictureSet.ShedSetBut.Default;
        }

        private void SPSettings_Pressed(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.ModesSetBut.Current = ViewModel.CPictureSet.ModesSetBut.Selected;
        }

        private void SPSettings_Released(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.ModesSetBut.Current = ViewModel.CPictureSet.ModesSetBut.Default;
        }

        private void OtherSettings_Pressed(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.SettingsOtherBut.Current = ViewModel.CPictureSet.SettingsOtherBut.Selected;
        }

        private void OtherSettings_Released(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.SettingsOtherBut.Current = ViewModel.CPictureSet.SettingsOtherBut.Default;
        }
        private void InformationSettings_Pressed(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.InformationBut.Current = ViewModel.CPictureSet.InformationBut.Selected;
        }

        private void InformationSettings_Released(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.InformationBut.Current = ViewModel.CPictureSet.InformationBut.Default;
        }

        private void Journal_Pressed(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.JournalBut.Current = ViewModel.CPictureSet.JournalBut.Selected;
        }

        private void Journal_Released(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.JournalBut.Current = ViewModel.CPictureSet.JournalBut.Default;
        }




        private void HelperSettings_Pressed(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.HelperBut.Current = ViewModel.CPictureSet.HelperBut.Selected;
        }

        private void HelperSettings_Released(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.HelperBut.Current = ViewModel.CPictureSet.HelperBut.Default;
        }

        #endregion

        #region  JOURNAL PAGE
        private void ResetJournal_Pressed(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.ResetButton.Current = ViewModel.CPictureSet.ResetButton.Selected;
        }

        private void ResetJournal_Released(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.ResetButton.Current = ViewModel.CPictureSet.ResetButton.Default;
        }
        #endregion

        #region VacPage
        private void VacPage_Pressed(object sender, EventArgs e)
        {
            for (int i = 0; i < ViewModel.CModesEntities.CTimeModeValues.Count; i++)
            {
                ViewModel.CModesEntities.CTimeModeValues[i].StrokeImg.Current =
                 ViewModel.CModesEntities.CTimeModeValues[i].StrokeImg.Default;
            }
            ImageButton clickedButton = (ImageButton)sender;
            int index = (int)clickedButton.CommandParameter;
            int m2Num = index / 100;
            int tModeNum = index % 100;
            ViewModel.CModesEntities.Mode2ValuesList[m2Num].TimeModeValues[tModeNum - 1].StrokeImg.Current =
                 ViewModel.CModesEntities.Mode2ValuesList[m2Num].TimeModeValues[m2Num - 1].StrokeImg.Selected;
        }
        #endregion

        #region OtherSettings Page
        private void ClockSettings_Pressed(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.ClocksSettingsBut.Current = ViewModel.CPictureSet.ClocksSettingsBut.Selected;
        }

        private void ClockSettings_Released(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.ClocksSettingsBut.Current = ViewModel.CPictureSet.ClocksSettingsBut.Default;
        }

        private void UpdaterSettings_Pressed(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.UpdaterSettingsBut.Current = ViewModel.CPictureSet.UpdaterSettingsBut.Selected;
        }

        private void UpdaterSettings_Released(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.UpdaterSettingsBut.Current = ViewModel.CPictureSet.UpdaterSettingsBut.Default;
        }

        private void CorrSettings_Pressed(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.CorrSettingsBut.Current = ViewModel.CPictureSet.CorrSettingsBut.Selected;
        }

        private void CorrSettings_Released(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.CorrSettingsBut.Current = ViewModel.CPictureSet.CorrSettingsBut.Default;
        }

        private void Humidity_Pressed(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.CorrSettingsBut.Current = ViewModel.CPictureSet.CorrSettingsBut.Selected;
        }

        private void Humidity_Released(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.CorrSettingsBut.Current = ViewModel.CPictureSet.CorrSettingsBut.Default;
        }

        #endregion

        #region Updater
        private void Download_Pressed(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.DownloadBut.Current = ViewModel.CPictureSet.DownloadBut.Selected;
        }

        private void Download_Released(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.DownloadBut.Current = ViewModel.CPictureSet.DownloadBut.Default;
        }

        private void Updater_Pressed(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.UpdaterBut.Current = ViewModel.CPictureSet.ResetBut.Selected;
        }

        private void Updater_Released(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.UpdaterBut.Current = ViewModel.CPictureSet.ResetBut.Default;
        }

        private void Reset_Pressed(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.ResetBut.Current = ViewModel.CPictureSet.ResetBut.Selected;
        }

        private void Reset_Released(object sender, EventArgs e)
        {
            ViewModel.CPictureSet.ResetBut.Current = ViewModel.CPictureSet.ResetBut.Default;
        }

        #endregion



        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            Picker picker = sender as Picker;
            picker.Unfocus();
        }


    }

}