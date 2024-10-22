using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Android_Silver.Entities.FBEntities
{
    public class DamperSetPoints
    {
        public int DamperOpenTime;
        public int DamperHeatingTime;
        public List<ServoDamer> ServoDampers;
        public byte isTest;
        public DamperSetPoints()
        {
            ServoDampers=new List<ServoDamer>();
            ServoDampers.Add(new ServoDamer());
            ServoDampers.Add(new ServoDamer());
            ServoDampers.Add(new ServoDamer());
            ServoDampers.Add(new ServoDamer());
        }
    }

    public class ServoDamer
    {
        public ushort StartPos;
        public ushort EndPos;
        public ushort CAngle;
        public ushort CalPos;
    }
}
