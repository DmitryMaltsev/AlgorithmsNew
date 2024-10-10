using Android_Silver.Entities;
using Android_Silver.Pages;
using Android_Silver.Pages.ModesSettings;
using Android_Silver.ViewModels;

using Microsoft.Maui.Controls;

using System.ComponentModel;

namespace Android_Silver
{
    public partial class AppShell : Shell
    {
        EthernetEntities _ethernetEntities;
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("servicePage", typeof(ServicePage));
            Routing.RegisterRoute("startPage", typeof(StartPage));
            Routing.RegisterRoute("chooseModePage", typeof(ChooseModePage));
            Routing.RegisterRoute("kitchenTimerPage", typeof(KitchenTimerPage));
            Routing.RegisterRoute("vacationSettingsPage", typeof(VacationSettingsPage));
            Routing.RegisterRoute("loadingPage",typeof(LoadingPage));
            #region ModesSettings pages
            Routing.RegisterRoute("setPointsPage", typeof(SetPointsPage));
            #endregion
            Routing.RegisterRoute("settingsPage", typeof(SettingsPage));
            Routing.RegisterRoute("mainPage", typeof(MainPage));
            _ethernetEntities=DIContainer.Resolve<EthernetEntities>();
        }

  
        private void ShellContent_Appearing(object sender, EventArgs e)
        {
          //  MainPageViewModel mpViewModel=new MainPageViewModel();
         //       mpViewModel.SetActivePageIfNeed();
                _ethernetEntities.PagesTab = 0;
            Element view = Routing.GetOrCreateContent("mainPage");
            MainPageViewModel mainPageViewModel = view.BindingContext as MainPageViewModel;
            if (mainPageViewModel != null)
            {
                mainPageViewModel.SetActivePageIfNeed();
            }
        }

        private void ShellContent_Appearing_1(object sender, EventArgs e)
        {
          //  ServicePageViewModel spViewModel= new ServicePageViewModel();
           //     spViewModel.SetActivePageIfNeed();
                _ethernetEntities.PagesTab = 1;
            Element view = Routing.GetOrCreateContent("servicePage");
            ServicePageViewModel spViewModel = view.BindingContext as ServicePageViewModel;
            if (spViewModel != null)
            {
                spViewModel.SetActivePageIfNeed();
            }
        }
    }
}