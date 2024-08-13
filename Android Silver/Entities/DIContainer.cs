using Android_Silver.Entities.FBEntities;
using Android_Silver.Entities.Modes;
using Android_Silver.Entities.Visual;
using Android_Silver.Pages;
using Android_Silver.Services;
using Android_Silver.ViewModels;

using Unity;

namespace Android_Silver.Entities
{
    public static class DIContainer
    {
        private static UnityContainer _container;
        public static void RegisterDependencies()
        {
            _container = new UnityContainer();
            _container.RegisterSingleton<EthernetEntities>();
            _container.RegisterSingleton<SensorsEntities>();
            _container.RegisterSingleton<TcpClientService>();
            _container.RegisterSingleton<SetPoints>();
            _container.RegisterSingleton<ModesEntities>();
            _container.RegisterSingleton<PicturesSet>();
            _container.RegisterSingleton<ActivePagesEntities>();
            _container.RegisterSingleton<Alarms>();
            _container.RegisterSingleton<HumiditySPS>();
            _container.RegisterSingleton<FilterVals>();
            _container.RegisterSingleton<FBs>();
        }

        public static T Resolve<T>()
        {
            return _container.Resolve<T>();
        }
    }
}

