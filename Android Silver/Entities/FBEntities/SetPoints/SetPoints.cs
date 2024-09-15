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
        public EConfig CEConfig;

        public SetPoints()
        {
            CCommonSetPoints = new CommonSetPoints();
            CEConfig = new EConfig();
        }
    }
}
