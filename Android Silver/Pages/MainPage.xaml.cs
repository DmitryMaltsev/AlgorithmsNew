

namespace Android_Silver.Pages
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        private MainPageViewModel _viewModel;
        public MainPage()
        {
            InitializeComponent();
            _viewModel = new MainPageViewModel();
            BindingContext = _viewModel;
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

        private void Settings_Unfocused(object sender, FocusEventArgs e)
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

    }
}