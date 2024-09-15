using Android_Silver.Entities.FBEntities;
using Android_Silver.Entities.FBEntities.SetPoints;
using Android_Silver.Entities.Modes;

using Android_Silver.Entities.Visual;
using Android_Silver.Entities.Visual.Menus;
using Android_Silver.Pages;
using Android_Silver.Services;
using Android_Silver.ViewModels;

using Unity;
using Unity.Injection;
using Unity.Resolution;

using MenuItem = Android_Silver.Entities.Visual.Menus.MItem;

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
            _container.RegisterSingleton<SetPoints>();
            _container.RegisterSingleton<ModesEntities>();
            _container.RegisterSingleton<PicturesSet>();
            _container.RegisterSingleton<ActivePagesEntities>();
            _container.RegisterSingleton<Alarms>();
            _container.RegisterSingleton<HumiditySPS>();
            _container.RegisterSingleton<FilterVals>();
            _container.RegisterSingleton<OtherSettings>();
            _container.RegisterSingleton<FBs>();
            _container.RegisterSingleton<Time>();
            _container.RegisterSingleton<CarouselViewData>();
            _container.RegisterSingleton<Recup>();
            _container.RegisterSingleton<ServiceActivePagesEntities>();
            _container.RegisterSingleton<Fans>();
            _container.RegisterSingleton<MenusEntities>();
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

