using Android_Silver.ViewModels;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Android_Silver.Entities.Visual
{
    public class PicturesSet : BindableBase
    {

        public void Init(PicturesSetStates picturesSetStates)
        {
            switch (picturesSetStates)
            {
                case PicturesSetStates.Base:
                    {
                        Background = "background_base.png";
                        Substrate = new PicByStates(def: "substrate_base.png", selected: "");

                        #region Кухня, счетчики
                        AcceptButton = new PicByStates("accept_but_base.png", "accept_but_base.png");
                        CancelButton = new PicByStates("cancel_but_base.png", "cancel_but_base.png");
                        DigitalButton = new PicByStates("dig_but_base.png", "dig_but_base.png");
                        #endregion
                        #region Кнопки настроек
                        HomeButton = new PicByStates("accept_but_base.png", "accept_but_base.png");
                        NextButton = new PicByStates("next_but_base.png", "next_but_base.png");
                        OkButton = new PicByStates("ok_but_base.png", "ok_but_base.png");
                        SettingsButton = new PicByStates("settings_but_base.png", "settings_but_base.png");
                        #endregion
                        #region Кнопки активных режимов
                        ActiveModesPicks = new ObservableCollection<PicByStates>();
                        ActiveModesPicks.Add(new PicByStates("turnoff_but_on_base.png", "turnoff_but_on_base.png"));
                        ActiveModesPicks.Add(new PicByStates("min_but_on_base.png", "min_but_on_base.png"));
                        ActiveModesPicks.Add(new PicByStates("norm_but_on_base.png", "norm_but_on_base.png"));
                        ActiveModesPicks.Add(new PicByStates("max_but_on_base.png", "max_but_on_base.png"));
                        ActiveModesPicks.Add(new PicByStates("kitchen_but_on_base.png", "kitchen_but_on_base.png"));
                        ActiveModesPicks.Add(new PicByStates("vac_but_on_base.png", "vac_but_on_base.png"));
                        ActiveModesPicks.Add(new PicByStates("shed_but_on_base.png", "shed_but_on_base.png"));
                        ActiveModesPicks.Add(new PicByStates("alarm_but_base.png", "alarm_but_base.png"));
                        ActiveModesPicks.Add(new PicByStates("spec_but_on_base.png", "spec_but_on_base.png"));
                        #endregion
                        #region Кнопки выбора режимов
                        SelectModesPicks = new ObservableCollection<PicByStates>();
                        ActiveModesPicks.Add(new PicByStates("kitchen_but_select_on_base.png", "kitchen_but_select_on_base.png"));
                        ActiveModesPicks.Add(new PicByStates("min_but_select_on_base.png", "min_but_select_on_base.png"));
                        ActiveModesPicks.Add(new PicByStates("norm_but_select_on_base.png", "norm_but_select_on_base.png"));
                        ActiveModesPicks.Add(new PicByStates("max_but_select_on_base.png", "max_but_select_on_base.png"));
                        ActiveModesPicks.Add(new PicByStates("kitchen_but_select_on_base.png", "kitchen_but_select_on_base.png"));
                        ActiveModesPicks.Add(new PicByStates("vac_but_select_on_base.png", "vac_but_select_on_base.png"));
                        ActiveModesPicks.Add(new PicByStates("shed_but_select_on_base.png", "shed_but_select_on_base.png"));
                        #endregion
                        #region Иконки режимов 
                        IconsPics = new ObservableCollection<PicByStates>();
                        IconsPics.Add(new PicByStates("turn_off_icon_off_base.png", "turn_off_icon_off_base.png"));
                        IconsPics.Add(new PicByStates("min_icon_off_base.png", "min_icon_off_base.png"));
                        IconsPics.Add(new PicByStates("norm_icon_off_base", "norm_icon_off_base.png"));
                        IconsPics.Add(new PicByStates("max_icon_off_base.png", "max_icon_off_base.png"));
                        IconsPics.Add(new PicByStates("shed_icon_off_base.png", "shed_icon_off_base.png"));
                        IconsPics.Add(new PicByStates("vac_icon_off_base.png", "vac_icon_off_base.png"));

                        #endregion
                        #region Кнопки настроек
                        JournalBut = new PicByStates("journal_but_base.png", "journal_but_base.png");
                        ModesSetBut = new PicByStates("modes_set_but_base.png", "modes_set_but_base.png");
                        SettingsOtherBut = new PicByStates("settings_other_but_base.png", "settings_other_but_base.png");
                        ShedSetBut = new PicByStates("shed_set_but_base.png", "shed_set_but_base.png");
                        VacSetBut = new PicByStates("vac_set_but_base.png", "vac_set_but_base.png");
                        #endregion
                    }
                    break;
                default:
                    break;
            }
        }

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

        private PicByStates _digitalButton;

        public PicByStates DigitalButton
        {
            get
            {
                return _digitalButton;
            }
            set
            {
                _digitalButton = value;
                OnPropertyChanged(nameof(DigitalButton));
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

        private ObservableCollection<PicByStates> _selectModesPicks = new ObservableCollection<PicByStates>();

        public ObservableCollection<PicByStates> SelectModesPicks
        {
            get { return _selectModesPicks; }
            set
            {
                _selectModesPicks = value;
                OnPropertyChanged(nameof(SelectModesPicks));
            }
        }

        //Иконки а календаре, возможно при сменен уставок режима
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
    }
}
