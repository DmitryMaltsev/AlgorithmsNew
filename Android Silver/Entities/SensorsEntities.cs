using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Android_Silver.Entities
{
    public class SensorsEntities : INotifyPropertyChanged
    {
        private float _outDoorTemp;
        public float OutdoorTemp
        {
            get
            {
                return _outDoorTemp;
            }
            set
            {
                _outDoorTemp = value;
                OnPropertyChanged(nameof(OutdoorTemp));
            }
        }

        private float _supplyTemp;

        public float SupplyTemp
        {
            get { return _supplyTemp; }
            set
            {
                _supplyTemp = value;
                OnPropertyChanged(nameof(SupplyTemp));
            }
        }

        private float _exhaustTemp;

        public float ExhaustTemp
        {
            get { return _exhaustTemp; }
            set
            {
                _exhaustTemp = value;
                OnPropertyChanged(nameof(ExhaustTemp));
            }
        }

        private float _roomTemp;

        public float RoomTemp
        {
            get { return _roomTemp; }
            set
            {
                _roomTemp = value;
                OnPropertyChanged(nameof(RoomTemp));
            }
        }

        private float _returnWaterTemp;

        public float ReturnWaterTemp
        {
            get { return _returnWaterTemp; }
            set
            {
                _returnWaterTemp = value;
                OnPropertyChanged(nameof(ReturnWaterTemp));
            }
        }






        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
