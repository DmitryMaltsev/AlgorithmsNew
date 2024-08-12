using Android_Silver.Entities.Visual;
using Android_Silver.ViewModels;

namespace Android_Silver.Entities.Modes
{
    public class Mode2Values : BindableBase
    {

        public TimeModeValues CTimeMode { get; set; }

        private List<TimeModeValues> _timeModeValues = new();

        public List<TimeModeValues> TimeModeValues
        {
            get { return _timeModeValues; }
            set { 
                _timeModeValues = value; 
                OnPropertyChanged(nameof(TimeModeValues));
            }
        }

        private PicturesSet _pictureSet;

        public int CNum { get; private set; }

        public string Mode2Icon { get; set; }

        public int StartAddress { get; set; }

        public Mode2Values(int cNum, int tModesCount, string mode2Icon, int startAddress)
        {
            Mode2Icon = mode2Icon;
            cNum = CNum;
            StartAddress = startAddress;
            TimeModeValues = SetTModes(tModesCount);
          
        }

        private List<TimeModeValues> SetTModes(int count)
        {
            _pictureSet=DIContainer.Resolve<PicturesSet>();
            List<TimeModeValues> tModeList = new List<TimeModeValues>();
            for (int i = 0; i < count; i++)
            {
                tModeList.Add(new TimeModeValues(i, CMode1.Num, StartAddress + i*4, i+1));
                tModeList[i].SetTValues(i, cMode1.MiniIcon);
                tModeList[i].StrokeImg = new PicByStates(_pictureSet.TModeStroke.Default, _pictureSet.TModeStroke.Selected);
            }
            return tModeList;
        }
    }
}


