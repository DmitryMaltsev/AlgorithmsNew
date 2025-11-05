using Android_Silver.Entities.Srs;
using Android_Silver.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Android_Silver.Entities.FBEntities
{
    public class FBs : BindableBase
    {
        public Alarms CAlarms { get; set; }
        public HumiditySPS CHumiditySPS { get; set; }
        public FilterVals CFilterVals { get; set; }
        public OtherSettings OtherSettings { get; set; }
        public Time CTime { get; set; }
        public Fans CFans { get; set; }
        public Sensors CSensors { get; set; }
        public Recup CRecup { get; set; }
        public ThermoanemometersSPS ThmSps { get; set; }
        public SPCalibrateThm SupCalibrateThm { get; set; }

        public SPCalibrateThm ExhaustCalibrateThm { get; set; }

        public MBRecupSetPoints MbRecSPs { get; set; }
        public CommonSetPoints CCommonSetPoints { get; set; }
        public DamperSetPoints CDamperSetPoints;
        public ElementsConfig CEConfig;
        public WHSetPoints CWHSetPoints;
        public EHSetPoints CEHSetPoints { get; set; }
        public FreonCoolerSP CFreonCoolerSP;
        public UFLeds UFLeds { get; set; }

        public Updater CUpdater {get; set;}

        public FBs()
        {
            CAlarms = new Alarms();
            CHumiditySPS = new HumiditySPS();
            CFilterVals = new FilterVals();
            OtherSettings = new OtherSettings();
            CTime = new Time();
            CRecup = new Recup();
            CRecup.RecProfiles = new List<RecPrfofile>
            {
                new RecPrfofile(0, 2, 1, 4, 0.03f),
                new RecPrfofile(1, 2, 1, 4, 0.03f),
                new RecPrfofile(2, 2, 1, 4, 0.03f),
                new RecPrfofile(3, 2, 1, 4, 0.03f),
                new RecPrfofile(4, 2, 1, 4, 0.03f),
                new RecPrfofile(5, 2, 1, 4, 0.03f),
                new RecPrfofile(6, 2, 1.2f, 4, 0.03f),
                new RecPrfofile(7, 2, 1.3f, 4, 0.03f),
                new RecPrfofile(8, 2, 1.4f, 4, 0.03f),
                new RecPrfofile(9, 2, 1.6f, 4, 0.03f),
            };


            CSensors = new Sensors();
            CFans = new Fans();
            CCommonSetPoints = new CommonSetPoints();
            CDamperSetPoints = new DamperSetPoints();
            CEConfig = new ElementsConfig();
            CWHSetPoints = new WHSetPoints();
            CEHSetPoints = new EHSetPoints();
            CFreonCoolerSP = new FreonCoolerSP();
            ThmSps = new ThermoanemometersSPS();
            MbRecSPs = new MBRecupSetPoints();
            UFLeds = new UFLeds();
            SupCalibrateThm = new SPCalibrateThm();
            ExhaustCalibrateThm = new SPCalibrateThm();
            CUpdater = new Updater();
        }
    }
}
