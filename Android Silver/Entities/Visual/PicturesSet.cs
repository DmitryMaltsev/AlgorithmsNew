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
            set
            {
                _ipBut = value;
                OnPropertyChanged(nameof(IPBut));
            }
        }




        #endregion

        #region Главное окно

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

        #region Filters
        private PicByStates _filterHeader;

        public PicByStates FilterHeader
        {
            get { return _filterHeader; }
            set
            {
                _filterHeader = value;
                OnPropertyChanged(nameof(FilterHeader));
            }
        }

        #endregion
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

        private PicByStates _mode1Select;

        public PicByStates Mode1Select
        {
            get { return _mode1Select; }
            set
            {
                _mode1Select = value;
                OnPropertyChanged(nameof(Mode1Select));
            }
        }
        private PicByStates _recuperatorHeader;
        public PicByStates RecuperatorHeader
        {
            get { return _recuperatorHeader; }
            set
            {
                _recuperatorHeader = value;
                OnPropertyChanged(nameof(RecuperatorHeader));
            }
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

        private string _miscellaneous;

        public string Miscellaneous
        {
            get { return _miscellaneous; }
            set
            {
                _miscellaneous = value;
                OnPropertyChanged(nameof(Miscellaneous));
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

        private PicByStates _nextButton1;
        public PicByStates NextButton1
        {
            get { return _nextButton1; }
            set
            {
                _nextButton1 = value;
                OnPropertyChanged(nameof(NextButton1));
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

        private PicByStates _prevButton;
        public PicByStates PrevButton
        {
            get { return _prevButton; }
            set
            {
                _prevButton = value;
                OnPropertyChanged(nameof(PrevButton));
            }
        }

        private PicByStates _prevButton1;
        public PicByStates PrevButton1
        {
            get { return _prevButton1; }
            set
            {
                _prevButton1 = value;
                OnPropertyChanged(nameof(PrevButton1));
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

        private string _isSpecMode;

        public string IsSpecMode
        {
            get { return _isSpecMode; }
            set
            {
                _isSpecMode = value;
                OnPropertyChanged(nameof(IsSpecMode));
            }
        }

        #endregion

        #region Кнопки и картинки уставок

        private string _strokeImg;

        public string StrokeImg
        {
            get { return _strokeImg; }
            set
            {
                _strokeImg = value;
                OnPropertyChanged(nameof(StrokeImg));
            }
        }

        private List<PicByStates> _subButs;
        public List<PicByStates> SubButs
        {
            get { return _subButs; }
            set
            {
                _subButs = value;
                OnPropertyChanged(nameof(SubButs));
            }
        }

        private List<PicByStates> _addButs;

        public List<PicByStates> AddButs
        {
            get { return _addButs; }
            set
            {
                _addButs = value;
                OnPropertyChanged(nameof(AddButs));
            }
        }

        private PicByStates _leftBut;

        public PicByStates LeftBut
        {
            get { return _leftBut; }
            set {
                _leftBut = value; 
                OnPropertyChanged(nameof(LeftBut));
            }
        }

        private PicByStates _rightBut;

        public PicByStates RightBut
        {
            get { return _rightBut; }
            set { 
                _rightBut = value; 
                OnPropertyChanged(nameof(RightBut));
            }
        }

        //private PicByStates _sFanRightBut;
        //public PicByStates SFanRightBut
        //{
        //    get { return _sFanRightBut; }
        //    set { 
        //        _sFanRightBut = value;

        //    }
        //}

        //private PicByStates _sFanLeftBut;
        //public PicByStates SFanLeftBut
        //{
        //    get { return _sFanLeftBut; }
        //    set { _sFanLeftBut = value; }
        //}

        //private PicByStates _eFanRightBut;
        //public PicByStates EFanRightBut
        //{
        //    get { return _eFanRightBut; }
        //    set { 
        //        _eFanRightBut = value;
        //        OnPropertyChanged(nameof(EFanRightBut));
        //    }
        //}

        //private PicByStates _eFanLeftBut;
        //public PicByStates EFanLeftBut
        //{
        //    get { return _eFanLeftBut; }
        //    set { _eFanLeftBut = value; }
        //}

        //private PicByStates _tempSPRightBut;
        //public PicByStates TempSPRightBut
        //{
        //    get { return _tempSPRightBut; }
        //    set { _tempSPRightBut = value; }
        //}

        //private PicByStates _tempSPLeftBut;
        //public PicByStates TempSPLeftBut
        //{
        //    get { return _tempSPLeftBut; }
        //    set { _tempSPLeftBut = value; }
        //}


        //private PicByStates _sFanCorrLeftBut;
        //public PicByStates SFanCorrLeftBut
        //{
        //    get { return _sFanCorrLeftBut; }
        //    set {
        //        _sFanCorrLeftBut = value;
        //        OnPropertyChanged(nameof(SFanCorrLeftBut));
        //    }
        //}
        //private PicByStates _sFanCorrRightBut;
        //public PicByStates SFanCorrRightBut
        //{
        //    get { return _sFanCorrRightBut; }
        //    set
        //    {
        //        _sFanCorrRightBut = value;
        //        OnPropertyChanged(nameof(SFanCorrRightBut));
        //    }
        //}
        //private PicByStates _eFanCorrLeftBut;
        //public PicByStates EFanCorrLeftBut
        //{
        //    get { return _eFanCorrLeftBut; }
        //    set
        //    {
        //        _eFanCorrLeftBut = value;
        //        OnPropertyChanged(nameof(EFanCorrLeftBut));
        //    }
        //}
        //private PicByStates _eFanCorrRightBut;
        //public PicByStates EFanCorrRightBut
        //{
        //    get { return _eFanCorrRightBut; }
        //    set
        //    {
        //        _eFanCorrRightBut = value;
        //        OnPropertyChanged(nameof(EFanCorrRightBut));
        //    }
        //}
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
        private string _contactBackground;
        public string ContactBackground
        {
            get { return _contactBackground; }
            set
            {
                _contactBackground = value;
                OnPropertyChanged(nameof(ContactBackground));
            }
        }

        private string _humidityBackground;

        public string HumidityBackground
        {
            get { return _humidityBackground; }
            set { 
                _humidityBackground = value;
                OnPropertyChanged(nameof(HumidityBackground));
            }
        }

        private PicByStates _clocksSettingsBut;
        public PicByStates ClocksSettingsBut
        {
            get { return _clocksSettingsBut; }
            set
            {
                _clocksSettingsBut = value;
                OnPropertyChanged(nameof(ClocksSettingsBut));
            }
        }

        private PicByStates _updaterSettingsBut;
        public PicByStates UpdaterSettingsBut
        {
            get { return _updaterSettingsBut; }
            set
            {
                _updaterSettingsBut = value;
                OnPropertyChanged(nameof(UpdaterSettingsBut));
            }
        }

        private PicByStates _corrSettingsBut;
        public PicByStates CorrSettingsBut
        {
            get { return _corrSettingsBut; }
            set
            {
                _corrSettingsBut = value;
                OnPropertyChanged(nameof(CorrSettingsBut));
            }
        }

        private PicByStates _updaterBut;

        public PicByStates UpdaterBut
        {
            get { return _updaterBut; }
            set
            {
                _updaterBut = value;
                OnPropertyChanged(nameof(UpdaterBut));
            }
        }

        private PicByStates _resetBut;

        public PicByStates ResetBut
        {
            get { return _resetBut; }
            set
            {
                _resetBut = value;
                OnPropertyChanged(nameof(ResetBut));
            }
        }


        private PicByStates _downloadBut;

        public PicByStates DownloadBut
        {
            get { return _downloadBut; }
            set
            {
                _downloadBut = value;
                OnPropertyChanged(nameof(DownloadBut));
            }
        }

        private PicByStates _specModeSwitch;

        public PicByStates SpecModeSwitch
        {
            get { return _specModeSwitch; }
            set {
                _specModeSwitch = value;
                OnPropertyChanged(nameof(SpecModeSwitch));
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

        #region Главное меню
        private List<PicByStates> _baseSettings1ButCollection;

        public List<PicByStates> BaseSettings1ButCollection
        {
            get { return _baseSettings1ButCollection; }
            set
            {
                _baseSettings1ButCollection = value;
                OnPropertyChanged(nameof(BaseSettings1ButCollection));
            }
        }

        private PicByStates _baseSettings2But;
        public PicByStates BaseSettings2But
        {
            get { return _baseSettings2But; }
            set
            {
                _baseSettings2But = value;
                OnPropertyChanged(nameof(BaseSettings2But));
            }
        }

        private string _flowIcon;

        public string FlowIcon
        {
            get { return _flowIcon; }
            set
            {
                _flowIcon = value;
                OnPropertyChanged(nameof(FlowIcon));
            }
        }

        private string _tempIcon;

        public string TempIcon
        {
            get { return _tempIcon; }
            set
            {
                _tempIcon = value;
                OnPropertyChanged(nameof(TempIcon));
            }
        }


        #endregion

        private ObservableCollection<string> _activeModesPics = new ObservableCollection<string>();

        public ObservableCollection<string> ActiveModesPics
        {
            get { return _activeModesPics; }
            set
            {
                _activeModesPics = value;
                OnPropertyChanged(nameof(ActiveModesPics));
            }
        }

        private ObservableCollection<string> _activeMode2Pics = new ObservableCollection<string>();

        public ObservableCollection<string> ActiveMode2Pics
        {
            get { return _activeMode2Pics; }
            set
            {
                _activeMode2Pics = value;
                OnPropertyChanged(nameof(ActiveMode2Pics));
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

        private PicByStates _shedSetBut;
        public PicByStates ShedSetBut
        {
            get { return _shedSetBut; }
            set
            {
                _shedSetBut = value;
                OnPropertyChanged($"{nameof(ShedSetBut)}");
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

        private PicByStates _informationBut;

        public PicByStates InformationBut
        {
            get { return _informationBut; }
            set
            {
                _informationBut = value;
                OnPropertyChanged($"{nameof(InformationBut)}");
            }
        }

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

        private PicByStates _helpterBut;
        public PicByStates HelperBut
        {
            get { return _helpterBut; }
            set
            {
                _helpterBut = value;
                OnPropertyChanged(nameof(HelperBut));
            }
        }
        #endregion

        #region InfoPics

        private string _ventInfo;

        public string VentInfo
        {
            get { return _ventInfo; }
            set
            {
                _ventInfo = value;
                OnPropertyChanged(nameof(VentInfo));
            }
        }

        private string _tempInfo;

        public string TempInfo
        {
            get { return _tempInfo; }
            set
            {
                _tempInfo = value;
                OnPropertyChanged(nameof(TempInfo));
            }
        }



        private string _recupInfo;

        public string RecupInfo
        {
            get { return _recupInfo; }
            set
            {
                _recupInfo = value;
                OnPropertyChanged(nameof(RecupInfo));
            }
        }
        private string _eHeaterInfo;
        public string EHeaterInfo
        {
            get { return _eHeaterInfo; }
            set
            {
                _eHeaterInfo = value;
                OnPropertyChanged(nameof(EHeaterInfo));
            }
        }

        private string _otherInfo;
        public string OtherInfo
        {
            get { return _otherInfo; }
            set
            {
                _otherInfo = value;
                OnPropertyChanged(nameof(OtherInfo));
            }
        }





        #endregion

        public PicturesSet()
        {
            Init(PicturesSetStates.Base);
        }

        public void Init(PicturesSetStates picturesSetStates)
        {
            switch (picturesSetStates)
            {
                case PicturesSetStates.Base:
                    {
                        IPBut = new PicByStates("ok_but_off_base.jpg", "ok_but_on_base.jpg");
                        Background = "background_base.png";
                        Loading = "loading_pic_base_sign.jpg";
                        LoadingPic = "loading_pic_base_sign.jpg";
                        Title = "title_base.png";
                        BackButton = new PicByStates("back_but_off_base.png", "back_but_on_base.png");
                        ResetButton = new PicByStates("reset_but_off_base.jpg", "reset_but_on_base.jpg");
                        JournalStroke = "journal_stroke_base.jpg";
                        SelectStroke = new PicByStates("journal_stroke_base.png", "test.jpg");
                        TModeStroke = new PicByStates("tmode_stroke_off_base.jpg", "");
                        #region Главное окно
                        Mode1Select = new PicByStates("select_mode_base_off.jpg", "select_mode_base_on.jpg");
                        AlarmMainIcon = new PicByStates("", "alarm_main_base.png");
                        EHeaterHeader = new PicByStates("eheater_header_base.jpg", "");
                        FanHeader = new PicByStates("fan_header_base.jpg", "");
                        FilterHeader = new PicByStates("filter_header_base.jpg", "");
                        RecuperatorHeader = new PicByStates("recup_header_base.jpg", "");
                        LinkHeader = new PicByStates("", "link_header_base.png");
                        TempIcon = "temp_icon_base.jpg";
                        FlowIcon = "flow_icon_base.jpg";
                        #endregion
                        #region Разное
                        FilterPol = "filter_pol_base.png";
                        Efficiency = "eff_base.png";
                        SupCons = "supply_cons_base.png";
                        ExhaustCons = "exhaust_cons_base.png";
                        OutDoorTemp = "out_temp_base.png";
                        RoomTemp = "room_temp_base.png";
                        Miscellaneous = "miscel_base.png";
                        #endregion
                        #region Уставки
                        SubButs = [new PicByStates("sub_but_off_base.jpg", "sub_but_on_base.jpg"), 
                                   new PicByStates("sub_but_off_base.jpg", "sub_but_on_base.jpg"),
                                   new PicByStates("sub_but_off_base.jpg", "sub_but_on_base.jpg"),
                                   new PicByStates("sub_but_off_base.jpg", "sub_but_on_base.jpg"),
                                   new PicByStates("sub_but_off_base.jpg", "sub_but_on_base.jpg")];
                        AddButs = [new PicByStates("add_but_off_base.jpg", "add_but_on_base.jpg"),
                                   new PicByStates("add_but_off_base.jpg", "add_but_on_base.jpg"),
                                   new PicByStates("add_but_off_base.jpg", "add_but_on_base.jpg"),
                                   new PicByStates("add_but_off_base.jpg", "add_but_on_base.jpg"),
                                   new PicByStates("add_but_off_base.jpg", "add_but_on_base.jpg")];
                        LeftBut = new PicByStates("left_but_off_base.jpg", "left_but_on_base.jpg");
                        RightBut = new PicByStates("right_but_off_base.jpg", "right_but_on_base.jpg");
                        StrokeImg = "stroke_img_base.jpg";
                        #endregion
                        #region Уставки
                        HomeButton = new PicByStates("home_but_off_base.png", "home_but_on_base.png");
                        NextButton = new PicByStates("right_but_off_base.jpg", "right_but_on_base.jpg");
                        NextButton1 = new PicByStates("right_but_off_base.jpg", "right_but_on_base.jpg");
                        PrevButton = new PicByStates("left_but_off_base.jpg", "left_but_on_base.jpg");
                        PrevButton1 = new PicByStates("left_but_off_base.jpg", "left_but_on_base.jpg");
                        OkButton = new PicByStates("ok_but_off_base.jpg", "ok_but_on_base.jpg");
                        SettingsButton = new PicByStates("settings_base_off.jpg", "settings_base_on.jpg");
                        #endregion
                        #region Картинки активных режимов
                        ActiveModesPics = new ObservableCollection<string>();
                        ActiveModesPics.Add("turn_off_but_base.png");
                        ActiveModesPics.Add("min_but_base.png");
                        ActiveModesPics.Add("norm_but_base.png");
                        ActiveModesPics.Add("max_but_base.png");
                        ActiveModesPics.Add("kitchen_but_base.png");
                        ActiveModesPics.Add("vac_but_base.png");
                        ActiveModesPics.Add("spec_but_base.png");
                        ActiveModesPics.Add("alarm_but_base.png");
                        ActiveModesPics.Add("spec_but_base.png");
                        #endregion
                        #region Иконки актиыный режимов 2
                        ActiveMode2Pics = new ObservableCollection<string>();
                        ActiveMode2Pics.Add("turnoff_but_on_base.png");
                        ActiveMode2Pics.Add("mode2_kitchen_base.png");
                        ActiveMode2Pics.Add("mode2_vac_base.png");
                        ActiveMode2Pics.Add("mode2_shed_base.png");
                        #endregion
                        #region Кнопки выбора режимов
                        SelectModesPics = new ObservableCollection<PicByStates>();
                        SelectModesPics.Add(new PicByStates("turnoff_but_select_off_base.png", "turnoff_but_select_on_base.png"));
                        SelectModesPics.Add(new PicByStates("min_but_select_off_base.png", "min_but_select_on_base.png"));
                        SelectModesPics.Add(new PicByStates("norm_but_select_off_base.png", "norm_but_select_on_base.png"));
                        SelectModesPics.Add(new PicByStates("max_but_select_off_base.png", ("max_but_select_on_base.png")));
                        SelectModesPics.Add(new PicByStates("kitchen_but_select_off_base.png", "kitchen_but_select_on_base.png"));
                        SelectModesPics.Add(new PicByStates("vac_but_select_off_base.png", "vac_but_select_on_base.png"));
                        SelectModesPics.Add(new PicByStates("shed_but_select_off_base.png", "shed_but_select_on_base.png"));
                        SelectModesPics.Add(new PicByStates("min_but_select_off_base.png", "min_but_select_on_base.png"));
                        SelectModesPics.Add(new PicByStates("min_but_select_off_base.png", "min_but_select_on_base.png"));
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
                        MiniIconsPics.Add("");
                        MiniIconsPics.Add("");
                        MiniIconsPics.Add("");
                        #endregion
                        #region Кнопки настроек
                        ShedSetBut = new PicByStates("shed_set_but_off_base.jpg", "shed_set_but_on_base.jpg");
                        ModesSetBut = new PicByStates("modes_set_but_off_base.jpg", "modes_set_but_on_base.jpg");
                        SettingsOtherBut = new PicByStates("settings_other_but_off_base.jpg", "settings_other_but_on_base.jpg");
                        InformationBut = new PicByStates("information_but_off_base.jpg", "information_but_on_base.jpg");
                        JournalBut = new PicByStates("journal_but_off_base.jpg", "journal_but_on_base.jpg");
                        HelperBut = new PicByStates("helper_but_off_base.jpg", "helper_but_on_base.jpg");
                        #endregion
                        #region Информационный экран
                        VentInfo = "vent_info_icon.jpg";
                        RecupInfo = "recup_info_icon.jpg";
                        TempInfo = "temp_info_icon.jpg";
                        EHeaterInfo = "eheater_info_icon.jpg";
                        OtherInfo = "other_info_icon.jpg";
                        #endregion
                        #region Другие настройки
                        ContactBackground = "other_settings_stroke_base.jpg";
                        HumidityBackground = "other_settings_stroke_base.jpg";
                        ClocksSettingsBut = new PicByStates("clocks_settings_but_off_base.jpg", "clocks_settings_but_on_base.jpg");
                        UpdaterSettingsBut = new PicByStates("updater_settings_but_off.jpg", "updater_settings_but_on.jpg");
                        CorrSettingsBut = new PicByStates("corr_settings_but_off_base.jpg", "corr_settings_but_on_base.jpg");
                        UpdaterBut = new PicByStates("other_set_but_off_base.png", "other_set_but_on_base.png");
                        DownloadBut = new PicByStates("other_set_but_off_base.png", "other_set_but_on_base.png");
                        ResetBut = new PicByStates("other_set_but_off_base.png", "other_set_but_on_base.png");
                        SpecModeSwitch = new PicByStates("spec_mode_off_base.jpg", "spec_mode_on_base.jpg");
                        #endregion
                        #region Основные настройки
                        BaseSettings1ButCollection = new List<PicByStates>();
                        for (int i = 0; i < 20; i++)
                        {
                            BaseSettings1ButCollection.Add(new PicByStates("settings_base_off.jpg", "settings_base_on.jpg"));
                        }
                        BaseSettings2But = new PicByStates("settings_base_off.jpg", "settings_base_on.jpg");
                        IsSpecMode = "is_sheduler_base.png";
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
