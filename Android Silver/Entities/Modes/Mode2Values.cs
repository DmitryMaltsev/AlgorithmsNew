using Android_Silver.Entities.Visual;

namespace Android_Silver.Entities.Modes
{
    public class Mode2Values
    {
        public Mode1Values CMode1 { get; set; }
        public TimeModeValues CTimeMode { get; set; }
        public List<TimeModeValues> TimeModeValues { get; set; } = new List<TimeModeValues>();

        private PicturesSet _pictureSet;

        public int CNum { get; private set; }

        public string Mode2Icon { get; set; }

        public int StartAddress { get; set; }

        public Mode2Values(int cNum, int tModesCount, string mode2Icon, Mode1Values cMode1, int startAddress)
        {
            Mode2Icon = mode2Icon;
            cNum = CNum;
            CMode1 = cMode1;
            TimeModeValues = SetTModes(tModesCount, CMode1);
            StartAddress = startAddress;
        }

        private List<TimeModeValues> SetTModes(int count, Mode1Values cMode1)
        {
            _pictureSet=DIContainer.Resolve<PicturesSet>();
            List<TimeModeValues> tModeList = new List<TimeModeValues>();
            CMode1 = cMode1;
            for (int i = 0; i < count; i++)
            {
                tModeList.Add(new TimeModeValues(i, CMode1.Num, StartAddress + i*4));
                tModeList[i].SetTValues(i, cMode1.MiniIcon);
                tModeList[i].StrokeImg = new PicByStates(_pictureSet.TModeStroke.Default, _pictureSet.TModeStroke.Selected);
            }
            return tModeList;
        }
    }
}


