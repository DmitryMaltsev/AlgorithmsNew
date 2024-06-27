namespace Android_Silver.Entities.Modes
{
    public class Mode2Values
    {
        public Mode1Values CMode1 { get; set; }
        public TimeModeValues CTimeMode { get; set; }
        public List<TimeModeValues> TimeModeValues { get; set; } = new List<TimeModeValues>();

        public Mode2Values(int tModesCount)
        {
            TimeModeValues = SetTModes(tModesCount);
        }

        private List<TimeModeValues> SetTModes(int count)
        {
            List<TimeModeValues> tModeList = new List<TimeModeValues>();
            for (int i = 0; i < count; i++)
            {
                tModeList.Add(new TimeModeValues() { TimeModeNum = i });
            }
            return tModeList;
        }
    }
}


