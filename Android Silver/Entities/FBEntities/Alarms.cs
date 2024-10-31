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
       public bool IsRecupLowTurns = false;
        private List<Alarm> _alarmsList;

        public ushort Alarms1;
       public bool Alarms1Changed = false;
        public ushort Alarms2;
       public bool Alarms2Changed = false;

        private ObservableCollection<Alarm> _alarmsCollection=new();
        public ObservableCollection<Alarm> AlarmsCollection
        {
			get { return _alarmsCollection; }
			set { 
				_alarmsCollection = value;
				OnPropertyChanged(nameof(AlarmsCollection));
			}
		}

        private PicturesSet _pictureSet;
        public bool Warning { get; set; }

        public Alarms()
        {
            
           
            _pictureSet =DIContainer.Resolve<PicturesSet>();
            _alarmsList=new List<Alarm>();

            _alarmsList.Add(new Alarm("Пожар", _pictureSet.JournalStroke, 1));
            _alarmsList.Add(new Alarm("Отказ датчика темп. притока", _pictureSet.JournalStroke, 2));
            _alarmsList.Add(new Alarm("Отказ датчика темп. наружного воздуха", _pictureSet.JournalStroke, 3));
            _alarmsList.Add(new Alarm("Отказ датчика темп. обратной воды", _pictureSet.JournalStroke, 4));
            _alarmsList.Add(new Alarm("Отказ датчика темп. помещения", _pictureSet.JournalStroke, 5));
            _alarmsList.Add(new Alarm("Низкая температура притока", _pictureSet.JournalStroke, 6));
            _alarmsList.Add(new Alarm("Низкая температура обратной воды", _pictureSet.JournalStroke, 7));
            _alarmsList.Add(new Alarm("Прессостат вент. притока 1", _pictureSet.JournalStroke, 8));
            _alarmsList.Add(new Alarm("Прессостат вент. вытяжки 1", _pictureSet.JournalStroke, 9));
            _alarmsList.Add(new Alarm("Перегруз вент. притока 1", _pictureSet.JournalStroke, 10));
            _alarmsList.Add(new Alarm("Перегруз вент. вытяжки 1", _pictureSet.JournalStroke, 11));
            _alarmsList.Add(new Alarm("Термостат водяного нагревателя", _pictureSet.JournalStroke, 12));
            _alarmsList.Add(new Alarm("Термостат электр. нагревателя", _pictureSet.JournalStroke, 13));
            _alarmsList.Add(new Alarm("Отказ датчика темп. вытяжки", _pictureSet.JournalStroke, 14));
            _alarmsList.Add(new Alarm("Низкий КПД рекуператора", _pictureSet.JournalStroke, 15));
            _alarmsList.Add(new Alarm("Отказ датчика влажности 1", _pictureSet.JournalStroke, 16));
            _alarmsList.Add(new Alarm("Отказ датчика влажности 2", _pictureSet.JournalStroke, 17));
            _alarmsList.Add(new Alarm("Отказ датчика качества воздуха", _pictureSet.JournalStroke, 18));
            _alarmsList.Add(new Alarm("Отказ термоанемометра TC1", _pictureSet.JournalStroke, 19));
            _alarmsList.Add(new Alarm("Отказ термоанемометра TH1", _pictureSet.JournalStroke, 20));
            _alarmsList.Add(new Alarm("Отказ термоанемометра TC2", _pictureSet.JournalStroke, 21));
            _alarmsList.Add(new Alarm("Отказ термоанемометра TH2", _pictureSet.JournalStroke, 22));
            _alarmsList.Add(new Alarm("Отказ привода рекуператора", _pictureSet.JournalStroke, 23));
            _alarmsList.Add(new Alarm("Низкий расход притока", _pictureSet.JournalStroke, 24));
            _alarmsList.Add(new Alarm("Низкий расход вытяжки", _pictureSet.JournalStroke, 25));
            _alarmsList.Add(new Alarm("Пустой", _pictureSet.JournalStroke, 26));
            _alarmsList.Add(new Alarm("Пустой", _pictureSet.JournalStroke, 27));
            _alarmsList.Add(new Alarm("Пустой", _pictureSet.JournalStroke, 28));
            _alarmsList.Add(new Alarm("Пустой", _pictureSet.JournalStroke, 29));
            _alarmsList.Add(new Alarm("Пустой", _pictureSet.JournalStroke, 30));
            _alarmsList.Add(new Alarm("Пустой", _pictureSet.JournalStroke, 31));
            _alarmsList.Add(new Alarm("Пустой", _pictureSet.JournalStroke, 32));
            _alarmsList.Add(new Alarm("Пустой", _pictureSet.JournalStroke, 33));
            AlarmsCollection.Add(_alarmsList[5]);
        }

        public BitArray GetAlarmsByBits(ushort alarm)
        {
            BitArray bits = new BitArray(new int[] { alarm });
            bits = new BitArray(bits.Cast<bool>().ToArray());
            return bits;
        }

        public void ConverBitArrayToAlarms(BitArray bits,int num)
        {
            IsRecupLowTurns = false;
            //Если берем из 1 битов или 2 битов
            int addVal = num == 0 ? 0 : 16;
            MainThread.BeginInvokeOnMainThread(() =>
            {
                for (int i = 0; i < bits.Count; i++)
                {
                    if (bits[i] == true)
                    {
                        AlarmsCollection.Add(_alarmsList[i+ addVal]);
                        if (i + addVal == 22)
                        {
                            IsRecupLowTurns = true;
                        }
                    }
                }
            });
        }
    }
}
