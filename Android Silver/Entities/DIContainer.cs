using Android_Silver.Entities.Visual;
using Android_Silver.Services;

using Unity;

namespace Android_Silver.Entities
{
    internal static class DIContainer
    {
        private static UnityContainer _container;
        public static void RegisterDependencies()
        {
            _container = new UnityContainer();
            _container.RegisterSingleton<IEthernetEntities, EthernetEntities>();
            _container.RegisterSingleton<SensorsEntities>();
            _container.RegisterSingleton<ITcpClientService,TcpClientService>();
            _container.RegisterSingleton<SetPoints>();
            _container.RegisterSingleton<ModesEntities>();
            _container.RegisterSingleton<PicturesSet>();
        }

        public static T Resolve<T>()
        {
            return _container.Resolve<T>();
        }
    }
}

