using Android_Silver.Entities.ValuesEntities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Android_Silver.Entities.FBEntities
{
    public class WHSetPoints
    {
        public int SSMaxIntervalS;
        public FloatValue TRetStart;
        public FloatValue TRetStb;
        public FloatValue TRetMax;
        public FloatValue TRetMin;
        public FloatValue TRetF;
        public FloatValue SPWinterProcess;
        public int MinDamperPerc;
        public int PWork;
        public int IWork;
        public int DWork;
        public int PRet;
        public int IRet;
        public int DRet;
        public int IsSummerTestPump;

        public WHSetPoints()
        {
            TRetStart = new FloatValue(0, 100, 1);
            TRetStb = new FloatValue(0, 100, 1);
            TRetMax = new FloatValue(0, 100, 1);
            TRetMin = new FloatValue(0,100,1);
            TRetF = new FloatValue(0,100,1);
            SPWinterProcess = new FloatValue(0, 100, 1);
        }
    }
}
