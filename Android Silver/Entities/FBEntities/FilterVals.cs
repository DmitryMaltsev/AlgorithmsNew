using Android_Silver.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Android_Silver.Entities.FBEntities
{
    public class FilterVals : BindableBase
    {
        private bool _filterPol70;

        public bool FilterPol70
        {
            get { return _filterPol70; }
            set
            {
                _filterPol70 = value;
                OnPropertyChanged(nameof(FilterPol70));
            }
        }

        private bool _filterPol100;
        public bool FilterPol100
        {
            get { return _filterPol100; }
            set
            {
                _filterPol100 = value;
                OnPropertyChanged(nameof(FilterPol100));
            }
        }

        private bool _filterReset;

        public bool FilterReset
        {
            get { return _filterReset; }
            set
            {
                _filterReset = value;
                OnPropertyChanged(nameof(FilterReset));
            }
        }


        private int _filterPolPercent;
        public int FilterPolPercent
        {
            get { return _filterPolPercent; }
            set
            {
                _filterPolPercent = value;
                OnPropertyChanged($"{nameof(FilterPolPercent)}");
            }
        }
    }
}
