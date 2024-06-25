using Android_Silver.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Android_Silver.Entities.Visual
{
    public class PicByStates:BindableBase
    {
        public string Selected { get; set; }
        public string Default { get; set; }

        private string _current;

        public string Current
        {
            get { return _current; }
            set
            {
                _current = value;
                OnPropertyChanged(nameof(Current));
            }
        }

        public PicByStates(string def, string selected)
        {
            Selected = selected;
            Default = def;
            Current = def;
        }
    }
}
