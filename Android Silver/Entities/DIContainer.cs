using Android_Silver.Entities.FBEntities;
using Android_Silver.Entities.Modes;

using Android_Silver.Entities.Visual;
using Android_Silver.Entities.Visual.Menus;
using Android_Silver.Services;

using Unity;
using Unity.Resolution;

namespace Android_Silver.Entities
{
    public static class DIContainer
    {
        public static UnityContainer _container;
        public static void RegisterDependencies()
        {
            _container = new UnityContainer();
            _container.RegisterSingleton<EthernetEntities>();
            _container.RegisterSingleton<TcpClientService>();
            _container.RegisterSingleton<ModesEntities>();
            _container.RegisterSingleton<PicturesSet>();
            _container.RegisterSingleton<ActivePagesEntities>();
            _container.RegisterSingleton<FBs>();
            _container.RegisterSingleton<CarouselViewData>();
            _container.RegisterSingleton<ServiceActivePagesEntities>();
            _container.RegisterSingleton<MenusEntities>();
            _container.RegisterSingleton<FileSystemService>();
            _container.RegisterSingleton<MathService>();
            _container.RegisterSingleton<FilesEntities>();
        }

        public static T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        public static T Resolve<T>(ParameterOverride[] parameters)
        {
            return _container.Resolve<T>(parameters);
        }

    }
}

