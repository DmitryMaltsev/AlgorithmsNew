using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Android_Silver.Entities.FBEntities.SetPoints
{
    public class SetPoints
    {
        public CommonSetPoints CCommonSetPoints;
        public DamperSetPoints CDamperSetPoints;
        public EConfig CEConfig;
        public FanSetPoints CFansSetPoints;
        public WHSetPoints WHSetPoints;

        public SetPoints()
        {
            CCommonSetPoints = new CommonSetPoints();
            CDamperSetPoints=new DamperSetPoints();
            CEConfig = new EConfig();
            CFansSetPoints=new FanSetPoints();
            WHSetPoints=new WHSetPoints();
        }
    }
}
