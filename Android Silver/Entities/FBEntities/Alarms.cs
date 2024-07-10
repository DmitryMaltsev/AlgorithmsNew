using Android_Silver.Entities.Visual;
using Android_Silver.ViewModels;
using System;
using System.Collections;
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

        public int Index { get;private set; }

        public Alarm(string name, string imageName, int index)
        {
            Name = name;    
            ImageName = imageName;
            Index = index;

        }
    }


    public class Alarms:BindableBase
    {
		private ObservableCollection<Alarm> _alarmsCollection;

        private List<Alarm> _alarmsList;


        public ushort Alarms1;
        public ushort Alarms2;


 
                   

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
            _alarmsList=new List<Alarm>();

            _alarmsList.Add(new Alarm("", "", 0));
        }

        public BitArray GetAlarmsByBits(ushort alarm)
        {
            BitArray bits = new BitArray(new int[] { alarm });
            bits = new BitArray(bits.Cast<bool>().ToArray());
            return bits;
        }

        private void ConverBoolToAlarms(BitArray bits)
        {
            for (int i = 0; i < bits.Count; i++)
            {
                foreach (Alarm alarm in _alarmsCollection)
                {

                }
            }
        }
    }
}
