using Android_Silver.Entities;

using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Android_Silver.ViewModels
{
    public class LoadingPageViewModel : BindableBase
    {
        IEthernetEntities _ethernetEntities;
        private int _counter;
        public LoadingPageViewModel()
        {
            _ethernetEntities=DIContainer.Resolve<IEthernetEntities>();
            _counter = 0;
            GoHome();
        }
        async public void GoHome()
        {
            while (!_ethernetEntities.Loaded && _counter<5)
            {
                _counter += 1;
                await Task.Delay(200);
            }
            _ethernetEntities.Loaded = false;
            await Shell.Current.Navigation.PopToRootAsync(false);
            await Shell.Current.GoToAsync("mainPage", false);
           
        }
    }



}
