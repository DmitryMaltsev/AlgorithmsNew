using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Media.Midi;
using Android.Net;
using Android.Net.Wifi;
using Android.Net.Wifi.Rtt;
using Android.OS;
using Android.Widget;

using Android_Silver.Entities;

using AndroidX.Activity.Result;
using AndroidX.Activity.Result.Contract;
using AndroidX.Fragment.App;

using Java.Util;

using Microsoft.Maui.Storage;

using System.Net.Http;
using System.Net.Sockets;

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

            var currentActivity = Platform.CurrentActivity as FragmentActivity;
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

        public bool IsConnectedToWifi()
        {
            var connectivityManager = (ConnectivityManager)Android.App.Application.Context.GetSystemService(Context.ConnectivityService);
            var activeNetwork = connectivityManager.ActiveNetworkInfo;

            return activeNetwork != null && activeNetwork.IsConnected && activeNetwork.Type == ConnectivityType.Wifi;
        }

        public string GetCurrentWifiSsid()
        {
            WifiManager wifiManager = (WifiManager)Android.App.Application.Context.GetSystemService(Context.WifiService);
            var wifiInfo = wifiManager.ConnectionInfo;
            return wifiInfo.SSID.Trim('"');
        }

    }



    public class CustomFilePickerService : IFilePicker
    {
        private readonly ActivityResultLauncher _filePickerLauncher;
        public CustomFilePickerService()
        {

            var currentActivity = Platform.CurrentActivity as FragmentActivity;
            // Регистрируем обработчик результата
            _filePickerLauncher = currentActivity.RegisterForActivityResult(
               new ActivityResultContracts.StartActivityForResult(),
               HandleActivityResult); // HandleActivityResult — ваш метод для обработки
        }

           public IActivityResultCallback HandleActivityResult { get; private set; }

        public async Task<FileResult> PickAsync2(PickOptions options)
        {
            // Проверяем, Xiaomi ли устройство (MIUI)
            if (Build.Manufacturer?.Equals("xiaomi", StringComparison.OrdinalIgnoreCase) == true)
            {
                //   return await PickForXiaomiAsync(options);
            }

            // Для других брендов используем стандартный пикер
            return await FilePicker.Default.PickAsync(options);
        }

        public Task<FileResult> PickAsync(PickOptions options)
        {
            TaskCompletionSource<FileResult> tcs = new TaskCompletionSource<FileResult>(TaskCreationOptions.RunContinuationsAsynchronously);

            var intent = new Intent(Intent.ActionOpenDocument);
            intent.AddCategory(Intent.CategoryOpenable);
            intent.SetType("*/*");
            _filePickerLauncher.Launch(intent);
            // Обработка результата выбора
            // (Здесь нужна реализация через OnActivityResult или ActivityResultLauncher)
            // ...

            return tcs.Task;
        }

        public Task<IEnumerable<FileResult>> PickMultipleAsync(PickOptions options)
        {
            // Аналогичная логика для выбора нескольких файлов
            // Для простоты примера возвращаем стандартное поведение
            return FilePicker.Default.PickMultipleAsync(options);
        }
    }
}


