using Android_Silver.Entities;
using Android_Silver.Entities.Modes;
using Android_Silver.Entities.Visual;
using Android_Silver.Entities.Visual.Menus;
using Android_Silver.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Android_Silver.ViewModels
{
    public class KitchenTimerViewModel : BindableBase
    {
        public PicturesSet CPictureSet { get; set; }
        MenusEntities _menuesEntities { get; set; }


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

        #region Commands
        public ICommand UpMInutesCommand { get; private set; }
        public ICommand DnMinutesCommand { get; private set; }
        public ICommand OkCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        public ICommand HomeCommand { get; private set; }
        #endregion

        TcpClientService _tcpClientService;
        ModesEntities _modesEntities;

        public KitchenTimerViewModel()
        {
            CPictureSet = DIContainer.Resolve<PicturesSet>();

            UpMInutesCommand = new Command(ExecuteUpMinutes);
            DnMinutesCommand = new Command(ExecuteDnMinutes);
            HomeCommand = new Command(ExecuteHomeCommand);
            OkCommand = new Command(ExecuteOk);
            CancelCommand = new Command(ExecuteCancel);
            _tcpClientService = DIContainer.Resolve<TcpClientService>();
            _modesEntities=DIContainer.Resolve<ModesEntities>();
            Minutes = _modesEntities.Mode2ValuesList[1].TimeModeValues[0].Minute;
            _menuesEntities=DIContainer.Resolve<MenusEntities>();
        }

        async void ExecuteHomeCommand(object obj)
        {
            await Shell.Current.GoToAsync("mainPage");
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
            int[] values = { Minutes };
            _tcpClientService.SetCommandToServer(134+ _menuesEntities.WriteOffset, values);
            await Shell.Current.GoToAsync("mainPage");
        }
    }
}
