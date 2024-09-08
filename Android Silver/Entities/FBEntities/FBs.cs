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

        public Temps CTemps { get; set; }
        public Recup CRecup { get; set; }
        public FBs()
        {
            CAlarms=DIContainer.Resolve<Alarms>();
            CHumiditySPS=DIContainer.Resolve<HumiditySPS>();
            CFilterVals=DIContainer.Resolve<FilterVals>();
            OtherSettings = DIContainer.Resolve<OtherSettings>();
            CTime=DIContainer.Resolve<Time>();
            CRecup=DIContainer.Resolve<Recup>();
            CTemps=DIContainer.Resolve<Temps>();
            CFans=DIContainer.Resolve<Fans>();
        }
    }
}
