using Android.App;
using Android.Runtime;


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

        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}