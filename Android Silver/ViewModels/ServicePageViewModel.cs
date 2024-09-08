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
    class ServicePageViewModel : BindableBase
    {
        #region Rising properties
        private int _entryPass = 1111;
        public int EntryPass
        {
            get { return _entryPass; }
            set {
                _entryPass = value;
                OnPropertyChanged(nameof(EntryPass));
            }
        }

        private string _entryMessage = "Введите пароль для входа";

        public string EntryMessage
        {
            get { return _entryMessage; }
            set {
                _entryMessage = value;
                OnPropertyChanged(nameof(EntryMessage));
            }
        }
        #endregion

        #region Commands
        public ICommand EntryPassedCommand { get; private set; }
        public ICommand StartPageConnectCommand {  get; private set; }

        public ICommand FanSettingsCommand { get; private set; }
        #endregion

        public TcpClientService CTcpClientService { get; set; }
        public PicturesSet CPictureSet { get; set; }
        public ServiceActivePagesEntities CActivePagesEntities { get;set;}
        public EthernetEntities EthernetEntities { get; set; }

        public ServicePageViewModel()
        {
            CPictureSet = DIContainer.Resolve<PicturesSet>();
            CActivePagesEntities=DIContainer.Resolve<ServiceActivePagesEntities>();
            EthernetEntities=DIContainer.Resolve<EthernetEntities>();
            CTcpClientService=DIContainer.Resolve<TcpClientService>();
            EntryPassedCommand = new Command(ExecuteEntryPass);
            StartPageConnectCommand = new Command(ExecuteConnect);
            FanSettingsCommand = new Command(ExecuteFanSettings);
            CActivePagesEntities.SetActivePageState(SActivePageState.BaseSettingsPage);

        }

        private void ExecuteFanSettings(object obj)
        {
            CActivePagesEntities.SetActivePageState(SActivePageState.FanSettingsPage);
        }

        async private void ExecuteConnect()
        {
            EthernetEntities.SystemMessage = "Check";
            if (!CTcpClientService.IsConnecting)
            {
                await CTcpClientService.Connect();
                if (EthernetEntities.IsConnected)
                {
                    CActivePagesEntities.SetActivePageState(SActivePageState.EntyPage);
                    CPictureSet.SetPicureSetIfNeed(CPictureSet.LinkHeader, CPictureSet.LinkHeader.Selected);
                    CTcpClientService.SendRecieveTask("103,50");

                    // TcpClientService.SendRecieveTask("137,4");
                }
            }
            else
            {

                CPictureSet.SetPicureSetIfNeed(CPictureSet.LinkHeader, CPictureSet.LinkHeader.Default);
                EthernetEntities.SystemMessage = "В данный момент подключаемся";
            }
        }



        #region Execute entry
        private void ExecuteEntryPass(object obj)
        {
            if (EntryPass == 4444)
            {
                EntryMessage = "Пароль введен верно";
            }
            else
            {
                EntryMessage = "Пароль введен неверно";
            }
        }

        public void SetActivePageIfNeed()
        {
            //if (!EthernetEntities.IsConnected)
            //{
            //    CActivePagesEntities.SetActivePageState(SActivePageState.StartPage);
            //}
            //else
            //{
            //    CActivePagesEntities.SetActivePageState(CActivePagesEntities.LastActivePageState);
            //}
           
            CActivePagesEntities.SetActivePageState(CActivePagesEntities.LastActivePageState);
        }

        #endregion
    }
}
