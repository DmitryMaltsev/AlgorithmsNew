using Android.App;
using Android.Runtime;

using Xamarin.Essentials;

namespace Android_Silver
{
    [Application]
    public class MainApplication : MauiApplication
    {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {

        }

        private void Test()
        {
            // Проверка статуса подключения к WiFi
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)
            {
                // Подключение к WiFi
                var currentWifi = Connectivity.WiFiSSID;
                var currentBSSID = Connectivity.WiFiBSSID;

                // Проверка доступных WiFi сетей
                var wifiNetworks = Connectivity.WiFi.GetConnectedNetworks();

                // Получение информации о текущем подключении
                var connectionProfiles = Connectivity.ConnectionProfiles;
            }

        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}