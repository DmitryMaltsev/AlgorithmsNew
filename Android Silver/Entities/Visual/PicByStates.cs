using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Android_Silver.Entities.Visual
{
    public class PicByStates
    {
        public string Selected { get; set; }
        public string Default { get; set; }
        public PicByStates(string def, string selected)
        {
            Selected = selected;
            Default = def;
        }
    }
}
