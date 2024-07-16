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


        public int CMode1Num { get; set; }

        public TimeModeValues(int index, int cMode1Num)
        {
            TimeModeNum= index+1;
            Days = new List<string>
            {
                "Нет",
                "1",
                "2",
                "3",
                "4",
                "5",
                "6",
                "7",
                "Все"
             };
            DayName = Days[0];
            CMode1Num = cMode1Num;
          

        }
    }
}
