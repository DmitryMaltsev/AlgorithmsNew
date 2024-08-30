using Android_Silver.Entities.Visual;
using Android_Silver.ViewModels;

using System.Collections.ObjectModel;

namespace Android_Silver.Entities.Modes
{
    public class Mode2Values : BindableBase
    {

        public TimeModeValues CTimeMode { get; set; }

        private ObservableCollection<TimeModeValues> _timeModeValues = new();

        public ObservableCollection<TimeModeValues> TimeModeValues
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

        public Mode2Values(int cNum, int tModesCount, string mode2Icon, Mode1Values defaultMode1, int startAddress)
        {
            Mode2Icon = mode2Icon;
            CNum=cNum;
            StartAddress = startAddress;
            TimeModeValues = SetTModes(tModesCount, defaultMode1);
        }

        private ObservableCollection<TimeModeValues> SetTModes(int count, Mode1Values defaultM1)
        {
            _pictureSet=DIContainer.Resolve<PicturesSet>();
            ObservableCollection<TimeModeValues> tModeList = new ObservableCollection<TimeModeValues>();
            for (int i = 0; i < count; i++)
            {
                tModeList.Add(new TimeModeValues(i, defaultM1, StartAddress + i*4, i+1, CNum));
                tModeList[i].StrokeImg = new PicByStates(_pictureSet.TModeStroke.Default, _pictureSet.TModeStroke.Selected);
            }
            return tModeList;
        }
    }
}


