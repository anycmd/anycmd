
namespace Anycmd.Edi.Application {
    using Engine.Host.Edi;
    using Engine.Host.Edi.Handlers;
    using Engine.Host.Edi.Handlers.Distribute;
    using Engine.Host.Edi.Handlers.Execute;

    /// <summary>
    /// 
    /// </summary>
    public static class ServiceContainerExtention {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        public static void RegisterEdiCore(this IAcDomain host)
        {
            #region Edi
            host.AddService(typeof(IExecutorFactory), new ExecutorFactory());
            host.AddService(typeof(IDispatcherFactory), new DispatcherFactory());
            host.AddService(typeof(IAuthenticator), new DefaultAuthenticator());
            host.AddService(typeof(IMessageProducer), new DefaultMessageProducer());
            host.AddService(typeof(IStackTraceFormater), new JsonStackTraceFormater());
            host.AddService(typeof(IInputValidator), new DefaultInputValidator());
            host.AddService(typeof(IAuditDiscriminator), new DefaultAuditDiscriminator());
            host.AddService(typeof(IPermissionValidator), new DefaultPermissionValidator());
            #endregion
        }
    }
}
