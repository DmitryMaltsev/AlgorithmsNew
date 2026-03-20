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
            OutdoorTemp = new TempSenor(-150,150,1);
            SupTemp = new TempSenor(-150,150,1);  
            ExhaustTemp = new TempSenor(-150, 150, 1);
            RoomTemp = new TempSenor(-150, 150, 1);
            ReturnTemp = new TempSenor(-150, 150, 1);
            HumiditySensor = new Sensor(0,100,1);   
            AirQualitySensor = new Sensor(0,100,1);
        }

    }
}
