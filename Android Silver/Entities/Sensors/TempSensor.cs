using Android_Silver.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Android_Silver.Entities.Sensors
{
    class TempSensor:BindableBase
    {
        public TempSensor OutdoorTemp { get; set; }
        public TempSensor SupplyTemp { get; set; }
        public TempSensor ExhaustTemp { get; set; }
        public TempSensor RoomTemp { get; set; }
        public TempSensor ReturnWaterTemp { get; set; }

    }
}
