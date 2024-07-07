

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