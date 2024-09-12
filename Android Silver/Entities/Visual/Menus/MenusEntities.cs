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
        private ObservableCollection<MenuItem> _menusCollection;

        public ObservableCollection<MenuItem> MenusCollection
        {
            get { return _menusCollection; }
            set
            {
                _menusCollection = value;
                OnPropertyChanged(nameof(MenusCollection));
            }
        }

        private PicturesSet _pictureSet { get; set; }


        public MenusEntities()
        {
            _pictureSet=DIContainer.Resolve<PicturesSet>();
            MenusCollection = new ObservableCollection<MenuItem>();
            MenusCollection.Add(new MenuItem("Общие",true, _pictureSet.BaseSettings1But, SActivePageState.CommonSettingsPage));
            MenusCollection.Add(new MenuItem("Жалюзи", true, _pictureSet.BaseSettings1But, SActivePageState.DamperSettingsPage));
            MenusCollection.Add(new MenuItem("Вентилятор", true, _pictureSet.BaseSettings1But, SActivePageState.FanSettingsPage));
            MenusCollection.Add(new MenuItem("Нагреватель водяной", true, _pictureSet.BaseSettings1But, SActivePageState.WHSettingsPage));
            MenusCollection.Add(new MenuItem("Нагреватель электрический", true, _pictureSet.BaseSettings1But, SActivePageState.EHSettingsPage));
        }



    }
}
