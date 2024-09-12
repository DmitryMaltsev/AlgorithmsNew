using Android_Silver.Entities;
using Android_Silver.Entities.FBEntities;
using Android_Silver.Entities.Visual;
using Android_Silver.Entities.Visual.Menus;
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
            set
            {
                _entryPass = value;
                OnPropertyChanged(nameof(EntryPass));
            }
        }

        private string _entryMessage = "Введите пароль для входа";

        public string EntryMessage
        {
            get { return _entryMessage; }
            set
            {
                _entryMessage = value;
                OnPropertyChanged(nameof(EntryMessage));
            }
        }
        #endregion

        #region Commands
        public ICommand EntryPassedCommand { get; private set; }
        public ICommand StartPageConnectCommand { get; private set; }
        public ICommand DamperSettingsCommand { get; private set; }
        public ICommand FanSettingsCommand { get; private set; }
        public ICommand WHSettingsCommand { get; private set; }
        public ICommand EHSettingsCommand { get; private set; }
        public ICommand FreonCoolerSettingsCommand { get; private set; }
        public ICommand RecupSettingsCommand { get; private set; }
        public ICommand HumSettingsCommand { get; private set; }
        public ICommand SensorsSettingsCommand { get; private set; }
        public ICommand HomeCommand { get; private set; }

        public ICommand FanReturnCommand { get; private set; }
        public ICommand FansAcceptCommand { get; private set; }

        public ICommand CommonSettingsCommand { get; private set; }
        #endregion

        private List<string> _sensorPicker=new();

        public List<string> SensorPicker
        {
            get { return _sensorPicker; }
            set { 
                _sensorPicker = value;
                OnPropertyChanged(nameof(SensorPicker));    
            }
        }

        private string _cString;

        public string CString
        {
            get { return _cString; }
            set { 
                _cString = value;
                OnPropertyChanged(nameof(CString));
            }
        }


        public TcpClientService CTcpClientService { get; set; }
        public PicturesSet CPictureSet { get; set; }
        public ServiceActivePagesEntities CActivePagesEntities { get; set; }
        public EthernetEntities EthernetEntities { get; set; }

        public MenusEntities CMenusEntities { get; set; }

        public FBs CFBs { get; set; }

        public ServicePageViewModel()
        {
           // SensorPicker=new List<string>();
            SensorPicker.Add("Нет");
            SensorPicker.Add("NTC10K");
            SensorPicker.Add("PT1000");
            CString = SensorPicker[0];
            CPictureSet = DIContainer.Resolve<PicturesSet>();
            CActivePagesEntities = DIContainer.Resolve<ServiceActivePagesEntities>();
            EthernetEntities = DIContainer.Resolve<EthernetEntities>();
            CTcpClientService = DIContainer.Resolve<TcpClientService>();
            CFBs = DIContainer.Resolve<FBs>();
            CMenusEntities=DIContainer.Resolve<MenusEntities>();
            EntryPassedCommand = new Command(ExecuteEntryPass);
            StartPageConnectCommand = new Command(ExecuteConnect);
            DamperSettingsCommand = new Command(ExecuteDamperSettings);
            FanSettingsCommand = new Command(ExecuteFanSettings);
            WHSettingsCommand = new Command(ExecuteWHSettings);
            EHSettingsCommand = new Command(ExecuteEHSettings);
            FreonCoolerSettingsCommand = new Command(ExecuteFreonCoolerSettings);
            RecupSettingsCommand = new Command(ExecuteRecupSettings);
            HumSettingsCommand = new Command(ExecuteHumSettings);
            SensorsSettingsCommand = new Command(ExecuteSensorsSettings);
            FansAcceptCommand = new Command(ExecuteFansAccept);
            FanReturnCommand = new Command(ExecuteFanReturn);
            HomeCommand = new Command(ExecuteHome);
            CommonSettingsCommand = new Command(ExecuteCommonSettings);
            CActivePagesEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
            DecreseMenuItemsCommand = new Command(ExecuteDecreaseMenus);


        }

        private void ExecuteDecreaseMenus(object obj)
        {
            if (CMenusEntities.MenusCollection.Count>0)
            {
                CMenusEntities.MenusCollection.Add(
                    new Entities.Visual.Menus.MenuItem(CMenusEntities.MenusCollection.Count.ToString(), true, CPictureSet.BaseSettings1But,SActivePageState.DamperSettingsPage));
            }
           
        }

        public ICommand DecreseMenuItemsCommand { get;private set; }
        

        async private void ExecuteConnect()
        {
            EthernetEntities.SystemMessage = "Check";
            if (!CTcpClientService.IsConnecting)
            {
                await CTcpClientService.Connect();
                if (EthernetEntities.IsConnected)
                {
                    CActivePagesEntities.SetActivePageState(SActivePageState.EntryPage);
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
        #region Base settings
        private void ExecuteCommonSettings(object obj)
        {
            CActivePagesEntities.SetActivePageState(SActivePageState.CommonSettingsPage);
        }
        private void ExecuteDamperSettings(object obj)
        {
            CActivePagesEntities.SetActivePageState(SActivePageState.DamperSettingsPage);
        }
        private void ExecuteFanSettings(object obj)
        {
            CActivePagesEntities.SetActivePageState(SActivePageState.FanSettingsPage);
        }
        private void ExecuteWHSettings(object obj)
        {
            CActivePagesEntities.SetActivePageState(SActivePageState.WHSettingsPage);
        }

        private void ExecuteEHSettings(object obj)
        {
            CActivePagesEntities.SetActivePageState(SActivePageState.EHSettingsPage);
        }


        private void ExecuteFreonCoolerSettings(object obj)
        {
            CActivePagesEntities.SetActivePageState(SActivePageState.FreonSettingsPage);
        }

        private void ExecuteRecupSettings(object obj)
        {
            CActivePagesEntities.SetActivePageState(SActivePageState.RecupSettingsPage);
        }

        private void ExecuteHumSettings(object obj)
        {
            CActivePagesEntities.SetActivePageState(SActivePageState.HumSettingsPage);
        }

        private void ExecuteSensorsSettings(object obj)
        {
            CActivePagesEntities.SetActivePageState(SActivePageState.SensorsSettingPage);
        }
        #endregion

        #region Fan
        private void ExecuteHome(object obj)
        {
            CActivePagesEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
        }

        private void ExecuteFanReturn(object obj)
        {
            CActivePagesEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
        }

        private void ExecuteFansAccept(object obj)
        {
            int[] vals = { CFBs.CFans.SupNominalFlow, CFBs.CFans.ExhaustNominalFlow };
            CTcpClientService.SetCommandToServer(400, vals);
            CActivePagesEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
        }
        #endregion


    }
}
