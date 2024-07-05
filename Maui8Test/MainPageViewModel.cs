using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Maui8Test
{
    public class MainPageViewModel
    {
        ICommand sPageCommand;
        public MainPageViewModel()
        {
            sPageCommand = new Command(ExecuteSPage);
        }

        async void ExecuteSPage(object obj)
        {
            await AppShell.Current.GoToAsync("sPage");
        }
    }
}
