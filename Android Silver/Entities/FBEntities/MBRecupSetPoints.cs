﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Android_Silver.Entities.FBEntities
{
    public class MBRecupSetPoints
    {
        public ushort MBRecMode;
        public byte IsRotTest1;
        public byte IsRotTest2;
        public byte IsForward1;
        public byte IsForward2;
        public byte IsGrindingMode;
        public ushort NominalCurrent;
        public float ReductKoef;
        public ushort NominalTurns1;
        public ushort NominalTurns2;
        public float NominalTemp1;
        public float NominalTemp2;
        public ushort GrindingCurrent;
        public ushort GrindingTurns;
    }
}
