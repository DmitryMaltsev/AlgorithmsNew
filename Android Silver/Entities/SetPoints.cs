using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Android_Silver.Entities
{
    public class SetPoints : INotifyPropertyChanged
    {
        private int _setPoint1;
        public int SetPoint1
        {
            get { return _setPoint1; }
            set
            {
                _setPoint1 = value;
                OnPropertyChanged(nameof(SetPoint1));
            }
        }

        private int _setPoint2;
        public int SetPoint2
        {
            get { return _setPoint2; }
            set
            {
                _setPoint2 = value;
                OnPropertyChanged(nameof(SetPoint2));
            }
        }

        private int _setPoint3;
        public int SetPoint3
        {
            get { return _setPoint3; }
            set
            {
                _setPoint3 = value;
                OnPropertyChanged(nameof(SetPoint3));
            }
        }

        private float _setPointF;
        public float SetPointF
        {
            get { return _setPointF; }
            set
            {
                _setPointF = value;
                OnPropertyChanged(nameof(SetPointF));
            }
        }


        private int _sp1Count;

        public int SP1Count
        {
            get { return _sp1Count; }
            set
            {
                _sp1Count = value;
                OnPropertyChanged(nameof(SP1Count));
            }
        }

        private int _sp2Count;

        public int SP2Count
        {
            get { return _sp2Count; }
            set
            {
                _sp2Count = value;
                OnPropertyChanged(nameof(SP2Count));
            }
        }

        private int _sp3Count;

        public int SP3Count
        {
            get { return _sp3Count; }
            set
            {
                _sp3Count = value;
                OnPropertyChanged(nameof(SP3Count));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

    }
}
