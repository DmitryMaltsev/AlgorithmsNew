using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows.Threading;

using OxyPlot;
using OxyPlot.Wpf;

namespace AlgorithmsNew
{
    public class ViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<PointF> _pPoints;
        public ObservableCollection<PointF> PPoints
        {
            get { return _pPoints; }
            set
            {
                _pPoints = value;
                OnPropertyChanged(nameof(PPoints));
            }
        }


        private ObservableCollection<PointF> _iPoints;
        public ObservableCollection<PointF> IPoints
        {
            get { return _iPoints; }
            set
            {
                _iPoints = value;
                OnPropertyChanged(nameof(IPoints));
            }
        }

        private ObservableCollection<PointF> _dPoints;
        public ObservableCollection<PointF> DPoints
        {
            get { return _dPoints; }
            set
            {
                _dPoints = value;
                OnPropertyChanged(nameof(DPoints));
            }
        }


        private ObservableCollection<PointF> _rPoints;
        public ObservableCollection<PointF> RPoints
        {
            get { return _rPoints; }
            set
            {
                _rPoints = value;
                OnPropertyChanged(nameof(RPoints));
            }
        }

        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }




        private DispatcherTimer _myTimer;
        int Counter = 0;
        public ViewModel()
        {
            PPoints = new ObservableCollection<PointF>();
            IPoints = new ObservableCollection<PointF>();
            DPoints = new ObservableCollection<PointF>();
            RPoints = new ObservableCollection<PointF>();
            _myTimer = new DispatcherTimer();
            _myTimer.Interval = TimeSpan.FromSeconds(1);
            _myTimer.Tick += _myTimer_Tick;
            _myTimer.Start();
        }

        private void _myTimer_Tick(object? sender, EventArgs e)
        {
            float result = PIDRegulator(1, 20, 1, 25, 30, false);
            PointF sensorPoint1 = new PointF(Counter, _errorCurrent);
            PointF sensorPoint2 = new PointF(Counter, _errorIntegral);
            PointF sensorPoint3 = new PointF(Counter, _errorDif);
            PointF sensorPoint4 = new PointF(Counter, result);
            PPoints.Add(sensorPoint1);
            IPoints.Add(sensorPoint2);
            DPoints.Add(sensorPoint3);
            RPoints.Add(sensorPoint4);
            Counter += 1;
        }

        float _errorPrevious = 0;
        float _errorCurrent = 0;
        float _errorIntegral = 0;
        float _errorDif = 0;
        float _lastTimeMS = 0;

        private float PIDRegulator(float Kp, float Ki, float Kd, float currentTemp, float targetTemp, bool isUp)
        {
            int minVal = 0;
            int maxVal = 1000;
            float result = 0;
            if (_lastTimeMS != 0)
            {
                float currentTimeS = (Counter - _lastTimeMS);
                _errorCurrent = isUp ? targetTemp - currentTemp : currentTemp - targetTemp;
                if (((Ki * _errorIntegral < maxVal) && (_errorCurrent >= 0)) ||
                    ((Ki * _errorIntegral >= minVal) && (_errorCurrent < 0)))
                {
                    _errorIntegral += _errorCurrent * currentTimeS;
                }
                _errorDif = (_errorCurrent - _errorPrevious) / currentTimeS;
                result = Kp * _errorCurrent + Ki * _errorIntegral + Kd * _errorDif;
                _errorPrevious = _errorCurrent;
            }
            else
            {
                _errorPrevious = isUp ? targetTemp - currentTemp : currentTemp - targetTemp;
            }
            if (result < minVal)
            {
                result = minVal;
            }
            if (result > maxVal)
            {
                result = maxVal;
            }

            _lastTimeMS = Counter;
            return result;
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
