
namespace jDTS.Mis.Web.Mvc {
    using Bootstrap;
    using Castle.Windsor;
    using DataContracts;
    using EDI.ApplicationServices;
    using Funq;
    using ServiceStack;

    /// <summary>
    /// Create your ServiceStack web service application with a singleton AppHost.
    /// </summary>        
    public class AppHost : AppHostBase {
        /// <summary>
        /// Initializes a new instance of your ServiceStack application, with the specified name and assembly containing the services.
        /// </summary>
        public AppHost() : base("EDI Web Api", typeof(EntityService).Assembly) { }

        /// <summary>
        /// Configure the container with the necessary routes for your ServiceStack application.
        /// </summary>
        /// <param name="container">The built-in IoC used with ServiceStack.</param>
        public override void Configure(Container container) {
            WindsorContainerAdapter adapter = new WindsorContainerAdapter((IWindsorContainer)Bootstrapper.Container);
            container.Adapter = adapter;

            SetConfig(new HostConfig {
                DebugMode = true,
                WsdlServiceNamespace = Consts.Namespace,
                EnableFeatures = Feature.Metadata | Feature.Json | Feature.Xml | Feature.Jsv | Feature.Html
            });
        }
    }
}
