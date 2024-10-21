using Android_Silver.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Android_Silver.Entities.Srs
{
    public class TempSenor:Sensor
    {
        //0-нет, 1-NTC10k, 2- pt1000
        public int SensorType;
        public float Correction;

    }
}
