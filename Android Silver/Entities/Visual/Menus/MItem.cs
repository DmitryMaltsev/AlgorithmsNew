using Android_Silver.Entities.FBEntities.SetPoints;
using Android_Silver.Services;
using Android_Silver.ViewModels;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Android_Silver.Entities.Visual.Menus
{
    public class MItem : BindableBase
    {
        public ICommand ToSettingsCommand { get; private set; }
        public ICommand SetSettingsCommand { get; private set; }

        private ServiceActivePagesEntities _sActivePagesentities { get; set; }
        private SetPoints _setPoints { get; set; }
        SActivePageState _activePageState;

        #region Rising properties 
        private ObservableCollection<StrSet> _strSetsCollection;
        public ObservableCollection<StrSet> StrSetsCollection
        {
            get { return _strSetsCollection; }
            set
            {
                _strSetsCollection = value;
                OnPropertyChanged(nameof(StrSetsCollection));
            }
        }


        private bool _menuIsVisible;
        public bool MenuIsVisible
        {
            get { return _menuIsVisible; }
            set
            {
                _menuIsVisible = value;
                OnPropertyChanged(nameof(MenuIsVisible));
            }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private PicByStates _imgSource;

        public PicByStates ImgSource
        {
            get { return _imgSource; }
            set
            {
                _imgSource = value;
                OnPropertyChanged(nameof(ImgSource));
            }
        }
        #endregion


        private int _address = 0;
        private TcpClientService _tcpClientService;

        public MItem(string name, bool isVisible, PicByStates imgSource, SActivePageState sactivePageState, ushort id, int startAddress)
        {
            StrSetsCollection = new ObservableCollection<StrSet>();
            _setPoints = DIContainer.Resolve<SetPoints>();
            Name = name;
            MenuIsVisible = isVisible;
            ImgSource = imgSource;
            _activePageState = sactivePageState;
            ID = id;
            _address = startAddress;
            _sActivePagesentities = DIContainer.Resolve<ServiceActivePagesEntities>();
            _tcpClientService = DIContainer.Resolve<TcpClientService>();
            ToSettingsCommand = new Command(ExecuteToSettingsWindow);
            SetSettingsCommand = new Command(ExecuteSetSettings);
        }

        public ushort ID { get; private set; }

        private void ExecuteToSettingsWindow(object obj)
        {

            _sActivePagesentities.SetActivePageState(_activePageState);
            switch (_activePageState)
            {
                case SActivePageState.EntryPage:
                    break;
                case SActivePageState.StartPage:
                    break;
                case SActivePageState.MainMenuPage:
                    break;
                case SActivePageState.BaseSettingsPage:
                    break;
                case SActivePageState.DamperSettingsPage:
                    StrSetsCollection[0].CVal = _setPoints.CDamperSetPoints.DamperOpenTime;
                    //  StrSetsCollection[1].CVal = _setPoints.CDamperSetPoints.DamperHeatingTime;
                    break;
                case SActivePageState.FanSettingsPage:
                    StrSetsCollection[0].CVal = _setPoints.CFansSetPoints.SFanNominalFlow;
                    StrSetsCollection[1].CVal = _setPoints.CFansSetPoints.EFanNominalFlow;
                    StrSetsCollection[2].CVal = _setPoints.CFansSetPoints.Speed0v;
                    StrSetsCollection[3].CVal = _setPoints.CFansSetPoints.Speed10v;
                    StrSetsCollection[4].CVal = _setPoints.CFansSetPoints.PressureFailureDelay;
                    StrSetsCollection[5].CVal = _setPoints.CFansSetPoints.FanFailureDelay;
                    StrSetsCollection[6].CPickVal = _setPoints.CFansSetPoints.DecrFanConfig;
                    StrSetsCollection[7].CVal = _setPoints.CFansSetPoints.PDecrFan;
                    StrSetsCollection[8].CVal = _setPoints.CFansSetPoints.IDecrFan;
                    StrSetsCollection[9].CVal = _setPoints.CFansSetPoints.DDecrFan;
                    StrSetsCollection[10].CVal = _setPoints.CFansSetPoints.MinFanPercent;
                    break;
                case SActivePageState.WHSettingsPage:
                    StrSetsCollection[0].CVal = _setPoints.WHSetPoints.PWork;
                    StrSetsCollection[1].CVal = _setPoints.WHSetPoints.IWork;
                    StrSetsCollection[2].CVal = _setPoints.WHSetPoints.DWork;
                    StrSetsCollection[3].CVal = _setPoints.WHSetPoints.PRet;
                    StrSetsCollection[4].CVal = _setPoints.WHSetPoints.IRet;
                    StrSetsCollection[5].CVal = _setPoints.WHSetPoints.DRet;
                    StrSetsCollection[6].CPickVal = _setPoints.WHSetPoints.TRetMax;
                    StrSetsCollection[7].CVal = _setPoints.WHSetPoints.TRetMin;
                    StrSetsCollection[8].CVal = _setPoints.WHSetPoints.TRetStb;
                    StrSetsCollection[9].CVal = _setPoints.WHSetPoints.TRetF;
                    StrSetsCollection[10].CVal = _setPoints.WHSetPoints.TRetStart;
                    StrSetsCollection[11].CVal = _setPoints.WHSetPoints.SSMaxIntervalS;
                    StrSetsCollection[12].CVal = _setPoints.WHSetPoints.MinDamperPerc;
                    StrSetsCollection[13].CVal = _setPoints.WHSetPoints.SPWinterProcess;
                    StrSetsCollection[14].CPickVal = _setPoints.WHSetPoints.IsSummerTestPump;

                    break;
                case SActivePageState.EHSettingsPage:
                    break;
                case SActivePageState.FreonSettingsPage:
                    break;
                case SActivePageState.RecupSettingsPage:
                    break;
                case SActivePageState.HumSettingsPage:
                    break;
                case SActivePageState.CommonSettingsPage:
                    {
                        StrSetsCollection[0].CVal = _setPoints.CCommonSetPoints.SPTempAlarm;
                        StrSetsCollection[1].CPickVal = _setPoints.CEConfig.TregularCh_R;
                        StrSetsCollection[2].CVal = _setPoints.CCommonSetPoints.SPTempMaxCh;
                        StrSetsCollection[3].CVal = _setPoints.CCommonSetPoints.SPTempMinCh;
                        StrSetsCollection[4].CVal = _setPoints.CCommonSetPoints.TControlDelayS;
                        StrSetsCollection[5].CPickVal = _setPoints.CCommonSetPoints.SeasonMode;
                        StrSetsCollection[6].CVal = _setPoints.CCommonSetPoints.SPSeason;
                        StrSetsCollection[7].CVal = _setPoints.CCommonSetPoints.HystSeason;
                        StrSetsCollection[8].CPickVal = _setPoints.CEConfig.AutoResetFire;
                        StrSetsCollection[9].CPickVal = _setPoints.CEConfig.AutoRestart;
                    }
                    break;
                case SActivePageState.SensorsSettingPage:
                    break;
                case SActivePageState.ConfigPage:
                    break;
                default:
                    break;
            }
        }

        private void ExecuteSetSettings(object obj)
        {
            int[] values = new int[StrSetsCollection.Count];
            for (int i = 0; i < StrSetsCollection.Count; i++)
            {
                if (StrSetsCollection[i].EntryIsVisible)
                {
                    values[i] = StrSetsCollection[i].CVal;
                }
                else
                  if (StrSetsCollection[i].PickerIsVisible)
                {
                    values[i] = StrSetsCollection[i].CPickVal;
                }
            }
            _tcpClientService.SetCommandToServer(_address, values);
            _sActivePagesentities.SetActivePageState(SActivePageState.BaseSettingsPage);
        }
    }
}
