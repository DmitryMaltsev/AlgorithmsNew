using Android.App;
using Android.Content.PM;
using Android.OS;
using Java.Util;

using Android.App;
using Android.Content;
using Android.Net.Wifi;
using Android.OS;
using Android.Widget;



namespace Android_Silver
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            Button button = FindViewById<Button>(Resource.Id.button1);
            ListView listView = FindViewById<ListView>(Resource.Id.listView1);

            button.Click += (sender, e) =>
            {
                WifiManager wifiManager = (WifiManager)GetSystemService(Context.WifiService);
                wifiManager.StartScan();

                var wifiScanReceiver = new WifiScanReceiver(wifiManager, listView);
                RegisterReceiver(wifiScanReceiver, new IntentFilter(WifiManager.ScanResultsAvailableAction));
            };
        }

        private class WifiScanReceiver : BroadcastReceiver
        {
            private WifiManager wifiManager;
            private ListView listView;

            public WifiScanReceiver(WifiManager wifiManager, ListView listView)
            {
                this.wifiManager = wifiManager;
                this.listView = listView;
            }

            public override void OnReceive(Context context, Intent intent)
            {
                IList<ScanResult> scanResults = wifiManager.ScanResults;

                List<string> wifiList = new List<string>();
                foreach (var result in scanResults)
                {
                    wifiList.Add(result.Ssid);
                }

                ArrayAdapter adapter = new ArrayAdapter(context, Android.Resource.Layout.SimpleListItem1, wifiList);
                listView.Adapter = adapter;
            }
        }
    }



}


