using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Maui8Test
{
    public class SeconPageViewModel
    {
        ICommand GoToMainCommand;
        public SeconPageViewModel()
        {
            GoToMainCommand = new Command(ExecuteGoToMain);
        }

        async void ExecuteGoToMain(object obj)
        {
            await AppShell.Current.GoToAsync("mainPage");
        }
    }
}
