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

        public SetPoints()
        {
            CCommonSetPoints = new CommonSetPoints();
            CDamperSetPoints=new DamperSetPoints();
            CEConfig = new EConfig();
        }
    }
}
