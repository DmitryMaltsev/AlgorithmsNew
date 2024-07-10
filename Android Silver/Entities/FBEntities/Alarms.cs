using Android_Silver.Entities.Visual;
using Android_Silver.ViewModels;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Android_Silver.Entities.FBEntities
{

    public class Alarm:BindableBase
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { 
                _name = value; 
                OnPropertyChanged(nameof(Name));
            }
        }

        private string _imageName;

        public string ImageName
        {
            get { return _imageName; }
            set { 
                _imageName = value;
                OnPropertyChanged(nameof(_imageName));            
            }
        }

        public Alarm(string name, string imageName)
        {
                Name = name;    
            ImageName = imageName;
        }


    }


    public class Alarms:BindableBase
    {
		private ObservableCollection<Alarm> _alarmsCollection;

		public ObservableCollection<Alarm> AlarmsCollection
        {
			get { return _alarmsCollection; }
			set { 
				_alarmsCollection = value;
				OnPropertyChanged(nameof(Alarms));
			}
		}
        private PicturesSet _pictureSet;
        public Alarms()
        {
            _pictureSet=DIContainer.Resolve<PicturesSet>();
            AlarmsCollection = new ObservableCollection<Alarm>();
            AlarmsCollection.Add(new Alarm("1. Пожар",_pictureSet.JournalStroke));
            AlarmsCollection.Add(new Alarm("2. Авария 1", _pictureSet.JournalStroke));
            AlarmsCollection.Add(new Alarm("3. Авария 2", _pictureSet.JournalStroke));
            AlarmsCollection.Add(new Alarm("4. Авария 3", _pictureSet.JournalStroke));
            AlarmsCollection.Add(new Alarm("5. Авария 4", _pictureSet.JournalStroke));
            AlarmsCollection.Add(new Alarm("6. Авария 5", _pictureSet.JournalStroke));
            AlarmsCollection.Add(new Alarm("7. Авария 5", _pictureSet.JournalStroke));
            AlarmsCollection.Add(new Alarm("8. Авария 5", _pictureSet.JournalStroke));
            AlarmsCollection.Add(new Alarm("9. Авария 5", _pictureSet.JournalStroke));
            AlarmsCollection.Add(new Alarm("10. Авария 5", _pictureSet.JournalStroke));
            AlarmsCollection.Add(new Alarm("11. Авария 5", _pictureSet.JournalStroke));
            AlarmsCollection.Add(new Alarm("7. Авария 5", _pictureSet.JournalStroke));
            AlarmsCollection.Add(new Alarm("8. Авария 5", _pictureSet.JournalStroke));
            AlarmsCollection.Add(new Alarm("9. Авария 5", _pictureSet.JournalStroke));
            AlarmsCollection.Add(new Alarm("10. Авария 5", _pictureSet.JournalStroke));
            AlarmsCollection.Add(new Alarm("11. Авария 5", _pictureSet.JournalStroke));
        }


    }
}
