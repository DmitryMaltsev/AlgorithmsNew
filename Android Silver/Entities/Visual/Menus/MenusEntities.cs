using Android_Silver.Entities.FBEntities;
using Android_Silver.ViewModels;

using System.Collections.ObjectModel;

namespace Android_Silver.Entities.Visual.Menus
{
    public class MenusEntities : BindableBase
    {
        public int WriteOffset { get; set; } = 5000;
        public int ETH_COMMON_SETTINGS_ADDR;
        public int ETH_COMMON_SETTINGS_LENGTH;

        public int ETH_DAMPER_SETTINGS_ADDR;
        public int ETH_DAMPER_SETTINGS_LENGTH;

        public int ETH_FAN_SETTINGS_ADDR;
        public int ETH_FAN_SETTINGS_LENGTH;

        public int ETH_WH_SETTINGS_ADDR;
        public int ETH_WH_SETTINGS_LENGTH;

        public int ETH_EH_SETTINGS_ADDR;
        public int ETH_EH_SETTINGS_LENGTH;

        public int ETH_FREON_SETTINGS_ADDR;
        public int ETH_FREON_SETTINGS_LENGTH;

        public int ETH_HUM_SETTINGS_ADDR;
        public int ETH_HUM_SETTINGS_LENGTH;

        public int ETH_RECUP_SETTINGS_ADDR;
        public int ETH_RECUP_SETTINGS_LENGTH;

        public int ETH_SENS_SETTINGS_ADDR;
        public int ETH_SENS_SETTINGS_LENGTH;

        public int ETH_CONFIG_SETTINGS_ADDR;
        public int ETH_CONFIG_SETTINGS_LENGTH;

        public int ETH_THM_SETTINGS_ADDR;
        public int ETH_THM_SETTINGS_LENGTH;

        public int ETH_MBRECUP_SETTINGS_ADDR;
        public int ETH_MBRECUP_SETTINGS_LENGTH;

        public int ETH_SPECMODE_SETTINGS_ADDR;
        public int ETH_SPECMODE_SETTINGS_LENGTH;

        public int ETH_CALIBRATE_THM_ADDR;
        public int ETH_CALIBRATE_THM_LENGTH;

        public int ETH_RECUP_CURRENTSETTINGS_ADDR;
        public int ETH_RECUP_CURRENTSETTINGS_LENGTH;

        //Основная таблица 
        public List<MItem> StartMenuCollection { get; set; }

        private ObservableCollection<MItem> _menusCollection;
        public ObservableCollection<MItem> MenusCollection
        {
            get { return _menusCollection; }
            set
            {
                _menusCollection = value;
                OnPropertyChanged(nameof(MenusCollection));
            }
        }

        private ObservableCollection<StrSet> _interfaceStrCollecton;

        public ObservableCollection<StrSet> InterfaceStrCollection
        {
            get { return _interfaceStrCollecton; }
            set
            {
                _interfaceStrCollecton = value;
                OnPropertyChanged($"{nameof(InterfaceStrCollection)}");
            }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }


        public void GenerateBaseTable()
        {
            MenusCollection = new ObservableCollection<MItem>();
            foreach (var stroke in StartMenuCollection)
            {
                if (stroke.MenuIsVisible)
                {
                    MenusCollection.Add(stroke);
                }
            }
        }

        public void GenerateInterfaceTable(int index)
        {

            InterfaceStrCollection = new ObservableCollection<StrSet>();
            foreach (var strSet in StartMenuCollection[index].StrSetsCollection)
            {
                if (strSet.IsVisible)
                {
                    InterfaceStrCollection.Add(strSet);
                }
            }
        }

        private int PidMaxVal = 10_000;

        private PicturesSet _pictureSet { get; set; }

        FBs _fbEntities;

