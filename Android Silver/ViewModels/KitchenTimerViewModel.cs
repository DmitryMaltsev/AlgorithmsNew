using Android_Silver.Entities;
using Android_Silver.Entities.Visual;
using Android_Silver.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Android_Silver.ViewModels
{
    public class KitchenTimerViewModel : BindableBase
    {
        public PicturesSet CPictureSet { get; set; }

        private int _minutes;
        public int Minutes
        {
            get { return _minutes; }
            set
            {
                _minutes = value;
                OnPropertyChanged(nameof(Minutes));
            }
        }

        public ICommand UpMInutesCommand { get; private set; }
        public ICommand DnMinutesCommand { get; private set; }
        public ICommand OkCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        public KitchenTimerViewModel()
        {
            CPictureSet = DIContainer.Resolve<PicturesSet>();
            UpMInutesCommand = new Command(ExecuteUpMinutes);
            DnMinutesCommand = new Command(ExecuteDnMinutes);
            OkCommand = new Command(ExecuteOk);
            CancelCommand = new Command(ExecuteCancel);
        }



        private void ExecuteDnMinutes(object obj)
        {
            if (Minutes >= 10)
            {
                Minutes -= 10;
            }
            else
            {
                Minutes = 0;
            }
        }

        private void ExecuteUpMinutes(object obj)
        {
            if (Minutes < 600)
            {
                Minutes += 10;
            }
            else
            {
                Minutes = 600;
            }
        }

        async void ExecuteCancel(object obj)
        {
            await Shell.Current.GoToAsync("mainPage");
        }

        async void ExecuteOk(object obj)
        {
            await Shell.Current.GoToAsync("mainPage");
        }
    }
}
