using Android_Silver.ViewModels;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.Net.Mime.MediaTypeNames;

namespace Android_Silver.Entities.Visual
{
    public class PicturesSet : BindableBase
    {

        private string _background = String.Empty;
        public string Background
        {
            get { return _background; }
            set
            {
                _background = value;
                OnPropertyChanged(nameof(Background));
            }
        }

        private string _loading;

        public string Loading
        {
            get { return _loading; }
            set
            {
                _loading = value;
                OnPropertyChanged(nameof(Loading));
            }
        }

        private string _loadingPic;
        public string LoadingPic
        {
            get { return _loadingPic; }
            set
            {
                _loadingPic = value;
                OnPropertyChanged(nameof(LoadingPic));
            }
        }

        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged($"{nameof(Title)}");
            }
        }
        private PicByStates _selectStroke;
        public PicByStates SelectStroke
        {
            get { return _selectStroke; }
            set { _selectStroke = value; }
        }
        #region Начальное окно
        private PicByStates _ipBut;
        public PicByStates IPBut
        {
            get { return _ipBut; }
            set {
                _ipBut = value;
                OnPropertyChanged(nameof(IPBut));
            }
        }




        #endregion
        #region Главное окно
        private PicByStates _substrate;
        public PicByStates Substrate
        {
            get { return _substrate; }
            set
            {
                _substrate = value;
                OnPropertyChanged(nameof(Substrate));
            }
        }

        private PicByStates _eHeaterHeader;

        public PicByStates EHeaterHeader
        {
            get { return _eHeaterHeader; }
            set
            {
                _eHeaterHeader = value;
                OnPropertyChanged(nameof(EHeaterHeader));
            }
        }

        private PicByStates _fanHeader;
        public PicByStates FanHeader
        {
            get { return _fanHeader; }
            set
            {
                _fanHeader = value;
                OnPropertyChanged(nameof(FanHeader));
            }
        }

        private PicByStates _filter70Header;

        public PicByStates Filter70Header
        {
            get { return _filter70Header; }
            set
            {
                _filter70Header = value;
                OnPropertyChanged(nameof(Filter70Header));
            }
        }

        private PicByStates _linkHeader;

        public PicByStates LinkHeader
        {
            get { return _linkHeader; }
            set
            {
                _linkHeader = value;
                OnPropertyChanged(nameof(LinkHeader));
            }
        }

        private PicByStates _recuperatorHeader;

        public PicByStates RecuperatorHeader
        {
            get { return _recuperatorHeader; }
            set { _recuperatorHeader = value; }
        }

        private PicByStates _filter100MainIcon;

        public PicByStates Filter100MainIcon
        {
            get { return _filter100MainIcon; }
            set
            {
                _filter100MainIcon = value;
                OnPropertyChanged(nameof(Filter100MainIcon));
            }
        }

        private PicByStates _alarmMainIcon;
        public PicByStates AlarmMainIcon
        {
            get { return _alarmMainIcon; }
            set
            {
                _alarmMainIcon = value;
                OnPropertyChanged(nameof(AlarmMainIcon));
            }
        }

        #endregion

        #region Общие кнопки
        private PicByStates _homeButton;

        public PicByStates HomeButton
        {
            get { return _homeButton; }
            set
            {
                _homeButton = value;
                OnPropertyChanged(nameof(HomeButton));
            }
        }

        private PicByStates _nextButton;

        public PicByStates NextButton
        {
            get { return _nextButton; }
            set
            {
                _nextButton = value;
                OnPropertyChanged(nameof(NextButton));
            }
        }

        private PicByStates _okButton;

        public PicByStates OkButton
        {
            get { return _okButton; }
            set
            {
                _okButton = value;
                OnPropertyChanged(nameof(OkButton));
            }
        }

        private PicByStates _settingsButton;

        public PicByStates SettingsButton
        {
            get { return _settingsButton; }
            set
            {
                _settingsButton = value;
                OnPropertyChanged(nameof(SettingsButton));
            }
        }

        private PicByStates _backButton;

        public PicByStates BackButton
        {
            get { return _backButton; }
            set
            {
                _backButton = value;
                OnPropertyChanged(nameof(BackButton));
            }
        }

        private PicByStates _resetButton;

        public PicByStates ResetButton
        {
            get { return _resetButton; }
            set
            {
                _resetButton = value;
                OnPropertyChanged(nameof(_resetButton));
            }
        }

        private string _journalStroke;
        public string JournalStroke
        {
            get { return _journalStroke; }
            set
            {
                _journalStroke = value;
                OnPropertyChanged(nameof(JournalStroke));
            }
        }


        #endregion

        #region Кухня+влажность
        private PicByStates _acceptButton;

        public PicByStates AcceptButton
        {
            get { return _acceptButton; }
            set
            {
                _acceptButton = value;
                OnPropertyChanged(nameof(AcceptButton));
            }
        }

        private PicByStates _cancelButton;
        public PicByStates CancelButton
        {
            get { return _cancelButton; }
            set
            {
                _cancelButton = value;
                OnPropertyChanged(nameof(AcceptButton));
            }
        }

        private List<PicByStates> _digitalButtonsUp;

        public List<PicByStates> DigitalButtonsUp
        {
            get { return _digitalButtonsUp; }
            set
            {
                _digitalButtonsUp = value;
                OnPropertyChanged(nameof(DigitalButtonsUp));
            }
        }

        private PicByStates _kitchenButtonUp;

        public PicByStates KitchenButtonUp
        {
            get { return _kitchenButtonUp; }
            set
            {
                _kitchenButtonUp = value;
                OnPropertyChanged(nameof(KitchenButtonUp));
            }
        }

        private PicByStates _kitchenButtonDn;

        public PicByStates KitchenButtonDn
        {
            get { return _kitchenButtonDn; }
            set
            {
                _kitchenButtonDn = value;
                OnPropertyChanged(nameof(KitchenButtonDn));
            }
        }

        private List<PicByStates> _digitalButtonsDn;

        public List<PicByStates> DigitalButtonsDn
        {
            get { return _digitalButtonsDn; }
            set
            {
                _digitalButtonsDn = value;
                OnPropertyChanged(nameof(DigitalButtonsDn));
            }
        }

        private string _digitalImage;

        public string DigitalImage
        {
            get { return _digitalImage; }
            set
            {
                _digitalImage = value;
                OnPropertyChanged($"{nameof(DigitalImage)}");
            }
        }

        private string _timeDigitalImage;

        public string TimeDigitalImage
        {
            get { return _timeDigitalImage; }
            set
            {
                _timeDigitalImage = value;
                OnPropertyChanged($"{nameof(TimeDigitalImage)}");
            }
        }
        #endregion

        #region Временные режимы
        private PicByStates _tModeStroke;

        public PicByStates TModeStroke
        {
            get { return _tModeStroke; }
            set
            {
                _tModeStroke = value;
            }
        }



        #endregion

        #region Настройки прочие
        private string _contactBackGround;
        public string ContactBackGround
        {
            get { return _contactBackGround; }
            set
            {
                _contactBackGround = value;
                OnPropertyChanged(nameof(ContactBackGround));
            }
        }

        private PicByStates _filterChangedBut;
        public PicByStates FilterChangedBut
        {
            get { return _filterChangedBut; }
            set
            {
                _filterChangedBut = value;
                OnPropertyChanged(nameof(FilterChangedBut));
            }
        }

        private PicByStates _timeBut;
        public PicByStates TimeBut
        {
            get { return _timeBut; }
            set
            {
                _timeBut = value;
                OnPropertyChanged(nameof(TimeBut));
            }
        }

        private PicByStates _humidityBut;
        public PicByStates HumidityBut
        {
            get { return _humidityBut; }
            set
            {
                _humidityBut = value;
                OnPropertyChanged(nameof(HumidityBut));
            }
        }

        private PicByStates _arrowButLeft;
        public PicByStates ArrowButLeft
        {
            get { return _arrowButLeft; }
            set
            {
                _arrowButLeft = value;
                OnPropertyChanged(nameof(_arrowButLeft));
            }
        }

        private PicByStates _arrowButRight;

        public PicByStates ArrowButRight
        {
            get { return _arrowButRight; }
            set
            {
                _arrowButRight = value;
                OnPropertyChanged($"{nameof(ArrowButRight)}");
            }
        }
        #endregion

        #region Разное

        private string _efficiency;
        public string Efficiency
        {
            get { return _efficiency; }
            set
            {
                _efficiency = value;
                OnPropertyChanged($"{nameof(Efficiency)}");
            }
        }

        private string _supCons;
        public string SupCons
        {
            get { return _supCons; }
            set
            {
                _supCons = value;
                OnPropertyChanged($"{nameof(SupCons)}");
            }
        }

        private string _exhaustCons;
        public string ExhaustCons
        {
            get { return _exhaustCons; }
            set
            {
                _exhaustCons = value;
                OnPropertyChanged($"{nameof(ExhaustCons)}");
            }
        }

        private string _outdoorTemp;
        public string OutDoorTemp
        {
            get { return _outdoorTemp; }
            set
            {
                _outdoorTemp = value;
                OnPropertyChanged($"{nameof(OutDoorTemp)}");
            }
        }

        private string _roomTemp;

        public string RoomTemp
        {
            get { return _roomTemp; }
            set
            {
                _roomTemp = value;
                OnPropertyChanged(nameof(RoomTemp));
            }
        }

        private string _filterPol;

        public string FilterPol
        {
            get { return _filterPol; }
            set
            {
                _filterPol = value;
                OnPropertyChanged($"{nameof(FilterPol)}");
            }
        }

        #endregion

        private ObservableCollection<PicByStates> _activeModesPics = new ObservableCollection<PicByStates>();

        public ObservableCollection<PicByStates> ActiveModesPics
        {
            get { return _activeModesPics; }
            set
            {
                _activeModesPics = value;
                OnPropertyChanged(nameof(ActiveModesPics));
            }
        }

        private ObservableCollection<PicByStates> _selectModesPics = new ObservableCollection<PicByStates>();

        public ObservableCollection<PicByStates> SelectModesPics
        {
            get { return _selectModesPics; }
            set
            {
                _selectModesPics = value;
                OnPropertyChanged(nameof(SelectModesPics));
            }
        }

        private ObservableCollection<string> _miniIconsPics;

        public ObservableCollection<string> MiniIconsPics
        {
            get { return _miniIconsPics; }
            set
            {
                _miniIconsPics = value;
                OnPropertyChanged(nameof(MiniIconsPics));
            }
        }



        //Иконки в календаре, возможно при смене уставок режима
        private ObservableCollection<PicByStates> _iconsPics;

        public ObservableCollection<PicByStates> IconsPics
        {
            get { return _iconsPics; }
            set
            {
                _iconsPics = value;
                OnPropertyChanged(nameof(IconsPics));
            }
        }

        #region Settings pics
        private PicByStates _journalBut;
        public PicByStates JournalBut
        {
            get { return _journalBut; }
            set
            {
                _journalBut = value;
                OnPropertyChanged(nameof(JournalBut));
            }
        }

        private PicByStates _modesSetBut;

        public PicByStates ModesSetBut
        {
            get { return _modesSetBut; }
            set
            {
                _modesSetBut = value;
                OnPropertyChanged(nameof(ModesSetBut));
            }
        }


        private PicByStates _settingsOtherBut;
        public PicByStates SettingsOtherBut
        {
            get { return _settingsOtherBut; }
            set
            {
                _settingsOtherBut = value;
                OnPropertyChanged(nameof(SettingsOtherBut));
            }
        }


        private PicByStates shedSetSut;

        public PicByStates ShedSetBut
        {
            get { return shedSetSut; }
            set
            {
                shedSetSut = value;
                OnPropertyChanged($"{nameof(ShedSetBut)}");
            }
        }

        private PicByStates _vacSetBut;

        public PicByStates VacSetBut
        {
            get { return _vacSetBut; }
            set
            {
                _vacSetBut = value;
                OnPropertyChanged(nameof(VacSetBut));
            }
        }
        #endregion

        public void Init(PicturesSetStates picturesSetStates)
        {
            switch (picturesSetStates)
            {
                case PicturesSetStates.Base:
                    {
                        IPBut =new PicByStates("ok_but_base_off.png", "ok_but_base_on.png");
                        Background = "background_base.png";
                        Loading = "loading_base.jpg";
                        LoadingPic = "loading_pic_base.png";
                        Substrate = new PicByStates(def: "substrate_off_base.png", selected: "substrate_on_base.png");
                        Title = "title_base.png";
                        BackButton = new PicByStates("back_arrow_base.png", "test.jpg");
                        ResetButton = new PicByStates("reset_but_base.png", "test.jpg");
                        JournalStroke = "journal_stroke_base.png";
                        SelectStroke = new PicByStates("journal_stroke_base.png", "test.jpg");
                        TModeStroke = new PicByStates("tmode_stroke_off_base.png", "tmode_stroke_on_base.png");
                        TimeDigitalImage = "time_dig_img_base.png";
                        #region Главное окно
                        AlarmMainIcon = new PicByStates("", "alarm_main_base.png");
                        Filter100MainIcon = new PicByStates("", "filter_100_main_base.png");
                        EHeaterHeader = new PicByStates("", "eheater_header_base.png");
                        FanHeader = new PicByStates("", "fan_header_base.png");
                        Filter70Header = new PicByStates("", "filter70_header_base.png");
                        LinkHeader = new PicByStates("", "link_header_base.png");
                        RecuperatorHeader = new PicByStates("", "recuperator_header_base.png");
                        #endregion
                        #region Разное
                        FilterPol = "filter_pol_base.png";
                        Efficiency = "eff_base.png";
                        SupCons = "supply_cons_base.png";
                        ExhaustCons = "exhaust_cons_base.png";
                        OutDoorTemp = "out_temp_base.png";
                        RoomTemp = "room_temp_base.png";
                        #endregion
                        #region Кухня, счетчики
                        AcceptButton = new PicByStates("accept_but_base.png", "test.jpg");
                        CancelButton = new PicByStates("cancel_but_base.png", "test.jpg");
                        DigitalImage = "dig_img_base.png";
                        #region 4 кнопки вверх+низ
                        DigitalButtonsUp = new List<PicByStates>();

                        DigitalButtonsUp.Add(new PicByStates("dig_but_up_off_base.png", "dig_but_up_on_base.png"));
                        DigitalButtonsUp.Add(new PicByStates("dig_but_up_off_base.png", "dig_but_up_on_base.png"));
                        DigitalButtonsUp.Add(new PicByStates("dig_but_up_off_base.png", "dig_but_up_on_base.png"));
                        DigitalButtonsUp.Add(new PicByStates("dig_but_up_off_base.png", "dig_but_up_on_base.png"));
                        DigitalButtonsUp.Add(new PicByStates("dig_but_up_off_base.png", "dig_but_up_on_base.png"));

                        DigitalButtonsDn = new List<PicByStates>();
                        DigitalButtonsDn.Add(new PicByStates("dig_but_dn_off_base.png", "dig_but_dn_on_base.png"));
                        DigitalButtonsDn.Add(new PicByStates("dig_but_dn_off_base.png", "dig_but_dn_on_base.png"));
                        DigitalButtonsDn.Add(new PicByStates("dig_but_dn_off_base.png", "dig_but_dn_on_base.png"));
                        DigitalButtonsDn.Add(new PicByStates("dig_but_dn_off_base.png", "dig_but_dn_on_base.png"));
                        DigitalButtonsDn.Add(new PicByStates("dig_but_dn_off_base.png", "dig_but_dn_on_base.png"));

                        KitchenButtonUp = new PicByStates("dig_but_up_off_base.png", "dig_but_up_on_base.png");
                        KitchenButtonDn = new PicByStates("dig_but_dn_off_base.png", "dig_but_dn_on_base.png");
                        #endregion

                        #endregion
                        #region Уставки
                        HomeButton = new PicByStates("home_but_base.png", "test.jpg");
                        NextButton = new PicByStates("next_but_base.png", "test.jpg");
                        OkButton = new PicByStates("ok_but_base_off.png", "ok_but_base_on.png");
                        SettingsButton = new PicByStates("settings_but_base.png", "test.jpg");
                        #endregion
                        #region Кнопки активных режимов
                        ActiveModesPics = new ObservableCollection<PicByStates>();
                        ActiveModesPics.Add(new PicByStates("turnoff_but_on_base.png", "test.jpg"));
                        ActiveModesPics.Add(new PicByStates("min_but_on_base.png", "test.jpg"));
                        ActiveModesPics.Add(new PicByStates("norm_but_on_base.png", "test.jpg"));
                        ActiveModesPics.Add(new PicByStates("max_but_on_base.png", "test.jpg"));
                        ActiveModesPics.Add(new PicByStates("kitchen_but_on_base.png", "test.jpg"));
                        ActiveModesPics.Add(new PicByStates("vac_but_on_base.png", "test.jpg"));
                        ActiveModesPics.Add(new PicByStates("shed_but_on_base.png", "test.jpg"));
                        ActiveModesPics.Add(new PicByStates("alarm_but_base.png", "test.jpg"));
                        ActiveModesPics.Add(new PicByStates("spec_but_on_base.png", "test.jpg"));
                        #endregion
                        #region Кнопки выбора режимов
                        SelectModesPics = new ObservableCollection<PicByStates>();
                        SelectModesPics.Add(new PicByStates("turnoff_but_select_on_base.png", "test.jpg"));
                        SelectModesPics.Add(new PicByStates("min_but_select_on_base.png", "test.jpg"));
                        SelectModesPics.Add(new PicByStates("norm_but_select_on_base.png", "test.jpg"));
                        SelectModesPics.Add(new PicByStates("max_but_select_on_base.png", "test.jpg"));
                        SelectModesPics.Add(new PicByStates("kitchen_but_select_on_base.png", "test.jpg"));
                        SelectModesPics.Add(new PicByStates("vac_but_select_on_base.png", "test.jpg"));
                        SelectModesPics.Add(new PicByStates("shed_but_select_on_base.png", "test.jpg"));
                        SelectModesPics.Add(new PicByStates("min_but_select_on_base.png", "test.jpg"));
                        SelectModesPics.Add(new PicByStates("min_but_select_on_base.png", "test.jpg"));
                        #endregion
                        #region Иконки режимов 
                        IconsPics = new ObservableCollection<PicByStates>();
                        IconsPics.Add(new PicByStates("turn_off_icon_off_base.png", "turn_off_icon_off_base.png"));
                        IconsPics.Add(new PicByStates("min_icon_off_base.png", "min_icon_off_base.png"));
                        IconsPics.Add(new PicByStates("norm_icon_off_base", "norm_icon_off_base.png"));
                        IconsPics.Add(new PicByStates("max_icon_off_base.png", "max_icon_off_base.png"));
                        IconsPics.Add(new PicByStates("kitchen_icon_off_base.png", "kitchen_icon_off_base.png"));
                        IconsPics.Add(new PicByStates("shed_icon_off_base.png", "shed_icon_off_base.png"));
                        IconsPics.Add(new PicByStates("vac_icon_off_base.png", "vac_icon_off_base.png"));
                        IconsPics.Add(new PicByStates("min_icon_off_base.png", "min_icon_off_base.png"));
                        IconsPics.Add(new PicByStates("min_icon_off_base.png", "min_icon_off_base.png"));
                        #endregion
                        #region Минииконки режимов
                        //Что это и зачем нужно
                        MiniIconsPics = new ObservableCollection<string>();
                        MiniIconsPics.Add("turn_off_icon_off_base.png");
                        MiniIconsPics.Add("min_icon_off_base.png");
                        MiniIconsPics.Add("norm_icon_off_base.png");
                        MiniIconsPics.Add("max_icon_off_base.png");
                        MiniIconsPics.Add("kitchen_icon_off_base.png");
                        MiniIconsPics.Add("vac_icon_off_base.png");
                        MiniIconsPics.Add("shed_icon_off_base");
                        MiniIconsPics.Add("");
                        MiniIconsPics.Add("");
                        #endregion
                        #region Кнопки настроек
                        JournalBut = new PicByStates("journal_but_off_base.png", "journal_but_on_base.png");
                        ModesSetBut = new PicByStates("modes_set_but_off_base.png", "modes_set_but_on_base.png");
                        SettingsOtherBut = new PicByStates("settings_other_but_off_base.png", "settings_other_but_on_base.png");
                        ShedSetBut = new PicByStates("shed_set_but_off_base.png", "shed_set_but_on_base.png");
                        VacSetBut = new PicByStates("vac_set_but_off_base.png", "vac_set_but_on_base.png");
                        #endregion
                        #region Другие настройки
                        FilterChangedBut = new PicByStates("other_set_but_off_base.png", "test.jpg");
                        TimeBut = new PicByStates("other_set_but_off_base.png", "test.jpg");
                        HumidityBut = new PicByStates("other_set_but_off_base.png", "test.jpg");
                        ArrowButLeft = new PicByStates("but_left_off_base.png", "but_left_on_base.png");
                        ArrowButRight = new PicByStates("but_right_off_base.png", "but_right_on_base.png");
                        ContactBackGround = "other_set_but_off_base.png";
                        #endregion
                    }
                    break;
                default:
                    break;
            }
        }

        public void SetPicureSetIfNeed(PicByStates picPyStates, string set)
        {
            if (picPyStates.Current != set)
            {
                picPyStates.Current = set;
            }
        }
    }


}
