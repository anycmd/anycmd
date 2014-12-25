
namespace Anycmd.Edi.Application
{
    using ServiceStack.Configuration;
    using System.ComponentModel.Design;

    public sealed class ServiceContainerAdapter : IContainerAdapter
    {
        private readonly IServiceContainer _container;

        public ServiceContainerAdapter(IServiceContainer container)
        {
            this._container = container;
        }

        public T TryResolve<T>()
        {
            return (T)_container.GetService(typeof(T));

        }

        public T Resolve<T>()
        {
            return (T)_container.GetService(typeof(T));
        }
    }
}
