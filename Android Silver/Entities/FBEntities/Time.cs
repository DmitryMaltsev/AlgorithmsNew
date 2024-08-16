using Android_Silver.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Android_Silver.Entities.FBEntities
{
    public class Time:BindableBase
    {
        private int _year;
        public int Year
        {
            get { return _year; }
            set { 
                _year = value;
                OnPropertyChanged(nameof(Year));
            }
        }

        private int _month;

        public int Month
        {
            get { return _month; }
            set { 
                _month = value;
                OnPropertyChanged(nameof(Month));
            }
        }

        private int _day;

        public int Day
        {
            get { return _day; }
            set { 
                _day = value;
                OnPropertyChanged(nameof(Day));
            }
        }

        private int _hour;

        public int Hour
        {
            get { return _hour; }
            set {
                _hour = value;
                OnPropertyChanged(nameof(Hour));
            }
        }

        private int _minute;

        public int Minute
        {
            get { return _minute; }
            set { 
                _minute = value;
                OnPropertyChanged(nameof(Minute));
            }
        }

        private int _dayOfWeek;

        public int DayOfWeek
        {
            get { return _dayOfWeek; }
            set { 
                _dayOfWeek = value; 
                OnPropertyChanged($"{nameof(DayOfWeek)}");
            }
        }

        private string _timeInterface;

        public string TimeInterface
        {
            get { return _timeInterface; }
            set {
                _timeInterface = value;
                OnPropertyChanged(nameof(TimeInterface));
            }
        }

        public void SetTimerInterface()
        {
            string hourInterface=Hour < 10 ? $"0{Hour}":Hour.ToString();
            string minutesInterface = Minute < 10 ? $"0{Minute}":Minute.ToString();
            TimeInterface = $"{hourInterface}:{minutesInterface}";
        }
    }
}
