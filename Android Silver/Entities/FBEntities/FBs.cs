using Android_Silver.Entities.Srs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Android_Silver.Entities.FBEntities
{
    public class FBs
    { 
        public Alarms CAlarms { get; set; }
        public HumiditySPS CHumiditySPS {get;set;}
        public FilterVals CFilterVals { get; set; }
        public OtherSettings OtherSettings { get; set; }
        public Time CTime { get; set; }
        public Fans CFans { get; set; }
        public Sensors CSensors { get; set; }
        public Recup CRecup { get; set; }
        public ThermoanemometersSPS ThmSps { get; set; }

        public MBRecupSetPoints MbRecSPs { get; set; }

        public CommonSetPoints CCommonSetPoints;
        public DamperSetPoints CDamperSetPoints;
        public ElementsConfig CEConfig;
        public WHSetPoints CWHSetPoints;
        public EHSetPoints CEHSetPoints;
        public FreonCoolerSP CFreonCoolerSP;

        public FBs()
        {
            CAlarms = new Alarms();
            CHumiditySPS = new HumiditySPS();
            CFilterVals =new FilterVals();
            OtherSettings = new OtherSettings();
            CTime = new Time();
            CRecup = new Recup();
            CSensors = new Sensors();
            CFans = new Fans();
            CCommonSetPoints = new CommonSetPoints();
            CDamperSetPoints = new DamperSetPoints();
            CEConfig = new ElementsConfig();
            CWHSetPoints = new WHSetPoints();
            CEHSetPoints = new EHSetPoints();
            CFreonCoolerSP = new FreonCoolerSP();
            ThmSps = new ThermoanemometersSPS();
            MbRecSPs=new MBRecupSetPoints();
        }
    }
}
