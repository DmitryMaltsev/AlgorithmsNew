using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Unity;

namespace TCPTest.Entities
{
    internal static class DIContainer
    {
        private static UnityContainer _container;
        public static void RegisterDependencies()
        {
            _container = new UnityContainer();
            _container.RegisterType<IEthernetEntities,EthernetEntities>();
        }

        public static T Resolve<T>()
        {
            return _container.Resolve<T>();
        }
    }
}

