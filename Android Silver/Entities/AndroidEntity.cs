using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Android_Silver.Entities
{
    public static class AndroidEntity
    {
        public static Action<bool> WifiStateChanged;
        public static bool IsWifiEnabled { get; set; }
        public static bool IsWifiConnected { get; set; }
    }
}
