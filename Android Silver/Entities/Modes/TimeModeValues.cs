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


        private int _dayOfWeek;

        public int DayOfWeek
        {
            get { return _dayOfWeek; }
            set
            {
                _dayOfWeek = value;
                OnPropertyChanged(nameof(DayOfWeek));
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

        public Mode1Values CMode1 { get; set; }

        public TimeModeValues()
        {
            Days = new List<string>
            {
                "",
                "Понедельник",
                "Вторник",
                "Среда",
                "Четверг",
                "Пятница",
                "Суббота",
                "Воскресенье",
                "Все"
             };
        }
    }
}
