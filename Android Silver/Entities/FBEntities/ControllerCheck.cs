using Android_Silver.Entities.ValuesEntities;
using Android_Silver.ViewModels;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Android_Silver.Entities.FBEntities
{
    public class ControllerCheck : BindableBase
    {

        public List<bool> ServosOverridesList = new List<bool>();

        private IntValue _servo1Pos  ;
        public IntValue Servo1Pos
        {
            get { return _servo1Pos; }
            set
            {
                _servo1Pos = value;
                OnPropertyChanged(nameof(Servo1Pos));
            }
        }
        private IntValue _servo2Pos;
        public IntValue Servo2Pos
        {
            get { return _servo2Pos; }
            set
            {
                _servo2Pos = value;
                OnPropertyChanged(nameof(Servo2Pos));
            }
        }
        private IntValue _servo3Pos;
        public IntValue Servo3Pos
        {
            get { return _servo3Pos; }
            set
            {
                _servo3Pos = value;
                OnPropertyChanged(nameof(Servo3Pos));
            }
        }
        private IntValue _servo4Pos;
        public IntValue Servo4Pos
        {
            get { return _servo4Pos; }
            set
            {
                _servo4Pos = value;
                OnPropertyChanged(nameof(Servo4Pos));
            }
        }

        public ushort OverrideIsActive1;
        public ushort OverrideIsActive2;

        public ControllerCheck()
        {
            ServosOverridesList.Add(false);
            ServosOverridesList.Add(false);
            ServosOverridesList.Add(false);
            ServosOverridesList.Add(false);
            ServosOverridesList.Add(false);
            ServosOverridesList.Add(false);
            ServosOverridesList.Add(false);
            ServosOverridesList.Add(false);
            ServosOverridesList.Add(false);
            ServosOverridesList.Add(false);
            ServosOverridesList.Add(false);
            ServosOverridesList.Add(false);
            ServosOverridesList.Add(false);
            ServosOverridesList.Add(false);
            ServosOverridesList.Add(false);
            ServosOverridesList.Add(false);
            ServosOverridesList.Add(false);
            ServosOverridesList.Add(false);
            ServosOverridesList.Add(false);
            ServosOverridesList.Add(false);
            ServosOverridesList.Add(false);
            ServosOverridesList.Add(false);
            ServosOverridesList.Add(false);
            ServosOverridesList.Add(false);
            ServosOverridesList.Add(false);
            ServosOverridesList.Add(false);
            ServosOverridesList.Add(false);
            ServosOverridesList.Add(false);
            ServosOverridesList.Add(false);
            ServosOverridesList.Add(false);
            ServosOverridesList.Add(false);
            ServosOverridesList.Add(false);
            ServosOverridesList.Add(false);
            Servo1Pos = new IntValue(0, 100);
            Servo2Pos = new IntValue(0, 100);
            Servo3Pos = new IntValue(0, 100);
            Servo4Pos = new IntValue(0, 100);
        }

        public void GetOverrides()
        {
            BitArray bitArray = new BitArray(BitConverter.GetBytes(OverrideIsActive1));
            ServosOverridesList[0] = bitArray[0];
            ServosOverridesList[1] = bitArray[1];
            ServosOverridesList[2] = bitArray[2];
            ServosOverridesList[3] = bitArray[3];
            ServosOverridesList[4] = bitArray[4];
            ServosOverridesList[5] = bitArray[5];
            ServosOverridesList[6] = bitArray[6];
            ServosOverridesList[7] = bitArray[7];
            ServosOverridesList[8] = bitArray[8];
            ServosOverridesList[9] = bitArray[9];
            ServosOverridesList[10] = bitArray[10];
            ServosOverridesList[11] = bitArray[11];
            ServosOverridesList[12] = bitArray[12];
            ServosOverridesList[13] = bitArray[13];
            ServosOverridesList[14] = bitArray[14];
            ServosOverridesList[15] = bitArray[15];
            bitArray = new BitArray(BitConverter.GetBytes(OverrideIsActive2));
            ServosOverridesList[16] = bitArray[0];
            ServosOverridesList[17] = bitArray[1];
            ServosOverridesList[18] = bitArray[2];
            ServosOverridesList[19] = bitArray[3];
            ServosOverridesList[20] = bitArray[4];
            ServosOverridesList[21] = bitArray[5];
            ServosOverridesList[22] = bitArray[6];
            ServosOverridesList[23] = bitArray[7];
            ServosOverridesList[24] = bitArray[8];
            ServosOverridesList[25] = bitArray[9];
            ServosOverridesList[26] = bitArray[10];
            ServosOverridesList[27] = bitArray[11];
            ServosOverridesList[28] = bitArray[12];
            ServosOverridesList[29] = bitArray[13];
            ServosOverridesList[30] = bitArray[14];
            ServosOverridesList[31] = bitArray[15];
        }

        public void SetOverrides()
        {
            OverrideIsActive1 = 0;
            if (ServosOverridesList[0])
                OverrideIsActive1 |= 1;
            if (ServosOverridesList[1])
                OverrideIsActive1 |= 1 << 1;
            if (ServosOverridesList[2])
                OverrideIsActive1 |= 1 << 2;
            if (ServosOverridesList[3])
                OverrideIsActive1 |= 1 << 3;
            if (ServosOverridesList[4])
                OverrideIsActive1 |= 1 << 4;
            if (ServosOverridesList[5])
                OverrideIsActive1 |= 1 << 5;
            if (ServosOverridesList[6])
                OverrideIsActive1 |= 1 << 6;
            if (ServosOverridesList[7])
                OverrideIsActive1 |= 1 << 7;
            if (ServosOverridesList[8])
                OverrideIsActive1 |= 1 << 8;
            if (ServosOverridesList[9])
                OverrideIsActive1 |= 1 << 9;
            if (ServosOverridesList[10])
                OverrideIsActive1 |= 1 << 10;
            if (ServosOverridesList[11])
                OverrideIsActive1 |= 1 << 11;
            if (ServosOverridesList[12])
                OverrideIsActive1 |= 1 << 12;
            if (ServosOverridesList[13])
                OverrideIsActive1 |= 1 << 13;
            if (ServosOverridesList[14])
                OverrideIsActive1 |= 1 << 14;
            if (ServosOverridesList[15])
                OverrideIsActive1 |= 1 << 15;

            if (ServosOverridesList[16])
                OverrideIsActive2 |= 1;
            if (ServosOverridesList[17])
                OverrideIsActive2 |= 1 << 1;
            if (ServosOverridesList[18])
                OverrideIsActive2 |= 1 << 2;
            if (ServosOverridesList[19])
                OverrideIsActive2 |= 1 << 3;
            if (ServosOverridesList[20])
                OverrideIsActive2 |= 1 << 4;
            if (ServosOverridesList[21])
                OverrideIsActive2 |= 1 << 5;
            if (ServosOverridesList[22])
                OverrideIsActive2 |= 1 << 6;
            if (ServosOverridesList[23])
                OverrideIsActive2 |= 1 << 7;
            if (ServosOverridesList[24])
                OverrideIsActive2 |= 1 << 8;
            if (ServosOverridesList[25])
                OverrideIsActive2 |= 1 << 9;
            if (ServosOverridesList[26])
                OverrideIsActive2 |= 1 << 10;
            if (ServosOverridesList[27])
                OverrideIsActive2 |= 1 << 11;
            if (ServosOverridesList[28])
                OverrideIsActive2 |= 1 << 12;
            if (ServosOverridesList[29])
                OverrideIsActive2 |= 1 << 13;
            if (ServosOverridesList[30])
                OverrideIsActive2 |= 1 << 14;
            if (ServosOverridesList[31])
                OverrideIsActive2 |= 1 << 15;
        }


    }
}
