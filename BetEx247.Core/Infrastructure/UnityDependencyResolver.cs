using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Configuration;

namespace BetEx247.Core.Infrastructure
{
    public class UnityDependencyResolver : IDependencyResolver
    {
        private IUnityContainer container;

        public UnityDependencyResolver()
        {
            container = new UnityContainer();
            UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            section.Containers.Default.Configure(container);
        }

        public T Resolve<T>()
        {
            return container.Resolve<T>();
        }

        public T Resolve<T>(string name)
        {
            return container.Resolve<T>(name);
        }

        public void Register<T>(T instance)
        {
            throw new NotImplementedException();
        }

        public void Inject<T>(T existing)
        {
            throw new NotImplementedException();
        }

        public T Resolve<T>(Type type)
        {
            throw new NotImplementedException();
        }

        public T Resolve<T>(Type type, string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            throw new NotImplementedException();
        }
    }
}
