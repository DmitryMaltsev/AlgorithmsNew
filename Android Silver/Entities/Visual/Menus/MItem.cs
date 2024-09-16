using Android_Silver.Entities.FBEntities;
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

        private FBs _fbs;
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
            _fbs = DIContainer.Resolve<FBs>();
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
                    StrSetsCollection[0].CVal = _fbs.CDamperSetPoints.DamperOpenTime;
                    //  StrSetsCollection[1].CVal = _fbs.CDamperSetPoints.DamperHeatingTime;
                    break;
                case SActivePageState.FanSettingsPage:
                    StrSetsCollection[0].CVal = _fbs.CFans.SFanNominalFlow;
                    StrSetsCollection[1].CVal = _fbs.CFans.EFanNominalFlow;
                    StrSetsCollection[2].CVal = _fbs.CFans.Speed0v;
                    StrSetsCollection[3].CVal = _fbs.CFans.Speed10v;
                    StrSetsCollection[4].CVal = _fbs.CFans.PressureFailureDelay;
                    StrSetsCollection[5].CVal = _fbs.CFans.FanFailureDelay;
                    StrSetsCollection[6].CPickVal = _fbs.CFans.DecrFanConfig;
                    StrSetsCollection[7].CVal = _fbs.CFans.PDecrFan;
                    StrSetsCollection[8].CVal = _fbs.CFans.IDecrFan;
                    StrSetsCollection[9].CVal = _fbs.CFans.DDecrFan;
                    StrSetsCollection[10].CVal = _fbs.CFans.MinFanPercent;
                    break;
                case SActivePageState.WHSettingsPage:
                    StrSetsCollection[0].CVal = _fbs.CWHSetPoints.PWork;
                    StrSetsCollection[1].CVal = _fbs.CWHSetPoints.IWork;
                    StrSetsCollection[2].CVal = _fbs.CWHSetPoints.DWork;
                    StrSetsCollection[3].CVal = _fbs.CWHSetPoints.PRet;
                    StrSetsCollection[4].CVal = _fbs.CWHSetPoints.IRet;
                    StrSetsCollection[5].CVal = _fbs.CWHSetPoints.DRet;
                    StrSetsCollection[6].CVal = _fbs.CWHSetPoints.TRetMax;
                    StrSetsCollection[7].CVal = _fbs.CWHSetPoints.TRetMin;
                    StrSetsCollection[8].CVal = _fbs.CWHSetPoints.TRetStb;
                    StrSetsCollection[9].CVal = _fbs.CWHSetPoints.TRetF;
                    StrSetsCollection[10].CVal = _fbs.CWHSetPoints.TRetStart;
                    StrSetsCollection[11].CVal = _fbs.CWHSetPoints.SSMaxIntervalS;
                    StrSetsCollection[12].CVal = _fbs.CWHSetPoints.MinDamperPerc;
                    StrSetsCollection[13].CVal = _fbs.CWHSetPoints.SPWinterProcess;
                    StrSetsCollection[14].CPickVal = _fbs.CWHSetPoints.IsSummerTestPump;
                    break;
                case SActivePageState.EHSettingsPage:
                    StrSetsCollection[0].CVal = _fbs.CEHSetPoints.NomPowerVT;
                    StrSetsCollection[1].CVal = _fbs.CEHSetPoints.PReg;
                    StrSetsCollection[2].CVal = _fbs.CEHSetPoints.IReg;
                    StrSetsCollection[3].CVal = _fbs.CEHSetPoints.DReg;
                    StrSetsCollection[4].CVal = _fbs.CEHSetPoints.BlowDownTime;
                    break;
                case SActivePageState.FreonSettingsPage:
                    StrSetsCollection[0].CVal = _fbs.CFreonCoolerSP.PReg;
                    StrSetsCollection[1].CVal = _fbs.CFreonCoolerSP.IReg;
                    StrSetsCollection[2].CVal = _fbs.CFreonCoolerSP.DReg;
                    StrSetsCollection[3].CVal = _fbs.CFreonCoolerSP.Stage1OffS;
                    StrSetsCollection[4].CVal = _fbs.CFreonCoolerSP.Stage1OnS;
                    StrSetsCollection[5].CVal = _fbs.CFreonCoolerSP.Hyst;
                    break;
                case SActivePageState.RecupSettingsPage:
                    StrSetsCollection[0].CVal = _fbs.CRecup.PReg;
                    StrSetsCollection[1].CVal = _fbs.CRecup.IReg;
                    StrSetsCollection[2].CVal = _fbs.CRecup.DReg;
                    StrSetsCollection[3].CVal = _fbs.CRecup.EffSP;
                    StrSetsCollection[4].CVal = _fbs.CRecup.EffFailValue;
                    StrSetsCollection[5].CVal = _fbs.CRecup.EffFailDelay;
                    StrSetsCollection[6].CVal = _fbs.CRecup.HZMax;
                    StrSetsCollection[7].CVal = _fbs.CRecup.TempA;
                    StrSetsCollection[8].CVal = _fbs.CRecup.TempB;
                    StrSetsCollection[9].CVal = _fbs.CRecup.TempC;
                    StrSetsCollection[10].CVal = _fbs.CRecup.TempD;
                    break;
                case SActivePageState.HumSettingsPage:
                    StrSetsCollection[0].CVal = _fbs.CHumiditySPS.PReg;
                    StrSetsCollection[1].CVal = _fbs.CHumiditySPS.IReg;
                    StrSetsCollection[2].CVal = _fbs.CHumiditySPS.DReg;
                    StrSetsCollection[3].CVal = _fbs.CHumiditySPS.Stage1OffS;
                    StrSetsCollection[4].CVal = _fbs.CHumiditySPS.Stage1OnS;
                    StrSetsCollection[5].CVal = _fbs.CHumiditySPS.Hyst;
                    break;
                case SActivePageState.CommonSettingsPage:
                    {
                        StrSetsCollection[0].CVal = _fbs.CCommonSetPoints.SPTempAlarm;
                        StrSetsCollection[1].CPickVal = _fbs.CEConfig.TregularCh_R;
                        StrSetsCollection[2].CVal = _fbs.CCommonSetPoints.SPTempMaxCh;
                        StrSetsCollection[3].CVal = _fbs.CCommonSetPoints.SPTempMinCh;
                        StrSetsCollection[4].CVal = _fbs.CCommonSetPoints.TControlDelayS;
                        StrSetsCollection[5].CPickVal = _fbs.CCommonSetPoints.SeasonMode;
                        StrSetsCollection[6].CVal = _fbs.CCommonSetPoints.SPSeason;
                        StrSetsCollection[7].CVal = _fbs.CCommonSetPoints.HystSeason;
                        StrSetsCollection[8].CPickVal = _fbs.CEConfig.AutoResetFire;
                        StrSetsCollection[9].CPickVal = _fbs.CEConfig.AutoRestart;
                    }
                    break;
                case SActivePageState.SensorsSettingPage:
                    StrSetsCollection[0].CPickVal = _fbs.CEConfig.TOutDoorConfig;
                    StrSetsCollection[1].CPickVal = _fbs.CEConfig.TSupplyFConfig;
                    StrSetsCollection[2].CPickVal = _fbs.CEConfig.TExhaustFConfig;
                    StrSetsCollection[3].CPickVal = _fbs.CEConfig.TRoomConfig;
                    StrSetsCollection[4].CPickVal = _fbs.CEConfig.TReturnWaterConfig;
                    StrSetsCollection[5].CPickVal = _fbs.CEConfig.AirQualityConfig;
                    StrSetsCollection[6].CPickVal = _fbs.CEConfig.HumSensorConfig;
                    StrSetsCollection[7].CVal = _fbs.CSensors.OutdoorTemp.Correction;
                    StrSetsCollection[8].CVal = _fbs.CSensors.SupTemp.Correction;
                    StrSetsCollection[9].CVal = _fbs.CSensors.ExhaustTemp.Correction;
                    StrSetsCollection[10].CVal = _fbs.CSensors.RoomTemp.Correction;
                    StrSetsCollection[11].CVal = _fbs.CSensors.ReturnTemp.Correction;

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
