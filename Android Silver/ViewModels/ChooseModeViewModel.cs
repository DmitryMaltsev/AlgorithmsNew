using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Android_Silver.ViewModels
{
    public class ChooseModeViewModel:BindableBase
    {
        public ICommand MinModeCommand { get;private set; }
        public ICommand NormalModeCommand { get; private set; }
        public ICommand MaxModeCommand { get; private set; }
        public ICommand KitchenModeCommand { get; private set; }
        public ICommand ShedulerModeCommand { get; private set; }

        public ICommand VacationModeCommand { get; private set; }
        public ICommand TurnOffModeCommand { get; private set; }

        public ChooseModeViewModel()
        {
            MinModeCommand = new Command(ExecuteMinMode);
            NormalModeCommand = new Command(ExecuteNormalCommand);
        }

        private void ExecuteNormalCommand(object obj)
        {
            throw new NotImplementedException();
        }

        private void ExecuteMinMode(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
