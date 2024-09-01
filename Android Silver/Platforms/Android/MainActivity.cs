using Android.App;
using Android.Content.PM;
using Android.OS;
using Java.Util;
using Android.Content;
using Android.Net.Wifi;
using Android.Widget;
using Android.Media.Midi;
using System.Net.Sockets;
using System.Net.Http;
using Android_Silver.Entities;
using Android.Net;
using Android.Net.Wifi.Rtt;

namespace Android_Silver
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        private WifiStateReceiver _wifiReciever;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var filter = new IntentFilter(WifiManager.WifiStateChangedAction);
            _wifiReciever = new WifiStateReceiver();
            RegisterReceiver(_wifiReciever, filter);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (_wifiReciever != null)
            {
                {
                    UnregisterReceiver(_wifiReciever);
                }
            }
        }
    }

    [BroadcastReceiver(Enabled = true, Exported = true)]
    public class WifiStateReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.Action == WifiManager.WifiStateChangedAction)
            {
                int wifiState = intent.GetIntExtra(WifiManager.ExtraWifiState, -1);
                switch (wifiState)
                {
                    case (int)WifiState.Enabled:
                        AndroidEntity.IsWifiEnabled = true;
                        AndroidEntity.WifiStateChanged?.Invoke(AndroidEntity.IsWifiEnabled);
                        AndroidEntity.IsWifiConnected = IsConnectedToWifi();
                        // Wi-Fi включен
                        break;
                    case (int)WifiState.Disabled:
                        AndroidEntity.IsWifiEnabled = false;
                        AndroidEntity.WifiStateChanged?.Invoke(AndroidEntity.IsWifiEnabled);
                        // Wi-Fi выключен
                        break;
                }
            }
        }

        public  bool IsConnectedToWifi()
        {
            var connectivityManager = (ConnectivityManager)Android.App.Application.Context.GetSystemService(Context.ConnectivityService);
            var activeNetwork = connectivityManager.ActiveNetworkInfo;

            return activeNetwork != null && activeNetwork.IsConnected && activeNetwork.Type == ConnectivityType.Wifi;
        }

        public  string GetCurrentWifiSsid()
        {
            WifiManager wifiManager = (WifiManager)Android.App.Application.Context.GetSystemService(Context.WifiService);
            var wifiInfo = wifiManager.ConnectionInfo;
            return wifiInfo.SSID.Trim('"');
        }
    }
}


