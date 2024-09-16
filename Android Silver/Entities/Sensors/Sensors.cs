using Android_Silver.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Android_Silver.Entities.Srs
{
    public class Sensors
    {
        public TempSenor OutdoorTemp {  get; set; }
        public TempSenor SupTemp { get; set; }
        public TempSenor ExhaustTemp { get; set; }
        public TempSenor RoomTemp { get; set; }
        public TempSenor ReturnTemp { get; set; }
        Sensor HumiditySensor { get; set; }
        Sensor AirQualitySensor { get; set; }

        public Sensors()
        {
            OutdoorTemp = new TempSenor();
            SupTemp = new TempSenor();  
            ExhaustTemp = new TempSenor();
            RoomTemp = new TempSenor();
            ReturnTemp = new TempSenor();
            HumiditySensor = new Sensor();   
            AirQualitySensor = new Sensor();
        }

    }
}
