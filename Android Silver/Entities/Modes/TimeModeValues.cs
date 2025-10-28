using Android_Silver.Entities.Visual;
using Android_Silver.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Android_Silver.Entities.Modes
{
    public class TimeModeValues : BindableBase
    {

        private int _timeModeNum;
        public int TimeModeNum
        {
            get { return _timeModeNum; }
            set
            {
                _timeModeNum = value;
                OnPropertyChanged(nameof(TimeModeNum));
            }
        }

        private int _CodeNum;

        public int TCodeNum
        {
            get { return _CodeNum; } 
            set {
                _CodeNum = value;
                OnPropertyChanged(nameof(TCodeNum));
            }
        }

        private int _mode2Num;

        public int Mode2Num
        {
            get { return _mode2Num; }
           private set { _mode2Num = value; }
        }


        private List<string> _days;       
        public List<string> Days
        {
            get { return _days; }
            set
            {
                _days = value;
            }
        }


        private int _dayNum;
        public int DayNum
        {
            get { return _dayNum; }
            set
            {
                if (_dayNum != value)
                {

                    _dayNum = value;
                    DayName = Days[_dayNum];
                }
            }
        }

        private string _dayName;

        public string DayName
        {
            get { return _dayName; }
            set { 
                _dayName = value;
                OnPropertyChanged(nameof(DayName));
            }
        }


        private int _hour;
        public int Hour
        {
            get { return _hour; }
            set
            {
                _hour = value;
                OnPropertyChanged(nameof(Hour));
            }
        }

        private int _minute;

        public int Minute
        {
            get { return _minute; }
            set
            {
                _minute = value;
                OnPropertyChanged($"{nameof(Minute)}");
            }
        }


        private PicByStates _strokeImg;

        public PicByStates StrokeImg
        {
            get { return _strokeImg; }
            set { 
                _strokeImg = value; 
                OnPropertyChanged(nameof(StrokeImg));
            }
        }

        private Mode1Values _cMode1;

        public Mode1Values CMode1
        {
            get { return _cMode1; }
            set { 
                _cMode1 = value;
                OnPropertyChanged(nameof(CMode1));
            }
        }

        public int WriteAddress { get;private set; }

        public TimeModeValues(int timeIndex, Mode1Values mode1, int startAddress, int tModeNum, int mode2Num)
        {
            TimeModeNum = tModeNum;
            Days = new List<string>
            {
                "Нет",
                "Пн",
                "Вт",
                "Ср",
                "Чт",
                "Пт",
                "Сб",
                "Вс",
                "Все"
             };
            DayName = Days[0];
            CMode1 = mode1;  
            WriteAddress = startAddress;
            TCodeNum = mode2Num * 100 + TimeModeNum;
            Mode2Num= mode2Num;
        }
    }
}
