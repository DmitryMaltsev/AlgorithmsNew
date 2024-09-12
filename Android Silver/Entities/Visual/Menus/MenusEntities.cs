using Android_Silver.ViewModels;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Unity;
using Unity.Resolution;

namespace Android_Silver.Entities.Visual.Menus
{
    public class MenusEntities : BindableBase
    {

        private ObservableCollection<MItem> _menusBufferCollection;

        public ObservableCollection<MItem> MenusCollection
        {
            get { return _menusBufferCollection; }
            set
            {
                _menusBufferCollection = value;
                OnPropertyChanged(nameof(MenusCollection));
            }
        }

        private PicturesSet _pictureSet { get; set; }


        public MenusEntities()
        {
            _pictureSet = DIContainer.Resolve<PicturesSet>();

            MenusCollection = new ObservableCollection<MItem>();
            MItem mItem = new MItem("Общие", true, _pictureSet.BaseSettings1ButCollection[0], SActivePageState.CommonSettingsPage, 1);
            MenusCollection.Add(mItem);
            mItem = new MItem("Жалюзи", true, _pictureSet.BaseSettings1ButCollection[1], SActivePageState.DamperSettingsPage, 2);
            MenusCollection.Add(mItem);
            mItem = new MItem("Вентилятор", true, _pictureSet.BaseSettings1ButCollection[2], SActivePageState.FanSettingsPage, 3);
            MenusCollection.Add(mItem);
            mItem = new MItem("Нагреватель водяной", true, _pictureSet.BaseSettings1ButCollection[3], SActivePageState.WHSettingsPage, 4);
            MenusCollection.Add(mItem);
            mItem = new MItem("Нагреватель электрический", true, _pictureSet.BaseSettings1ButCollection[4], SActivePageState.EHSettingsPage, 5);
            MenusCollection.Add(mItem);

            //Общие
            ObservableCollection<StrSet> strStes = new ObservableCollection<StrSet>();
            List<string> pickVals = new List<string>(){ "Улица", "комната" };
            StrSet sSet = new StrSet(0, 50, "Уставка аварийной темп, °C", true, true, true,pickVals);
            strStes.Add(sSet);
            sSet = new StrSet(0, 1, "Регулирование темп. по", true, true, false, pickVals);
            strStes.Add(sSet);
            sSet = new StrSet(0, 50, "Уставка темп.кан.макс.,°C", true, false, true, pickVals);
            strStes.Add(sSet);
            sSet = new StrSet(0, 50, "Уставка темп. кан. мин.,°C", true, false, true, pickVals);
            strStes.Add(sSet);
            sSet = new StrSet(0, 10, "Задержка аварии по темп, сек", true, false, true, pickVals);
            strStes.Add(sSet);
            sSet = new StrSet(0, 40, "Уставка влажности, %", true, false, true, pickVals);
            strStes.Add(sSet);
            sSet = new StrSet(0, 3, "Время года", true, false, true, pickVals);
            strStes.Add(sSet);
            sSet = new StrSet(5, 30, "Темп. перехода зима/лето, °С", true, false, true, pickVals);
            strStes.Add(sSet);
            sSet = new StrSet(0, 3, "Гистерезис темп. перехода,°С", true, false, true, pickVals);
            strStes.Add(sSet);
            sSet = new StrSet(0, 3, "Перезапуск по питанию", true, false, true, pickVals);
            strStes.Add(sSet);
            sSet = new StrSet(0, 3, "Автосброс пожара", true, false, true, pickVals);
            strStes.Add(sSet);

            MenusCollection[0].StrSetsCollection = strStes;
        }

    }
}
