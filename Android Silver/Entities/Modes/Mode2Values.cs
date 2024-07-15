namespace Android_Silver.Entities.Modes
{
    public class Mode2Values
    {
        public Mode1Values CMode1 { get; set; }
        public TimeModeValues CTimeMode { get; set; }
        public List<TimeModeValues> TimeModeValues { get; set; } = new List<TimeModeValues>();

        public int CNum { get; private set; }

        public string Mode2Icon { get; set; }

        public Mode2Values(int cNum, int tModesCount, string mode2Icon)
        {
            TimeModeValues = SetTModes(tModesCount);
            Mode2Icon = mode2Icon;
            cNum = CNum;
        }

        private List<TimeModeValues> SetTModes(int count)
        {
            List<TimeModeValues> tModeList = new List<TimeModeValues>();
            //0 режим всегда стандартный
            for (int i = 0; i <= count; i++)
            {
                tModeList.Add(new TimeModeValues() { TimeModeNum = i });
            }
            return tModeList;
        }
    }
}


