using Android_Silver.Entities;

using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Android_Silver.ViewModels
{
    public class LoadingPageViewModel : BindableBase
    {
        IDispatcherTimer timer;
        IEthernetEntities _ethernetEntities;
        ICommand pageCommand;
        private int _counter;
        public LoadingPageViewModel()
        {
            _ethernetEntities=DIContainer.Resolve<IEthernetEntities>();
            _counter = 0;
            timer = Application.Current.Dispatcher.CreateTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            pageCommand = new Command(ExecutePageCommand);
            pageCommand.Execute(this);
         //   timer.Tick += (s, e) => GoHome();
         //   timer.Start();
        }

        async void ExecutePageCommand(object obj)
        {
            await Task.Delay(1);
            await Shell.Current.GoToAsync("mainPage", false);
         
        }

         private void GoHome()
        {
            if (_ethernetEntities.Loaded == true || _counter > 10)
            {
                _ethernetEntities.Loaded = false;
               
                if (timer != null)
                {
                    timer.Tick -= (s, e) => GoHome();
                    timer.Stop();
                }
                pageCommand.Execute(this);
            }
            _counter += 1;



        }
    }



}