        public MenusEntities()
        {
            _fbEntities = DIContainer.Resolve<FBs>();
            ETH_COMMON_SETTINGS_ADDR = 300;
            ETH_COMMON_SETTINGS_LENGTH = 15;
            ETH_DAMPER_SETTINGS_ADDR = ETH_COMMON_SETTINGS_ADDR + ETH_COMMON_SETTINGS_LENGTH;
            ETH_DAMPER_SETTINGS_LENGTH = 23;
            ETH_FAN_SETTINGS_ADDR = ETH_DAMPER_SETTINGS_ADDR + ETH_DAMPER_SETTINGS_LENGTH;
            ETH_FAN_SETTINGS_LENGTH = 11;
            ETH_WH_SETTINGS_ADDR = ETH_FAN_SETTINGS_ADDR + ETH_FAN_SETTINGS_LENGTH;
            ETH_WH_SETTINGS_LENGTH = 16;
            ETH_EH_SETTINGS_ADDR = ETH_WH_SETTINGS_ADDR + ETH_WH_SETTINGS_LENGTH;
            ETH_EH_SETTINGS_LENGTH = 5;
            ETH_FREON_SETTINGS_ADDR = ETH_EH_SETTINGS_ADDR + ETH_EH_SETTINGS_LENGTH;
            ETH_FREON_SETTINGS_LENGTH = 6;
            ETH_HUM_SETTINGS_ADDR = ETH_FREON_SETTINGS_ADDR + ETH_FREON_SETTINGS_LENGTH;
            ETH_HUM_SETTINGS_LENGTH = 6;
            ETH_RECUP_SETTINGS_ADDR = ETH_HUM_SETTINGS_ADDR + ETH_HUM_SETTINGS_LENGTH;
            ETH_RECUP_SETTINGS_LENGTH = 11;
            ETH_SENS_SETTINGS_ADDR = ETH_RECUP_SETTINGS_ADDR + ETH_RECUP_SETTINGS_LENGTH;
            ETH_SENS_SETTINGS_LENGTH = 5;
            ETH_CONFIG_SETTINGS_ADDR = ETH_SENS_SETTINGS_ADDR + ETH_SENS_SETTINGS_LENGTH;
            ETH_CONFIG_SETTINGS_LENGTH = 8;
            ETH_THM_SETTINGS_ADDR = ETH_CONFIG_SETTINGS_ADDR + ETH_CONFIG_SETTINGS_LENGTH;
            ETH_THM_SETTINGS_LENGTH = 13;
            ETH_MBRECUP_SETTINGS_ADDR = ETH_THM_SETTINGS_ADDR + ETH_THM_SETTINGS_LENGTH;
            ETH_MBRECUP_SETTINGS_LENGTH = 14;
            ETH_SPECMODE_SETTINGS_ADDR = ETH_MBRECUP_SETTINGS_ADDR + ETH_MBRECUP_SETTINGS_LENGTH;
            ETH_SPECMODE_SETTINGS_LENGTH = 8;
            ETH_CALIBRATE_THM_ADDR = ETH_SPECMODE_SETTINGS_ADDR + ETH_SPECMODE_SETTINGS_LENGTH;
            ETH_CALIBRATE_THM_LENGTH = 40;
            ETH_RECUP_CURRENTSETTINGS_ADDR = ETH_CALIBRATE_THM_ADDR + ETH_CALIBRATE_THM_LENGTH;
            ETH_RECUP_CURRENTSETTINGS_LENGTH = 40;
            InterfaceStrCollection = new ObservableCollection<StrSet>();
            _pictureSet = DIContainer.Resolve<PicturesSet>();
            StartMenuCollection = new List<MItem>();
            #region Пункты меню 
            MItem mItem = new MItem("Конфигурация", isVisible: true, _pictureSet.BaseSettings1ButCollection[0], SActivePageState.ConfigPage, id: 1, startAddress: ETH_CONFIG_SETTINGS_ADDR + WriteOffset);
            StartMenuCollection.Add(mItem);
            mItem = new MItem("Общие", isVisible: true, _pictureSet.BaseSettings1ButCollection[1], SActivePageState.CommonSettingsPage, id: 2, startAddress: ETH_COMMON_SETTINGS_ADDR + WriteOffset);
            StartMenuCollection.Add(mItem);
            mItem = new MItem("Жалюзи", isVisible: true, _pictureSet.BaseSettings1ButCollection[2], SActivePageState.DamperSettingsPage, id: 3, startAddress: ETH_DAMPER_SETTINGS_ADDR + WriteOffset);
            StartMenuCollection.Add(mItem);
            mItem = new MItem("Вентилятор", isVisible: true, _pictureSet.BaseSettings1ButCollection[3], SActivePageState.FanSettingsPage, id: 4, startAddress: ETH_FAN_SETTINGS_ADDR + WriteOffset);
            StartMenuCollection.Add(mItem);
            mItem = new MItem("Нагреватель водяной", isVisible: true, _pictureSet.BaseSettings1ButCollection[4], SActivePageState.WHSettingsPage, id: 5, startAddress: ETH_WH_SETTINGS_ADDR + WriteOffset);
            StartMenuCollection.Add(mItem);
            mItem = new MItem("Нагреватель электрический", isVisible: true, _pictureSet.BaseSettings1ButCollection[5], SActivePageState.EHSettingsPage, id: 6, startAddress: ETH_EH_SETTINGS_ADDR + WriteOffset);
            StartMenuCollection.Add(mItem);
            mItem = new MItem("Охладитель фреоновый", isVisible: true, _pictureSet.BaseSettings1ButCollection[6], SActivePageState.FreonSettingsPage, id: 7, startAddress: ETH_FREON_SETTINGS_ADDR + WriteOffset);
            StartMenuCollection.Add(mItem);
            mItem = new MItem("Рекуператор", isVisible: true, _pictureSet.BaseSettings1ButCollection[7], SActivePageState.RecupSettingsPage, id: 8, startAddress: ETH_RECUP_SETTINGS_ADDR + WriteOffset);
            StartMenuCollection.Add(mItem);
            mItem = new MItem("Увлажнитель", isVisible: true, _pictureSet.BaseSettings1ButCollection[8], SActivePageState.HumSettingsPage, id: 9, startAddress: ETH_HUM_SETTINGS_ADDR + WriteOffset);
            StartMenuCollection.Add(mItem);
            mItem = new MItem("Настройка аналоговых датчиков", isVisible: true, _pictureSet.BaseSettings1ButCollection[9], SActivePageState.SensorsSettingPage, id: 10, startAddress: ETH_SENS_SETTINGS_ADDR + WriteOffset);
            StartMenuCollection.Add(mItem);
            mItem = new MItem("Настройка термоанемометров", isVisible: true, _pictureSet.BaseSettings1ButCollection[10], SActivePageState.TmhSettingsPage, id: 11, startAddress: ETH_THM_SETTINGS_ADDR + WriteOffset);
            StartMenuCollection.Add(mItem);
            mItem = new MItem("Настройка Modbus шагового мотора", isVisible: true, _pictureSet.BaseSettings1ButCollection[11], SActivePageState.MBRecupSettingsPage, id: 12, startAddress: ETH_MBRECUP_SETTINGS_ADDR + WriteOffset);
            StartMenuCollection.Add(mItem);
            mItem = new MItem("Настройки спец режима", isVisible: true, _pictureSet.BaseSettings1ButCollection[12], SActivePageState.SpecModeSettingsPage, id: 13, startAddress: ETH_SPECMODE_SETTINGS_ADDR + WriteOffset);
            StartMenuCollection.Add(mItem);
            mItem = new MItem("Калибровка термоанемоетров", isVisible: true, _pictureSet.BaseSettings1ButCollection[13], SActivePageState.ThmCalibratePage, id: 14, startAddress: ETH_CALIBRATE_THM_ADDR + WriteOffset);
            StartMenuCollection.Add(mItem);
            mItem = new MItem("Настройка профилей шаг мотора", isVisible: true, _pictureSet.BaseSettings1ButCollection[14], SActivePageState.RecupCurrentPage, id: 15, startAddress: ETH_RECUP_CURRENTSETTINGS_ADDR + WriteOffset);
            StartMenuCollection.Add(mItem);
            #endregion

            #region Конфигурация
            ObservableCollection<StrSet> strSets = new ObservableCollection<StrSet>();
            List<string> pickVals = new List<string>() { "Нет", "Вод нагр 1", "Вод нагр 2", "Фр охл 1", "Фр охл 2", "Увл 1", "Увл 2" };
            StrSet sSet = new StrSet(0, 20, "Аналоговый выход ET1", isVisible: true, pickerIsVisible: true, entryIsVisible: false, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 20, "Аналоговый выход ET2", isVisible: true, pickerIsVisible: true, entryIsVisible: false, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 20, "Дискретный выход OUT1", isVisible: true, pickerIsVisible: true, entryIsVisible: false, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 20, "Дискретный выход OUT2", isVisible: true, pickerIsVisible: true, entryIsVisible: false, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            pickVals = new List<string>() { "Нет", "Кач-во возд", "Увл 1", "Увл 2" };
            sSet = new StrSet(0, 20, "Аналоговый вход 0-10В AR1", isVisible: true, pickerIsVisible: true, entryIsVisible: false, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 20, "Аналоговый вход 0-10В AR2", isVisible: true, pickerIsVisible: true, entryIsVisible: false, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 20, "Аналоговый вход 0-10В AR3", isVisible: true, pickerIsVisible: true, entryIsVisible: false, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            pickVals = new List<string>() { "Нет", "Драйвер", "Modbus", "MB1+MB2" };
            sSet = new StrSet(0, 4, "Рекуператор", isVisible: true, pickerIsVisible: true, entryIsVisible: false, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            StartMenuCollection[0].StrSetsCollection = strSets;
            #endregion
            #region Общие
            strSets = new ObservableCollection<StrSet>();
            pickVals = new List<string>() { "Канал", "Комната" };
            sSet = new StrSet(-10, 100, "Уставка аварийной темп, °C", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 1, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 1, "Регулирование темп. по", isVisible: true, pickerIsVisible: true, entryIsVisible: false, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Уставка темп.кан.макс.,°C", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 1, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Уставка темп. кан. мин.,°C", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 1, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 65535, "Задержка аварии по темп, сек", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            pickVals = new List<string>() { "Авто", "Зима", "Лето" };
            sSet = new StrSet(0, 3, "Тип регулирования времени года", isVisible: true, pickerIsVisible: true, entryIsVisible: false, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 30, "Темп. перехода зима/лето, °С", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 1, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 20, "Гистерезис темп. перехода,°С", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 1, pickVals);
            strSets.Add(sSet);
            pickVals = new List<string>() { "Откл", "Вкл" };
            sSet = new StrSet(0, 5, "Перезапуск по питанию", isVisible: true, pickerIsVisible: true, entryIsVisible: false, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 5, "Автосброс пожара", isVisible: true, pickerIsVisible: true, entryIsVisible: false, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 0.7f, "Сила тока УФ-светодиодов, А", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 2, pickVals);
            strSets.Add(sSet);
            pickVals = new List<string>() { "Неактивен", "Активен" };
            sSet = new StrSet(0, 1, "Демо режим", isVisible: true, pickerIsVisible: true, entryIsVisible: false, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.CCommonSetPoints.RoomSPPReg.Min, _fbEntities.CCommonSetPoints.RoomSPPReg.Max, "P коэф. комн температуры", isVisible: true, pickerIsVisible: false, entryIsVisible: true,
            isEnabled: true, valScale: _fbEntities.CCommonSetPoints.RoomSPPReg.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.CCommonSetPoints.RoomSPIReg.Min, _fbEntities.CCommonSetPoints.RoomSPIReg.Max, "I коэф. комн температуры", isVisible: true, pickerIsVisible: false, entryIsVisible: true,
            isEnabled: true, valScale: _fbEntities.CCommonSetPoints.RoomSPIReg.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.CCommonSetPoints.RoomSPDReg.Min, _fbEntities.CCommonSetPoints.RoomSPDReg.Max, "D коэф. комн температуры", isVisible: true, pickerIsVisible: false, entryIsVisible: true,
            isEnabled: true, valScale: _fbEntities.CCommonSetPoints.RoomSPDReg.NumChr, pickVals);
            strSets.Add(sSet);
            StartMenuCollection[1].StrSetsCollection = strSets;
            #endregion
            #region Жалюзи
            strSets = new ObservableCollection<StrSet>();
            pickVals = new List<string>() { "Нет", "Да" };
            sSet = new StrSet(0, 65000, "Время открытия, сек", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 65000, "Время прогрева", isVisible: false, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Сервопривод 1 начальная поз", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Сервопривод 1 конечная поз", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Сервопривод 2 начальная поз", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Сервопривод 2 конечная поз", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Сервопривод 3 начальная поз", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Сервопривод 3 конечная поз", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Сервопривод 4 начальная поз", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Сервопривод 4 конечная поз", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 90, "Сервопривод 1 положение 1 градусов", isVisible: false, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 90, "Сервопривод 1 положение 2 градусов", isVisible: false, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 90, "Сервопривод 2 положение 1 градусов", isVisible: false, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 90, "Сервопривод 2 положение 2 градусов", isVisible: false, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 90, "Сервопривод 3 положение 1 градусов", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 90, "Сервопривод 3 положение 2 градусов", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 90, "Сервопривод 4  положение 1 градусов", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 90, "Сервопривод 4 положение 2", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 1, "Тестовый режим", isVisible: true, pickerIsVisible: true, entryIsVisible: false, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Калибровка серво 1 0-100%", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Калибровка серво 2 0-100%", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Калибровка серво 3 0-100%", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Калибровка серво 4 0-100%", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            StartMenuCollection[2].StrSetsCollection = strSets;



            #endregion
            #region Вентилятор
            strSets = new ObservableCollection<StrSet>();
            pickVals = new List<string>() { "Откл", "Вкл" };
            //sSet = new StrSet(0, 1, "Вентилятор вытяжки", isVisible: true, pickerIsVisible: true, entryIsVisible: false, pickVals);
            //strStes.Add(sSet);
            sSet = new StrSet(0, 100000, "Номин.расход вент. прит, м3/ч", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100000, "Номин.расход вент. вытяжки, м3/ч", isVisible: false, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Нижняя граница запрета, %", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Верхняя граница запрета, %", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 65000, "Задержка контроля давления, сек", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 65000, "Задержка контроля аварии вентилятора, сек", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 1, "Функция снижения оборотов", isVisible: true, pickerIsVisible: true, entryIsVisible: false, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 1000, "P коэф. регулятора", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 1000, "I коэф. регулятора", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 1000, "D коэф. регулятора", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Минимальный % вент", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            StartMenuCollection[3].StrSetsCollection = strSets;
            #endregion
            #region WHHeater
            strSets = new ObservableCollection<StrSet>();
            pickVals = new List<string>() { "Откл", "Вкл" };
            sSet = new StrSet(0, PidMaxVal, "P коэф. регулятора (работа)", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, PidMaxVal, "I коэф. регулятора (работа)", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, PidMaxVal, "D коэф. регулятора (работа)", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, PidMaxVal, "P коэф. регулятора (огран)", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, PidMaxVal, "I коэф. регулятора (огран)", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, PidMaxVal, "D коэф. регулятора (огран)", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Max. темп. обратной воды, °C", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 1, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Min. темп. обратной воды, °C", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 1, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Дежурная темп. обратной воды, °C", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 1, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Авар. темп. обратной воды, °C", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 1, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Пусковая темп. обратной воды, °C", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 1, pickVals);
            strSets.Add(sSet);
            // не отображаем, потом потом удалить
            sSet = new StrSet(0, 100, "Пусковая темп. обратной воды2, °C", isVisible: false, pickerIsVisible: false, entryIsVisible: false, isEnabled: true, valScale: 1, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 65_000, "Время мягкого пуска, сек", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Минимальный % КЗР", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Температура 'зимних процедур'", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 1, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 1, "Проба насоса летом", isVisible: true, pickerIsVisible: true, entryIsVisible: false, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            StartMenuCollection[4].StrSetsCollection = strSets;
            #endregion 
            #region EHHeater
            strSets = new ObservableCollection<StrSet>();
            pickVals = new List<string>() { "Откл", "Вкл" };
            sSet = new StrSet(0, 100_000, "Мощность нагревателя, Вт", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, PidMaxVal, "P коэф. регулятора", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, PidMaxVal, "I коэф. регулятора", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, PidMaxVal, "D коэф. регулятора", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 65_000, "Время продувки, сек", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            StartMenuCollection[5].StrSetsCollection = strSets;
            #endregion
            #region FreonCooler
            strSets = new ObservableCollection<StrSet>();
            pickVals = new List<string>() { "Откл", "Вкл" };
            sSet = new StrSet(0, PidMaxVal, "P коэф. регулятора", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, PidMaxVal, "I коэф. регулятора", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, PidMaxVal, "D коэф. регулятора", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 65535, "Задержка включения ступени, сек", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 65535, "Минимальное время работы ступени, сек", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, PidMaxVal, "Гистерезис вкл/выкл фреонового охладителя", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            StartMenuCollection[6].StrSetsCollection = strSets;
            #endregion
            #region Recup
            strSets = new ObservableCollection<StrSet>();
            pickVals = new List<string>() { "Откл", "Вкл" };
            sSet = new StrSet(0, PidMaxVal, "P коэф. регулятора", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, PidMaxVal, "I коэф. регулятора", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, PidMaxVal, "D коэф. регулятора", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(-50, 50, "Граница темп. авар. КПД, °С", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 1, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Аварийный КПД, %", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 65535, "Задержка контроля КПД, мин", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Максимальная частота, Гц", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(-50, 50, "Температура А, °С", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 1, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(-50, 50, "Температура В, °С", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 1, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(-50, 50, "Температура C, °С", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 1, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(-50, 50, "Температура D, °С", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 1, pickVals);
            strSets.Add(sSet);
            StartMenuCollection[7].StrSetsCollection = strSets;
            #endregion
            #region Humidity 
            strSets = new ObservableCollection<StrSet>();
            pickVals = new List<string>() { "Откл", "Вкл" };
            sSet = new StrSet(0, PidMaxVal, "P коэф. регулятора", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, PidMaxVal, "I коэф. регулятора", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, PidMaxVal, "D коэф. регулятора", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 65535, "Задержка включения ступени, сек", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 65535, "Минимальное. время работы ступени, сек", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, PidMaxVal, "Гистерезис вкл/выкл увлажнителя", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            StartMenuCollection[8].StrSetsCollection = strSets;
            #endregion
            #region Sensors
            strSets = new ObservableCollection<StrSet>();
            pickVals = new List<string>() { "Нет", "Есть" };
            /*  sSet = new StrSet(0, 2, "Датчик уличной температуры", isVisible: true, pickerIsVisible: true, entryIsVisible: false, pickVals);
                 strSets.Add(sSet);
                 sSet = new StrSet(0, 2, "Датчик температуры приточного канала", isVisible: true, pickerIsVisible: true, entryIsVisible: false, pickVals);
                 strSets.Add(sSet);
                 sSet = new StrSet(0, 3, "Датчик температуры вытяжного канала", isVisible: true, pickerIsVisible: true, entryIsVisible: false, pickVals);
                 strSets.Add(sSet);
                 sSet = new StrSet(0, 2, "Датчик комнатной температуры", isVisible: true, pickerIsVisible: true, entryIsVisible: false, pickVals);
                 strSets.Add(sSet);
                 sSet = new StrSet(0, 2, "Датчик температуры обратной   воды", isVisible: true, pickerIsVisible: true, entryIsVisible: false, pickVals);
                 strSets.Add(sSet);
                 sSet = new StrSet(0, 1, "Датчик качества воздуха", isVisible: true, pickerIsVisible: true, entryIsVisible: false, pickVals);
                 strSets.Add(sSet);
                 sSet = new StrSet(0, 1, "Датчик влажности", isVisible: true, pickerIsVisible: true, entryIsVisible: false, pickVals);
                 strSets.Add(sSet);
               */
            sSet = new StrSet(-100, 100, "Коррекция датчика темп улицы", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 1, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(-100, 100, "Коррекция датчика темп приточного канала", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 1, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(-100, 100, "Коррекция датчика темп вытяжного канала", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 1, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(-100, 100, "Коррекция датчика темп комнаты", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 1, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(-100, 100, "Коррекция датчика темп обратной воды", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 1, pickVals);
            strSets.Add(sSet);
            StartMenuCollection[9].StrSetsCollection = strSets;
            #endregion
            #region Термоанемометры
            strSets = new ObservableCollection<StrSet>();
            pickVals = new List<string>() { "CAV", "DC" };
            sSet = new StrSet(_fbEntities.SupCalibrateThm.FanControlType.Min, _fbEntities.SupCalibrateThm.FanControlType.Max, "Тип управления CAV/DC",
                isVisible: true, pickerIsVisible: true, entryIsVisible: false, isEnabled: true, valScale: _fbEntities.SupCalibrateThm.FanControlType.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(-100, +120, "Температура H1 °C", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: false, valScale: 1, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(-100, +120, "Температура C1 °C", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: false, valScale: 1, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(-100, +120, "Температура H2 °C", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: false, valScale: 1, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(-100, +120, "Температура C2 °C", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: false, valScale: 1, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.ThmSps.PReg.Min, _fbEntities.ThmSps.PReg.Max, "P коэф.регулятора", isVisible: true, pickerIsVisible: false, entryIsVisible: true,
                isEnabled: true, valScale: _fbEntities.ThmSps.PReg.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.ThmSps.IReg.Min, _fbEntities.ThmSps.IReg.Max, "I коэф.регулятора", isVisible: true, pickerIsVisible: false, entryIsVisible: true,
                isEnabled: true, valScale: _fbEntities.ThmSps.IReg.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.ThmSps.DReg.Min, _fbEntities.ThmSps.DReg.Max, "D коэф.регулятора", isVisible: true, pickerIsVisible: false, entryIsVisible: true,
                isEnabled: true, valScale: _fbEntities.ThmSps.DReg.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.ThmSps.KPolKoef.Min, _fbEntities.ThmSps.KPolKoef.Max, "Коэф. К грязного фильтра", isVisible: true, pickerIsVisible: false, entryIsVisible: true,
                isEnabled: true, valScale: _fbEntities.ThmSps.KPolKoef.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.ThmSps.BPolKoef.Min, _fbEntities.ThmSps.BPolKoef.Max, "Коэф. B грязного фильтра", isVisible: true, pickerIsVisible: false, entryIsVisible: true,
                isEnabled: true, valScale: _fbEntities.ThmSps.BPolKoef.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.ThmSps.KClKoef.Min, _fbEntities.ThmSps.KClKoef.Max, "Коэф. К чистого фильтра", isVisible: true, pickerIsVisible: false, entryIsVisible: true,
                isEnabled: true, valScale: _fbEntities.ThmSps.KClKoef.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.ThmSps.BClKoef.Min, _fbEntities.ThmSps.BClKoef.Max, "Коэф. B чистого фильтра", isVisible: true, pickerIsVisible: false, entryIsVisible: true,
                isEnabled: true, valScale: _fbEntities.ThmSps.BClKoef.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.ThmSps.U.Min, _fbEntities.ThmSps.U.Max, "Напряжение термоанeмометра", isVisible: true, pickerIsVisible: false,
                entryIsVisible: true, isEnabled: true, valScale: _fbEntities.ThmSps.U.NumChr, pickVals);
            strSets.Add(sSet);
            StartMenuCollection[10].StrSetsCollection = strSets;
            #endregion
            #region Modbus рекуператор
            strSets = new ObservableCollection<StrSet>();
            pickVals = new List<string>() { "SR_OPEN", "SR_Close", "SR_vFoc" };
            sSet = new StrSet(0, 2, "Режим работы", isVisible: true, pickerIsVisible: true, entryIsVisible: false, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            pickVals = new List<string>() { "Нет", "Да" };
            sSet = new StrSet(0, 1, "Тест вращения 1", isVisible: true, pickerIsVisible: true, entryIsVisible: false, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            pickVals = new List<string>() { "Нет", "Да" };
            sSet = new StrSet(0, 1, "Тест вращения 2", isVisible: true, pickerIsVisible: true, entryIsVisible: false, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            pickVals = new List<string>() { "Пр часовой", "По часовой" };
            sSet = new StrSet(0, 1, "Направление вращения 1", isVisible: true, pickerIsVisible: true, entryIsVisible: false, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            pickVals = new List<string>() { "Пр часовой", "По часовой" };
            sSet = new StrSet(0, 1, "Направление вращения 2", isVisible: true, pickerIsVisible: true, entryIsVisible: false, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            pickVals = new List<string>() { "Нет", "Да" };
            sSet = new StrSet(0, 1, "Притирка ротора", isVisible: true, pickerIsVisible: true, entryIsVisible: false, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 10_000, "Номинальный ток, ма", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Коэффициент редукции", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 2, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Номин. обороты 1, об в мин.", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Номин. обороты 2, об в мин.", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(-70, 70, "Темп для номинала 1, °С", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 1, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(-70, 70, "Темп для номинала 2, °С", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 1, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 10_000, "Притирочный ток, мА", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Притирочные обороты, об в мин.", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            StartMenuCollection[11].StrSetsCollection = strSets;
            #endregion
            #region Спец режим 
            strSets = new ObservableCollection<StrSet>();
            sSet = new StrSet(0, 100, "Минимальная производительность притока", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Текущая производительность притока", isVisible: false, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Максимальная производительность притока", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Минимальная производительность вытяжки", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Текущая производительность вытяжки", isVisible: false, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Максимальная производительность вытяжки", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 40, "Уставка температуры", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Ограничение мощности", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: 0, pickVals);
            strSets.Add(sSet);
            StartMenuCollection[12].StrSetsCollection = strSets;
            #endregion
            #region Калибровка
            strSets = new ObservableCollection<StrSet>();
            pickVals = new List<string>() { "Нет", "ПВ", "П", "В", "Тест" };
            sSet = new StrSet(_fbEntities.SupCalibrateThm.CalibrateMode.Min, _fbEntities.SupCalibrateThm.CalibrateMode.Max, "Режим калибровки",
            isVisible: true, pickerIsVisible: true, entryIsVisible: false, isEnabled: true, valScale: _fbEntities.SupCalibrateThm.CalibrateMode.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.SupCalibrateThm.DeltaThm.Min, _fbEntities.SupCalibrateThm.DeltaThm.Max, "Дельта термоанемометра П,°C", isVisible: true,
                pickerIsVisible: false, entryIsVisible: true, isEnabled: false, valScale: _fbEntities.SupCalibrateThm.DeltaThm.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.ExhaustCalibrateThm.DeltaThm.Min, _fbEntities.ExhaustCalibrateThm.DeltaThm.Max, "Дельта термоанемометра В,°C", isVisible: true,
                pickerIsVisible: false, entryIsVisible: true, isEnabled: false, valScale: _fbEntities.ExhaustCalibrateThm.DeltaThm.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.SupCalibrateThm.CalibrateTimeS.Min, _fbEntities.SupCalibrateThm.CalibrateTimeS.Max, "Время ступени калибровки,с", isVisible: true,
            pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: _fbEntities.SupCalibrateThm.CalibrateTimeS.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.SupCalibrateThm.TestTimeS.Min, _fbEntities.SupCalibrateThm.TestTimeS.Max, "Время ступени прогона, с", isVisible: true,
                pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: _fbEntities.SupCalibrateThm.CalibrateTimeS.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.SupCalibrateThm.LeakFlow.Min, _fbEntities.SupCalibrateThm.LeakFlow.Max, "Расход утечки П, м3/ч", isVisible: true,
                pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: _fbEntities.SupCalibrateThm.LeakFlow.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.ExhaustCalibrateThm.LeakFlow.Min, _fbEntities.ExhaustCalibrateThm.LeakFlow.Max, "Расход утечки В, м3/ч", isVisible: true,
               pickerIsVisible: false, entryIsVisible: true, isEnabled: true, valScale: _fbEntities.ExhaustCalibrateThm.LeakFlow.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.SupCalibrateThm.CalibrateStepsLimits.Min, _fbEntities.SupCalibrateThm.CalibrateStepsLimits.Max,
                "Cтупень калибровки 1,%", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                                                                                    valScale: _fbEntities.SupCalibrateThm.CalibrateStepsLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.SupCalibrateThm.CalibrateStepsLimits.Min, _fbEntities.SupCalibrateThm.CalibrateStepsLimits.Max,
                "Cтупень калибровки 2,%", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                                                                          valScale: _fbEntities.SupCalibrateThm.CalibrateStepsLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.SupCalibrateThm.CalibrateStepsLimits.Min, _fbEntities.SupCalibrateThm.CalibrateStepsLimits.Max,
                "Cтупень калибровки 3,%", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                                                                        valScale: _fbEntities.SupCalibrateThm.CalibrateStepsLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.SupCalibrateThm.CalibrateStepsLimits.Min, _fbEntities.SupCalibrateThm.CalibrateStepsLimits.Max,
                "Cтупень калибровки 4,%", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                                                                        valScale: _fbEntities.SupCalibrateThm.CalibrateStepsLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.SupCalibrateThm.CalibrateStepsLimits.Min, _fbEntities.SupCalibrateThm.CalibrateStepsLimits.Max,
                "Cтупень калибровки 5,%", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                                                                        valScale: _fbEntities.SupCalibrateThm.CalibrateStepsLimits.NumChr, pickVals);
            strSets.Add(sSet);
            //Калибровочные дельты темп притока
            sSet = new StrSet(_fbEntities.SupCalibrateThm.DeltaTCalibratesLimits.Min, _fbEntities.SupCalibrateThm.DeltaTCalibratesLimits.Max,
                "Калиб. дельта t притока 0, °С", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                                                valScale: _fbEntities.SupCalibrateThm.DeltaTCalibratesLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.SupCalibrateThm.DeltaTCalibratesLimits.Min, _fbEntities.SupCalibrateThm.DeltaTCalibratesLimits.Max,
                 "Калиб. дельта t притока 1, °С", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                                                valScale: _fbEntities.SupCalibrateThm.DeltaTCalibratesLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.SupCalibrateThm.DeltaTCalibratesLimits.Min, _fbEntities.SupCalibrateThm.DeltaTCalibratesLimits.Max,
                "Калиб. дельта t притока 2, °С", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                                                valScale: _fbEntities.SupCalibrateThm.DeltaTCalibratesLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.SupCalibrateThm.DeltaTCalibratesLimits.Min, _fbEntities.SupCalibrateThm.DeltaTCalibratesLimits.Max,
                 "Калиб. дельта t притока 3, °С", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                                                valScale: _fbEntities.SupCalibrateThm.DeltaTCalibratesLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.SupCalibrateThm.DeltaTCalibratesLimits.Min, _fbEntities.SupCalibrateThm.DeltaTCalibratesLimits.Max,
                "Калиб. дельта t притока 4, °С", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                                                valScale: _fbEntities.SupCalibrateThm.DeltaTCalibratesLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.SupCalibrateThm.DeltaTCalibratesLimits.Min, _fbEntities.SupCalibrateThm.DeltaTCalibratesLimits.Max,
                "Калиб. дельта t притока 5, °С", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                                                valScale: _fbEntities.SupCalibrateThm.DeltaTCalibratesLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.SupCalibrateThm.DeltaTCalibratesLimits.Min, _fbEntities.SupCalibrateThm.DeltaTCalibratesLimits.Max,
                "Калиб. дельта t притока Max, °С", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                                    valScale: _fbEntities.SupCalibrateThm.DeltaTCalibratesLimits.NumChr, pickVals);
            strSets.Add(sSet);
            //Калибровочные расходы притока
            sSet = new StrSet(_fbEntities.SupCalibrateThm.FlowCalibratesLimits.Min, _fbEntities.SupCalibrateThm.FlowCalibratesLimits.Max,
                "Калиб. расход притока 0, м3/ч", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                                    valScale: _fbEntities.SupCalibrateThm.FlowCalibratesLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.SupCalibrateThm.FlowCalibratesLimits.Min, _fbEntities.SupCalibrateThm.FlowCalibratesLimits.Max,
                "Калиб. расход притока 1, м3/ч", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                        valScale: _fbEntities.SupCalibrateThm.FlowCalibratesLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.SupCalibrateThm.FlowCalibratesLimits.Min, _fbEntities.SupCalibrateThm.FlowCalibratesLimits.Max,
                "Калиб. расход притока 2, м3/ч", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                        valScale: _fbEntities.SupCalibrateThm.FlowCalibratesLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.SupCalibrateThm.FlowCalibratesLimits.Min, _fbEntities.SupCalibrateThm.FlowCalibratesLimits.Max,
                "Калиб. расход притока 3, м3/ч", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                        valScale: _fbEntities.SupCalibrateThm.FlowCalibratesLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.SupCalibrateThm.FlowCalibratesLimits.Min, _fbEntities.SupCalibrateThm.FlowCalibratesLimits.Max,
                "Калиб. расход притока 4, м3/ч", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                        valScale: _fbEntities.SupCalibrateThm.FlowCalibratesLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.SupCalibrateThm.FlowCalibratesLimits.Min, _fbEntities.SupCalibrateThm.FlowCalibratesLimits.Max,
                "Калиб. расход притока 5, м3/ч", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                        valScale: _fbEntities.SupCalibrateThm.FlowCalibratesLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.SupCalibrateThm.FlowCalibratesLimits.Min, _fbEntities.SupCalibrateThm.FlowCalibratesLimits.Max,
                "Калиб. расход притока Max, м3/ч", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                        valScale: _fbEntities.SupCalibrateThm.FlowCalibratesLimits.NumChr, pickVals);
            strSets.Add(sSet);
            //Калибровочные дельты темп вытяжки
            sSet = new StrSet(_fbEntities.ExhaustCalibrateThm.DeltaTCalibratesLimits.Min, _fbEntities.ExhaustCalibrateThm.DeltaTCalibratesLimits.Max,
                "Калиб. дельта t вытяжки 0, °С", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                                                valScale: _fbEntities.ExhaustCalibrateThm.DeltaTCalibratesLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.ExhaustCalibrateThm.DeltaTCalibratesLimits.Min, _fbEntities.ExhaustCalibrateThm.DeltaTCalibratesLimits.Max,
                 "Калиб. дельта t вытяжки 1, °С", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                                                valScale: _fbEntities.ExhaustCalibrateThm.DeltaTCalibratesLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.ExhaustCalibrateThm.DeltaTCalibratesLimits.Min, _fbEntities.ExhaustCalibrateThm.DeltaTCalibratesLimits.Max,
                "Калиб. дельта t вытяжки 2, °С", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                                                valScale: _fbEntities.ExhaustCalibrateThm.DeltaTCalibratesLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.ExhaustCalibrateThm.DeltaTCalibratesLimits.Min, _fbEntities.ExhaustCalibrateThm.DeltaTCalibratesLimits.Max,
                 "Калиб. дельта t вытяжки 3, °С", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                                                valScale: _fbEntities.ExhaustCalibrateThm.DeltaTCalibratesLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.ExhaustCalibrateThm.DeltaTCalibratesLimits.Min, _fbEntities.ExhaustCalibrateThm.DeltaTCalibratesLimits.Max,
                "Калиб. дельта t вытяжки 4, °С", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                                                valScale: _fbEntities.ExhaustCalibrateThm.DeltaTCalibratesLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.ExhaustCalibrateThm.DeltaTCalibratesLimits.Min, _fbEntities.ExhaustCalibrateThm.DeltaTCalibratesLimits.Max,
                "Калиб. дельта t вытяжки 5, °С", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                                                valScale: _fbEntities.ExhaustCalibrateThm.DeltaTCalibratesLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.ExhaustCalibrateThm.DeltaTCalibratesLimits.Min, _fbEntities.ExhaustCalibrateThm.DeltaTCalibratesLimits.Max,
                "Калиб. дельта t вытяжки Max, °С", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                                    valScale: _fbEntities.ExhaustCalibrateThm.DeltaTCalibratesLimits.NumChr, pickVals);
            strSets.Add(sSet);
            //Калибровочные расходы вытяжки
            sSet = new StrSet(_fbEntities.ExhaustCalibrateThm.FlowCalibratesLimits.Min, _fbEntities.ExhaustCalibrateThm.FlowCalibratesLimits.Max,
                "Калиб. расход вытяжки 0, м3/ч", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                                    valScale: _fbEntities.ExhaustCalibrateThm.FlowCalibratesLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.ExhaustCalibrateThm.FlowCalibratesLimits.Min, _fbEntities.ExhaustCalibrateThm.FlowCalibratesLimits.Max,
                "Калиб. расход вытяжки 1, м3/ч", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                        valScale: _fbEntities.ExhaustCalibrateThm.FlowCalibratesLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.ExhaustCalibrateThm.FlowCalibratesLimits.Min, _fbEntities.ExhaustCalibrateThm.FlowCalibratesLimits.Max,
                "Калиб. расход вытяжки 2, м3/ч", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                        valScale: _fbEntities.ExhaustCalibrateThm.FlowCalibratesLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.ExhaustCalibrateThm.FlowCalibratesLimits.Min, _fbEntities.ExhaustCalibrateThm.FlowCalibratesLimits.Max,
                "Калиб. расход вытяжки 3, м3/ч", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                        valScale: _fbEntities.ExhaustCalibrateThm.FlowCalibratesLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.ExhaustCalibrateThm.FlowCalibratesLimits.Min, _fbEntities.ExhaustCalibrateThm.FlowCalibratesLimits.Max,
                "Калиб. расход вытяжки 4, м3/ч", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                        valScale: _fbEntities.ExhaustCalibrateThm.FlowCalibratesLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.ExhaustCalibrateThm.FlowCalibratesLimits.Min, _fbEntities.ExhaustCalibrateThm.FlowCalibratesLimits.Max,
                "Калиб. расход вытяжки 5, м3/ч", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                        valScale: _fbEntities.ExhaustCalibrateThm.FlowCalibratesLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.ExhaustCalibrateThm.FlowCalibratesLimits.Min, _fbEntities.ExhaustCalibrateThm.FlowCalibratesLimits.Max,
                "Калиб. расход вытяжки Max, м3/ч", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                        valScale: _fbEntities.ExhaustCalibrateThm.FlowCalibratesLimits.NumChr, pickVals);
            strSets.Add(sSet);
            StartMenuCollection[13].StrSetsCollection = strSets;
            #endregion
            #region Профили шагового мотора
            strSets = new ObservableCollection<StrSet>();
            sSet = new StrSet(_fbEntities.CRecup.RecProfiles[0].I_StartLimits.Min, _fbEntities.CRecup.RecProfiles[0].I_StartLimits.Max,
            "Сила тока запуска профиль 0", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                 valScale: _fbEntities.CRecup.RecProfiles[0].I_StartLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.CRecup.RecProfiles[0].I_ContLimits.Min, _fbEntities.CRecup.RecProfiles[0].I_ContLimits.Max,
            "Сила тока работы профиль 0", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                 valScale: _fbEntities.CRecup.RecProfiles[0].I_ContLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.CRecup.RecProfiles[0].KpLimits.Min, _fbEntities.CRecup.RecProfiles[0].KpLimits.Max,
            "P коэф профиль 0", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                valScale: _fbEntities.CRecup.RecProfiles[0].KpLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.CRecup.RecProfiles[0].KiLimits.Min, _fbEntities.CRecup.RecProfiles[0].KiLimits.Max,
            "I коэф профиль 0", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                valScale: _fbEntities.CRecup.RecProfiles[0].KiLimits.NumChr, pickVals);
            strSets.Add(sSet);

            sSet = new StrSet(_fbEntities.CRecup.RecProfiles[1].I_StartLimits.Min, _fbEntities.CRecup.RecProfiles[1].I_StartLimits.Max,
            "Сила тока запуска профиль 1", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
             valScale: _fbEntities.CRecup.RecProfiles[1].I_StartLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.CRecup.RecProfiles[1].I_ContLimits.Min, _fbEntities.CRecup.RecProfiles[1].I_ContLimits.Max,
            "Сила тока работы профиль 1", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                 valScale: _fbEntities.CRecup.RecProfiles[1].I_ContLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.CRecup.RecProfiles[1].KpLimits.Min, _fbEntities.CRecup.RecProfiles[1].KpLimits.Max,
            "P коэф профиль 1", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                valScale: _fbEntities.CRecup.RecProfiles[1].KpLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.CRecup.RecProfiles[1].KiLimits.Min, _fbEntities.CRecup.RecProfiles[1].KiLimits.Max,
            "I коэф профиль 1", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                valScale: _fbEntities.CRecup.RecProfiles[1].KiLimits.NumChr, pickVals);
            strSets.Add(sSet);

            sSet = new StrSet(_fbEntities.CRecup.RecProfiles[2].I_StartLimits.Min, _fbEntities.CRecup.RecProfiles[2].I_StartLimits.Max,
            "Сила тока запуска профиль 2", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
            valScale: _fbEntities.CRecup.RecProfiles[2].I_StartLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.CRecup.RecProfiles[2].I_ContLimits.Min, _fbEntities.CRecup.RecProfiles[2].I_ContLimits.Max,
            "Сила тока работы профиль 2", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                 valScale: _fbEntities.CRecup.RecProfiles[2].I_ContLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.CRecup.RecProfiles[2].KpLimits.Min, _fbEntities.CRecup.RecProfiles[2].KpLimits.Max,
            "P коэф профиль 2", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                valScale: _fbEntities.CRecup.RecProfiles[2].KpLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.CRecup.RecProfiles[2].KiLimits.Min, _fbEntities.CRecup.RecProfiles[2].KiLimits.Max,
            "I коэф профиль 2", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                valScale: _fbEntities.CRecup.RecProfiles[2].KiLimits.NumChr, pickVals);
            strSets.Add(sSet);

            sSet = new StrSet(_fbEntities.CRecup.RecProfiles[3].I_StartLimits.Min, _fbEntities.CRecup.RecProfiles[3].I_StartLimits.Max,
            "Сила тока запуска профиль 3", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
            valScale: _fbEntities.CRecup.RecProfiles[3].I_StartLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.CRecup.RecProfiles[3].I_ContLimits.Min, _fbEntities.CRecup.RecProfiles[3].I_ContLimits.Max,
            "Сила тока работы профиль 3", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                 valScale: _fbEntities.CRecup.RecProfiles[3].I_ContLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.CRecup.RecProfiles[3].KpLimits.Min, _fbEntities.CRecup.RecProfiles[3].KpLimits.Max,
            "P коэф профиль 3", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                valScale: _fbEntities.CRecup.RecProfiles[3].KpLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.CRecup.RecProfiles[3].KiLimits.Min, _fbEntities.CRecup.RecProfiles[3].KiLimits.Max,
            "I коэф профиль 3", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                valScale: _fbEntities.CRecup.RecProfiles[3].KiLimits.NumChr, pickVals);
            strSets.Add(sSet);

            sSet = new StrSet(_fbEntities.CRecup.RecProfiles[4].I_StartLimits.Min, _fbEntities.CRecup.RecProfiles[4].I_StartLimits.Max,
            "Сила тока запуска профиль 4", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
            valScale: _fbEntities.CRecup.RecProfiles[4].I_StartLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.CRecup.RecProfiles[4].I_ContLimits.Min, _fbEntities.CRecup.RecProfiles[4].I_ContLimits.Max,
            "Сила тока работы профиль 4", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                 valScale: _fbEntities.CRecup.RecProfiles[4].I_ContLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.CRecup.RecProfiles[4].KpLimits.Min, _fbEntities.CRecup.RecProfiles[4].KpLimits.Max,
            "P коэф профиль 4", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                valScale: _fbEntities.CRecup.RecProfiles[4].KpLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.CRecup.RecProfiles[4].KiLimits.Min, _fbEntities.CRecup.RecProfiles[4].KiLimits.Max,
            "I коэф профиль 4", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                valScale: _fbEntities.CRecup.RecProfiles[4].KiLimits.NumChr, pickVals);
            strSets.Add(sSet);

            sSet = new StrSet(_fbEntities.CRecup.RecProfiles[5].I_StartLimits.Min, _fbEntities.CRecup.RecProfiles[5].I_StartLimits.Max,
            "Сила тока запуска профиль 5", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
            valScale: _fbEntities.CRecup.RecProfiles[5].I_StartLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.CRecup.RecProfiles[5].I_ContLimits.Min, _fbEntities.CRecup.RecProfiles[5].I_ContLimits.Max,
            "Сила тока работы профиль 5", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                 valScale: _fbEntities.CRecup.RecProfiles[5].I_ContLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.CRecup.RecProfiles[5].KpLimits.Min, _fbEntities.CRecup.RecProfiles[5].KpLimits.Max,
            "P коэф профиль 5", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                valScale: _fbEntities.CRecup.RecProfiles[5].KpLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.CRecup.RecProfiles[5].KiLimits.Min, _fbEntities.CRecup.RecProfiles[5].KiLimits.Max,
            "I коэф профиль 5", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                valScale: _fbEntities.CRecup.RecProfiles[5].KiLimits.NumChr, pickVals);
            strSets.Add(sSet);

            sSet = new StrSet(_fbEntities.CRecup.RecProfiles[6].I_StartLimits.Min, _fbEntities.CRecup.RecProfiles[6].I_StartLimits.Max,
            "Сила тока запуска профиль 6", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
            valScale: _fbEntities.CRecup.RecProfiles[6].I_StartLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.CRecup.RecProfiles[6].I_ContLimits.Min, _fbEntities.CRecup.RecProfiles[6].I_ContLimits.Max,
            "Сила тока работы профиль 6", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                 valScale: _fbEntities.CRecup.RecProfiles[6].I_ContLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.CRecup.RecProfiles[6].KpLimits.Min, _fbEntities.CRecup.RecProfiles[6].KpLimits.Max,
            "P коэф профиль 6", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                valScale: _fbEntities.CRecup.RecProfiles[6].KpLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.CRecup.RecProfiles[6].KiLimits.Min, _fbEntities.CRecup.RecProfiles[6].KiLimits.Max,
            "I коэф профиль 6", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                valScale: _fbEntities.CRecup.RecProfiles[6].KiLimits.NumChr, pickVals);
            strSets.Add(sSet);

            sSet = new StrSet(_fbEntities.CRecup.RecProfiles[7].I_StartLimits.Min, _fbEntities.CRecup.RecProfiles[7].I_StartLimits.Max,
            "Сила тока запуска профиль 7", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
            valScale: _fbEntities.CRecup.RecProfiles[7].I_StartLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.CRecup.RecProfiles[7].I_ContLimits.Min, _fbEntities.CRecup.RecProfiles[7].I_ContLimits.Max,
            "Сила тока работы профиль 7", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                 valScale: _fbEntities.CRecup.RecProfiles[7].I_ContLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.CRecup.RecProfiles[7].KpLimits.Min, _fbEntities.CRecup.RecProfiles[7].KpLimits.Max,
            "P коэф профиль 7", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                valScale: _fbEntities.CRecup.RecProfiles[7].KpLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.CRecup.RecProfiles[7].KiLimits.Min, _fbEntities.CRecup.RecProfiles[7].KiLimits.Max,
            "I коэф профиль 7", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                valScale: _fbEntities.CRecup.RecProfiles[7].KiLimits.NumChr, pickVals);
            strSets.Add(sSet);

            sSet = new StrSet(_fbEntities.CRecup.RecProfiles[8].I_StartLimits.Min, _fbEntities.CRecup.RecProfiles[8].I_StartLimits.Max,
            "Сила тока запуска профиль 8", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
            valScale: _fbEntities.CRecup.RecProfiles[8].I_StartLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.CRecup.RecProfiles[8].I_ContLimits.Min, _fbEntities.CRecup.RecProfiles[8].I_ContLimits.Max,
            "Сила тока работы профиль 8", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                 valScale: _fbEntities.CRecup.RecProfiles[8].I_ContLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.CRecup.RecProfiles[8].KpLimits.Min, _fbEntities.CRecup.RecProfiles[8].KpLimits.Max,
            "P коэф профиль 8", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                valScale: _fbEntities.CRecup.RecProfiles[8].KpLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.CRecup.RecProfiles[8].KiLimits.Min, _fbEntities.CRecup.RecProfiles[8].KiLimits.Max,
            "I коэф профиль 8", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                valScale: _fbEntities.CRecup.RecProfiles[8].KiLimits.NumChr, pickVals);
            strSets.Add(sSet);

            sSet = new StrSet(_fbEntities.CRecup.RecProfiles[9].I_StartLimits.Min, _fbEntities.CRecup.RecProfiles[9].I_StartLimits.Max,
               "Сила тока запуска профиль 9", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
             valScale: _fbEntities.CRecup.RecProfiles[9].I_StartLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.CRecup.RecProfiles[9].I_ContLimits.Min, _fbEntities.CRecup.RecProfiles[9].I_ContLimits.Max,
            "Сила тока работы профиль 9", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                 valScale: _fbEntities.CRecup.RecProfiles[9].I_ContLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.CRecup.RecProfiles[9].KpLimits.Min, _fbEntities.CRecup.RecProfiles[9].KpLimits.Max,
            "P коэф профиль 9", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                valScale: _fbEntities.CRecup.RecProfiles[9].KpLimits.NumChr, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(_fbEntities.CRecup.RecProfiles[9].KiLimits.Min, _fbEntities.CRecup.RecProfiles[9].KiLimits.Max,
            "I коэф профиль 9", isVisible: true, pickerIsVisible: false, entryIsVisible: true, isEnabled: true,
                valScale: _fbEntities.CRecup.RecProfiles[9].KiLimits.NumChr, pickVals);
            strSets.Add(sSet);
            StartMenuCollection[14].StrSetsCollection = strSets;

            #endregion
        }
    }
}
