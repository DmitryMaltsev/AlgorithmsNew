using Android_Silver.ViewModels;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Android_Silver.Entities.Visual
{
    public class SettingsSets:BindableBase
    {
		private ObservableCollection<StrSet> _sensorsSets;
		public ObservableCollection<StrSet> SensorsSets
		{
			get { return _sensorsSets; }
			set { 
				_sensorsSets = value;
				OnPropertyChanged(nameof(SensorsSets));
			}
		}

        public SettingsSets()
        {
            SensorsSets=new ObservableCollection<StrSet>();

		
			StrSet strSet1 = new StrSet(0, 1);
			strSet1.PickerIsVisible = true;
			strSet1.EntryIsVisible = false;
			strSet1.Name = "Время открытия жалюзи";
			SensorsSets.Add(strSet1);

        }

    }
}
