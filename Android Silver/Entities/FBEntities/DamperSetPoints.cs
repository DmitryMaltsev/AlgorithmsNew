using Android_Silver.ViewModels;

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

    public class ServoDamer:BindableBase
    {
        public ushort StartPos;
        public ushort EndPos;
        public ushort CAngle;
        private ushort _calPos;

        public ushort CalPos
        {
            get { return _calPos; }
            set { 
                _calPos = value; 
                OnPropertyChanged(nameof(CalPos));
            }
        }


    }
}
