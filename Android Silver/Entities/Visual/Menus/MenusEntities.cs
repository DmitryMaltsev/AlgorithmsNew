using Android_Silver.ViewModels;

using System.Collections.ObjectModel;

namespace Android_Silver.Entities.Visual.Menus
{
    public class MenusEntities : BindableBase
    {
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

        private PicturesSet _pictureSet { get; set; }

        public MenusEntities()
        {
            InterfaceStrCollection = new ObservableCollection<StrSet>();
            _pictureSet = DIContainer.Resolve<PicturesSet>();
            StartMenuCollection = new List<MItem>();
            #region Пункты меню 
            MItem mItem = new MItem("Общие", isVisible: true, _pictureSet.BaseSettings1ButCollection[0], SActivePageState.CommonSettingsPage, id: 1, startAddress: 699);
            StartMenuCollection.Add(mItem);
            mItem = new MItem("Жалюзи", isVisible: true, _pictureSet.BaseSettings1ButCollection[1], SActivePageState.DamperSettingsPage, id: 2, startAddress: 709);
            StartMenuCollection.Add(mItem);
            mItem = new MItem("Вентилятор", isVisible: true, _pictureSet.BaseSettings1ButCollection[2], SActivePageState.FanSettingsPage, id: 3, startAddress: 711);
            StartMenuCollection.Add(mItem);
            mItem = new MItem("Нагреватель водяной", isVisible: true, _pictureSet.BaseSettings1ButCollection[3], SActivePageState.WHSettingsPage, id: 4, startAddress: 722);
            StartMenuCollection.Add(mItem);
            mItem = new MItem("Нагреватель электрический", isVisible: true, _pictureSet.BaseSettings1ButCollection[4], SActivePageState.EHSettingsPage, id: 5, startAddress: 438);
            StartMenuCollection.Add(mItem);
            mItem = new MItem("Охладитель фреоновый", isVisible: true, _pictureSet.BaseSettings1ButCollection[5], SActivePageState.FreonSettingsPage, id: 6, startAddress: 743);
            StartMenuCollection.Add(mItem);
            mItem = new MItem("Рекуператор", isVisible: true, _pictureSet.BaseSettings1ButCollection[6], SActivePageState.RecupSettingsPage, id: 7, startAddress: 755);
            StartMenuCollection.Add(mItem);
            mItem = new MItem("Увлажнитель", isVisible: true, _pictureSet.BaseSettings1ButCollection[7], SActivePageState.HumSettingsPage, id: 8, startAddress: 749);
            StartMenuCollection.Add(mItem);
            mItem = new MItem("Конфигурация аналоговых датчиков", isVisible: true, _pictureSet.BaseSettings1ButCollection[8], SActivePageState.SensorsSettingPage, id: 9, startAddress: 766);
            StartMenuCollection.Add(mItem);
            #endregion

            #region Общие
            ObservableCollection<StrSet> strSets = new ObservableCollection<StrSet>();
            List<string> pickVals = new List<string>() { "Улица", "Комната" };
            StrSet sSet = new StrSet(0, 50, "Уставка аварийной темп, °C", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 1, "Регулирование темп. по", isVisible: true, pickerIsVisible: true, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Уставка темп.кан.макс.,°C", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Уставка темп. кан. мин.,°C", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 99000, "Задержка аварии по темп, сек", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            pickVals = new List<string>() { "Авто", "Зима", "Лето" };
            sSet = new StrSet(0, 3, "Тип регулиярования времени года", isVisible: true, pickerIsVisible: true, entryIsVisible: false, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(5, 30, "Темп. перехода зима/лето, °С", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 20, "Гистерезис темп. перехода,°С", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            pickVals = new List<string>() { "Откл", "Вкл" };
            sSet = new StrSet(0, 5, "Перезапуск по питанию", isVisible: true, pickerIsVisible: true, entryIsVisible: false, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 5, "Автосброс пожара", isVisible: true, pickerIsVisible: true, entryIsVisible: false, pickVals);
            strSets.Add(sSet);
            StartMenuCollection[0].StrSetsCollection = strSets;
            //Жалюзи
            strSets = new ObservableCollection<StrSet>();
            pickVals = new List<string>() { "", "" };
            sSet = new StrSet(0, 10000, "Время открытия, сек", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            StartMenuCollection[1].StrSetsCollection = strSets;
            #endregion

            #region Вентилятор
            strSets = new ObservableCollection<StrSet>();
            pickVals = new List<string>() { "Откл", "Вкл" };
            //sSet = new StrSet(0, 1, "Вентилятор вытяжки", isVisible: true, pickerIsVisible: true, entryIsVisible: false, pickVals);
            //strStes.Add(sSet);
            sSet = new StrSet(0, 65000, "Номин.расход вент. прит, м3/ч", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 65000, "Номин.расход вент. вытяжки, м3/ч", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Нижняя граница запрета, %", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Верхняя граница запрета, %", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 10000, "Задержка контроля давления, сек", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 10000, "Задержка контроля аварии вентилятора, сек", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 1, "Функция снижения оборотов", isVisible: true, pickerIsVisible: true, entryIsVisible: false, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 1000, "P коэф. регулятора", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 1000, "I коэф. регулятора", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 1000, "D коэф. регулятора", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Минимальный % вент", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            StartMenuCollection[2].StrSetsCollection = strSets;
            #endregion
            #region WHHeater
            strSets = new ObservableCollection<StrSet>();
            pickVals = new List<string>() { "Откл", "Вкл" };
            sSet = new StrSet(0, 1000, "P коэф. регулятора (работа)", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 1000, "I коэф. регулятора (работа)", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 1000, "D коэф. регулятора (работа)", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 1000, "P коэф. регулятора (огран)", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 1000, "I коэф. регулятора (огран)", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 1000, "D коэф. регулятора (огран)", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Max. темп. обратной воды, °C", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Min. темп. обратной воды, °C", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Дежурн. темп. обратной воды, °C", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Авар. темп. обратной воды, °C", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Пусковая темп. обратной воды, °C", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            // не отображаем, потом потом удалить
            sSet = new StrSet(0, 100, "Пусковая темп. обратной воды2, °C", isVisible: false, pickerIsVisible: false, entryIsVisible: false, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 10000, "Время мягкого пуска, сек", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Минимальный % КЗР", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Температура 'зимних процедур'", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 1, "Проба насоса летом", isVisible: true, pickerIsVisible: true, entryIsVisible: false, pickVals);
            strSets.Add(sSet);
            StartMenuCollection[3].StrSetsCollection = strSets;
            #endregion 
            #region EHHeater
            strSets = new ObservableCollection<StrSet>();
            pickVals = new List<string>() { "Откл", "Вкл" };
            sSet = new StrSet(0, 100_000, "Мощность нагревателя, Вт", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 1_000, "P коэф. регулятора", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 1_000, "I коэф. регулятора", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 1, "D коэф. регулятора", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 10_000, "Время продувки, сек", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            StartMenuCollection[4].StrSetsCollection = strSets;
            #endregion
            #region FreonCooler
            strSets = new ObservableCollection<StrSet>();
            pickVals = new List<string>() { "Откл", "Вкл" };
            sSet = new StrSet(0, 1000, "P коэф. регулятора", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 1000, "I коэф. регулятора", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 1000, "D коэф. регулятора", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 10000, "Задержка включения ступени, сек", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 10000, "Минимальное время работы ступени, сек", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 1000, "Гистерезис вкл/выкл фреонового охладителя", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            StartMenuCollection[5].StrSetsCollection = strSets;
            #endregion
            #region Recup
            strSets = new ObservableCollection<StrSet>();
            pickVals = new List<string>() { "Откл", "Вкл" };
            sSet = new StrSet(0, 1000, "P коэф. регулятора", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 1000, "I коэф. регулятора", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 1000, "D коэф. регулятора", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Уставка КПД, %", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Аварийный КПД, %", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 10000, "Задержка контроля КПД, мин", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Максимальная частота, Гц", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Температура А, °С", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Температура В, °С", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Температура C, °С", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 100, "Температура D, °С", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            StartMenuCollection[6].StrSetsCollection = strSets;
            #endregion
            #region Humidity 
            strSets = new ObservableCollection<StrSet>();
            pickVals = new List<string>() { "Откл", "Вкл" };
            sSet = new StrSet(0, 1000, "P коэф. регулятора", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 1000, "I коэф. регулятора", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 1000, "D коэф. регулятора", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 10000, "Задержка включения ступени, сек", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 10000, "Минимальное. время работы ступени, сек", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 1000, "Гистерезис вкл/выкл увлажнителя", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            StartMenuCollection[7].StrSetsCollection = strSets;
            #endregion
            #region Sensors
            strSets = new ObservableCollection<StrSet>();
            pickVals = new List<string>() { "Нет", "PT1000", "NTC10K" };
            sSet = new StrSet(0, 3, "Датчик уличной температуры", isVisible: true, pickerIsVisible: true, entryIsVisible: false, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 3, "Датчик температуры приточного канала", isVisible: true, pickerIsVisible: true, entryIsVisible: false, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 3, "Датчик температуры вытяжного канала", isVisible: true, pickerIsVisible: true, entryIsVisible: false, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 3, "Датчик комнатной температуры", isVisible: true, pickerIsVisible: true, entryIsVisible: false, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 3, "Датчик температуры обратной   воды", isVisible: true, pickerIsVisible: true, entryIsVisible: false, pickVals);
            strSets.Add(sSet);
            pickVals = new List<string>() { "Нет", "Есть" };
            sSet = new StrSet(0, 1, "Датчик качества воздуха", isVisible: true, pickerIsVisible: true, entryIsVisible: false, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(0, 1, "Датчик влажности", isVisible: true, pickerIsVisible: true, entryIsVisible: false, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(-100, 100, "Коррекция датчика уличной температуры", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(-100, 100, "Коррекция датчика температуры приточного канала", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(-100, 100, "Коррекция датчика вытяжного канала", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(-100, 100, "Коррекция датчика комнатной температуры", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            sSet = new StrSet(-100, 100, "Коррекция датчика температуры обратной воды", isVisible: true, pickerIsVisible: false, entryIsVisible: true, pickVals);
            strSets.Add(sSet);
            StartMenuCollection[8].StrSetsCollection = strSets;
            #endregion
        }

    }
}
