using ServiceStack;

namespace Anycmd.Edi.Application
{
    using DataContracts;
    using Funq;
    using MessageServices;
    using System;
    using System.Reflection;

    /// <summary>
    /// Create your ServiceHost web service application with a singleton ServiceHost.
    /// </summary>        
    public class ServiceHost : AppHostBase
    {
        private readonly IAcDomain _acDomain;

        /// <summary>
        /// Initializes a new instance of your ServiceStack application, with the specified name and assembly containing the services.
        /// </summary>
        public ServiceHost(IAcDomain acDomain)
            : base("数据交换服务", typeof(MessageService).Assembly)
        {
            if (acDomain == null)
            {
                throw new ArgumentNullException("acDomain");
            }
            this._acDomain = acDomain;
        }

        public ServiceHost(IAcDomain acDomain, string serviceName, params Assembly[] assembliesWithServices)
            : base(serviceName, assembliesWithServices)
        {
            if (acDomain == null)
            {
                throw new ArgumentNullException("acDomain");
            }
            this._acDomain = acDomain;
        }

        /// <summary>
        /// Configure the container with the necessary routes for your ServiceStack application.
        /// </summary>
        /// <param name="container">The built-in IoC used with ServiceStack.</param>
        public override void Configure(Container container)
        {
            container.Adapter = new ServiceContainerAdapter(_acDomain);

            SetConfig(new HostConfig
            {
                DebugMode = true,
                WsdlServiceNamespace = Consts.Namespace,
                EnableFeatures = Feature.Metadata | Feature.Json | Feature.Jsv | Feature.Html
            });
        }
    }
}
