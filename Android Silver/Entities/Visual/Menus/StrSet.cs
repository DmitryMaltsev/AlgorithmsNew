using Android_Silver.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Android_Silver.Entities.Visual.Menus
{
    public class StrSet : BindableBase
    {

        private float _minVal;
        private float _maxVal;
      
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private List<string> _pickVals;
        public List<string> PickVals
        {
            get { return _pickVals; }
            set
            {
                _pickVals = value;
                OnPropertyChanged(nameof(PickVals));
            }
        }


        //private string _pickName;
        //public string PickName
        //{
        //    get { return _pickName; }
        //    set
        //    {
        //        _pickName = value;
        //        OnPropertyChanged(nameof(_pickName));;
        //    }
        //}

        private int _cPickVal;
        public int CPickVal
        {
            get { return _cPickVal; }
            set
            {
                if (value>-1)
                {
                    _cPickVal = value;
                    OnPropertyChanged(nameof(CPickVal));
                  //  PickName = PickVals[CPickVal];
                }
            }
        }

        private float _cVal;
        public float CVal
        {
            get { return _cVal; }
            set
            {
                if (value >= _minVal && value <= _maxVal)
                {
                    _cVal = value;
                    OnPropertyChanged(nameof(CVal));
                }
            }
        }

        private bool _pickerIsVisible;
        public bool PickerIsVisible
        {
            get { return _pickerIsVisible; }
            set { 
                _pickerIsVisible = value;
                OnPropertyChanged(nameof(PickerIsVisible));
            }
        }

        private bool _entryIsVisible;
        public bool EntryIsVisible
        {
            get { return _entryIsVisible; }
            set {
                _entryIsVisible = value;
                OnPropertyChanged(nameof(EntryIsVisible));
            }
        }

        private bool _isVisible;

        public bool IsVisible
        {
            get { return _isVisible; }
            set { 
                _isVisible = value;
                OnPropertyChanged(nameof(IsVisible));
            }
        }


        public StrSet(float minVal, float maxVal, string name,bool isVisible, bool pickerIsVisible, bool entryIsVisible, List<string> pickVals)
        {
            PickVals = pickVals;
            CPickVal=0;
            _minVal = minVal;
            _maxVal = maxVal;
            Name = name;
            IsVisible = isVisible;
            PickerIsVisible = pickerIsVisible;
            EntryIsVisible = entryIsVisible;
        }

    }
}
