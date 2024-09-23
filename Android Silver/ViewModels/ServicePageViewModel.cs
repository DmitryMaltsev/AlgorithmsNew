using Android_Silver.Entities;
using Android_Silver.Entities.FBEntities;
using Android_Silver.Entities.Visual;
using Android_Silver.Entities.Visual.Menus;
using Android_Silver.Services;
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
        public ICommand StartPageDisconnectCommand { get; private set; }
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
        public ICommand ToSettingsCommand { get; private set; }
        public ICommand SetSettingsCommand { get; private set; }
        #endregion

        private string _cString;

        public string CString
        {
            get { return _cString; }
            set
            {
                _cString = value;
                OnPropertyChanged(nameof(CString));
            }
        }

        private int _menuNum;
        public int MenuNum
        {
            get { return _menuNum; }
            set
            {
                _menuNum = value;
                OnPropertyChanged(nameof(MenuNum));
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

            CPictureSet = DIContainer.Resolve<PicturesSet>();
            CActivePagesEntities = DIContainer.Resolve<ServiceActivePagesEntities>();
            EthernetEntities = DIContainer.Resolve<EthernetEntities>();
            CTcpClientService = DIContainer.Resolve<TcpClientService>();
            CFBs = DIContainer.Resolve<FBs>();
            CMenusEntities = DIContainer.Resolve<MenusEntities>();
            EntryPassedCommand = new Command(ExecuteEntryPass);
            StartPageConnectCommand = new Command(ExecuteConnect);
            StartPageDisconnectCommand = new Command(ExecuteDisconnect);
            EHSettingsCommand = new Command(ExecuteEHSettings);
            FreonCoolerSettingsCommand = new Command(ExecuteFreonCoolerSettings);
            RecupSettingsCommand = new Command(ExecuteRecupSettings);
            HumSettingsCommand = new Command(ExecuteHumSettings);
            SensorsSettingsCommand = new Command(ExecuteSensorsSettings);
            FanReturnCommand = new Command(ExecuteFanReturn);
            HomeCommand = new Command(ExecuteHome);
            CActivePagesEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
            DecreseMenuItemsCommand = new Command(ExecuteDecreaseMenus);
            IncreaseMenuItemsCommand = new Command(ExecuteIncrease);
            ToSettingsCommand = new Command(ExecuteToSettingsWindow);
            SetSettingsCommand = new Command(ExecuteSetSettings);
            StartTimer();

        }

        public ICommand IncreaseMenuItemsCommand { get; private set; }
        private void ExecuteIncrease(object obj)
        {
            if (CMenusEntities.MenusCollection.Count > 0)
            {
                var menu = CMenusEntities.MenusCollection.FirstOrDefault(menu => menu.ID == MenuNum);
                if (menu != null)
                {
                    menu.MenuIsVisible = true;
                }
            }
        }

        private void ExecuteDecreaseMenus(object obj)
        {
            if (CMenusEntities.MenusCollection.Count > 0)
            {
                var menu = CMenusEntities.MenusCollection.FirstOrDefault(menu => menu.ID == MenuNum);
                if (menu != null)
                {
                    menu.MenuIsVisible = false;
                }
            }
        }

        public ICommand DecreseMenuItemsCommand { get; private set; }


        async private void ExecuteConnect()
        {
            EthernetEntities.SystemMessage = "Check";
            if (!CTcpClientService.IsConnecting)
            {
                await CTcpClientService.Connect();
                if (EthernetEntities.IsConnected)
                {
                   // CActivePagesEntities.SetActivePageState(SActivePageState.LoadingPage);
                    CPictureSet.SetPicureSetIfNeed(CPictureSet.LinkHeader, CPictureSet.LinkHeader.Selected);
                    CTcpClientService.MessageToServer = "";
                    //CTcpClientService.SendRecieveTask("299,56");
                    CTcpClientService.SendRecieveTask("");
                   
                    // TcpClientService.SendRecieveTask("137,4");
                }
            }
            else
            {

                CPictureSet.SetPicureSetIfNeed(CPictureSet.LinkHeader, CPictureSet.LinkHeader.Default);
                EthernetEntities.SystemMessage = "В данный момент подключаемся";
            }
        }

        private void ExecuteDisconnect(object obj)
        {
            CTcpClientService.Disconnect();
            //   CPictureSet.SetPicureSetIfNeed(CPictureSet.LinkHeader, CPictureSet.LinkHeader.Default);
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


        private void ExecuteToSettingsWindow(object obj)
        {
            ushort id = (ushort)obj;
            SActivePageState page = CMenusEntities.StartMenuCollection[id - 1].CactivePageState;
            CActivePagesEntities.SetActivePageState(page);

        }

        private void ExecuteSetSettings(object obj)
        {
            switch (CActivePagesEntities.LastActivePageState)
            {
                case SActivePageState.ConfigPage:
                    SendMItemSettings(CMenusEntities.StartMenuCollection[0]);
                    break;
                case SActivePageState.CommonSettingsPage:
                    SendMItemSettings(CMenusEntities.StartMenuCollection[1]);
                    break;
                case SActivePageState.DamperSettingsPage:
                    SendMItemSettings(CMenusEntities.StartMenuCollection[2]);
                    break;
                case SActivePageState.FanSettingsPage:
                    SendMItemSettings(CMenusEntities.StartMenuCollection[3]);
                    break;
                case SActivePageState.WHSettingsPage:
                    SendMItemSettings(CMenusEntities.StartMenuCollection[4]);
                    break;
                case SActivePageState.EHSettingsPage:
                    SendMItemSettings(CMenusEntities.StartMenuCollection[5]);
                    break;
                case SActivePageState.FreonSettingsPage:
                    SendMItemSettings(CMenusEntities.StartMenuCollection[6]);
                    break;
                case SActivePageState.RecupSettingsPage:
                    SendMItemSettings(CMenusEntities.StartMenuCollection[7]);
                    break;
                case SActivePageState.HumSettingsPage:
                    SendMItemSettings(CMenusEntities.StartMenuCollection[8]);
                    break;
                case SActivePageState.SensorsSettingPage:
                    SendMItemSettings(CMenusEntities.StartMenuCollection[9]);
                    break;
            }
            CActivePagesEntities.SetActivePageState(SActivePageState.LoadingPage);
        }

        public void SendMItemSettings(MItem mItem)
        {
            if (mItem.StrSetsCollection.Count > 0)
            {
                int[] values = new int[mItem.StrSetsCollection.Count];
                for (int i = 0; i < mItem.StrSetsCollection.Count; i++)
                {
                    if (mItem.StrSetsCollection[i].EntryIsVisible)
                    {
                        values[i] = mItem.StrSetsCollection[i].CVal;
                    }
                    else
                      if (mItem.StrSetsCollection[i].PickerIsVisible)
                    {
                        values[i] = mItem.StrSetsCollection[i].CPickVal;
                    }
                }
                CTcpClientService.SetCommandToServer(mItem.Address, values);
            }
        }






        #endregion



        private void ExecuteHome(object obj)
        {
            CActivePagesEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
        }

        private void ExecuteFanReturn(object obj)
        {
            CActivePagesEntities.SetActivePageState(SActivePageState.BaseSettingsPage);
        }

        Timer timer;

        private void StartTimer()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                timer = new Timer(obj =>
                {
                    MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        EthernetEntities.SystemMessage = $"Счетчик принятий ={CTcpClientService.ResieveCounter}";
                    });
                },
                null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
            });
        }

    }
}
