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
            set { 
                _loading = value;
                OnPropertyChanged(nameof(Loading));
            }
        }

        private string _loadingPic;
        public string LoadingPic
        {
            get { return _loadingPic; }
            set { 
                _loadingPic = value;
                OnPropertyChanged(nameof(LoadingPic));
            }
        }



        private string _title;

        public string Title
        {
            get { return _title; }
            set { 
                _title = value; 
                OnPropertyChanged($"{nameof(Title)}");
            }
        }


        private PicByStates _substrate;

        public PicByStates Substrate
        {
            get { return _substrate; }
            set
            {
                _substrate = value;
                OnPropertyChanged($"{nameof(Substrate)}");
            }
        }

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
            set { 
                _backButton = value;
                OnPropertyChanged(nameof(BackButton));
            }
        }

        private PicByStates _resetButton;

        public PicByStates ResetButton
        {
            get { return _resetButton; }
            set { 
                _resetButton = value;
                OnPropertyChanged(nameof(_resetButton));            
            }
        }

        private string _journalStroke;
        public string JournalStroke
        {
            get { return _journalStroke; }
            set { 
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
            get{ return _digitalButtonsUp;}
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
            set {
                _digitalButtonsDn = value;
                OnPropertyChanged(nameof(DigitalButtonsDn));
            }
        }

        private string _digitalImage;

        public string DigitalImage
        {
            get { return _digitalImage; }
            set { 
                _digitalImage = value;
                OnPropertyChanged($"{nameof(DigitalImage)}");
            }
        }
        #endregion

        private ObservableCollection<PicByStates> _activeModesPics = new ObservableCollection<PicByStates>();

        public ObservableCollection<PicByStates> ActiveModesPicks
        {
            get { return _activeModesPics; }
            set
            {
                _activeModesPics = value;
                OnPropertyChanged(nameof(ActiveModesPicks));
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
            set { 
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
                        Background = "background_base.png";
                        Loading = "loading_base.jpg";
                        LoadingPic = "loading_pic_base.png";
                        Substrate = new PicByStates(def: "substrate_base.png", selected: "test.jpg");
                        Title = "title_base.png";
                        BackButton = new PicByStates("back_arrow_base.png", "back_arrow_base.png");
                        ResetButton = new PicByStates("reset_but_base.png","test.jpg");
                        JournalStroke = "journal_stroke_base.png";
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

                        DigitalButtonsDn = new List<PicByStates>();
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
                        OkButton = new PicByStates("ok_but_base.png", "ok_but_base.png");
                        SettingsButton = new PicByStates("settings_but_base.png", "test.jpg");
                        #endregion
                        #region Кнопки активных режимов
                        ActiveModesPicks = new ObservableCollection<PicByStates>();
                        ActiveModesPicks.Add(new PicByStates("turnoff_but_on_base.png", "test.jpg"));
                        ActiveModesPicks.Add(new PicByStates("min_but_on_base.png", "test.jpg"));
                        ActiveModesPicks.Add(new PicByStates("norm_but_on_base.png", "test.jpg"));
                        ActiveModesPicks.Add(new PicByStates("max_but_on_base.png", "test.jpg"));
                        ActiveModesPicks.Add(new PicByStates("kitchen_but_on_base.png", "test.jpg"));
                        ActiveModesPicks.Add(new PicByStates("vac_but_on_base.png", "test.jpg"));
                        ActiveModesPicks.Add(new PicByStates("shed_but_on_base.png", "test.jpg"));
                        ActiveModesPicks.Add(new PicByStates("alarm_but_base.png", "test.jpg"));
                        ActiveModesPicks.Add(new PicByStates("spec_but_on_base.png", "test.jpg"));
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
                        MiniIconsPics = new ObservableCollection<string>();
                        MiniIconsPics.Add("turnoff_but_on_base.png");
                        MiniIconsPics.Add("min_but_on_base.png");
                        MiniIconsPics.Add("norm_but_on_base.png");
                        MiniIconsPics.Add("max_but_on_base.png");
                        MiniIconsPics.Add("kitchen_but_on_base.png");
                        MiniIconsPics.Add("shed_but_on_base.png");
                        MiniIconsPics.Add("");
                        MiniIconsPics.Add("");
                        MiniIconsPics.Add("");
                        #endregion
                        #region Кнопки настроек
                        JournalBut = new PicByStates("journal_but_base.jpg", "test.jpg");
                        ModesSetBut = new PicByStates("modes_set_but_base.jpg", "test.jpg");
                        SettingsOtherBut = new PicByStates("settings_other_but_base.jpg", "test.jpg");
                        ShedSetBut = new PicByStates("shed_set_but_base.jpg", "test.jpg");
                        VacSetBut = new PicByStates("vac_set_but_base.jpg", "test.jpg");
                        #endregion

                    }
                    break;
                default:
                    break;
            }
        }
    }


}
