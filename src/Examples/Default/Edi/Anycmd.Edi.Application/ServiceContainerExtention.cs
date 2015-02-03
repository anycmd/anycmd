
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
        /// <param name="acDomain"></param>
        public static void RegisterEdiCore(this IAcDomain acDomain)
        {
            #region Edi
            acDomain.AddService(typeof(IExecutorFactory), new ExecutorFactory());
            acDomain.AddService(typeof(IDispatcherFactory), new DispatcherFactory());
            acDomain.AddService(typeof(IAuthenticator), new DefaultAuthenticator());
            acDomain.AddService(typeof(IMessageProducer), new DefaultMessageProducer());
            acDomain.AddService(typeof(IStackTraceFormater), new JsonStackTraceFormater());
            acDomain.AddService(typeof(IInputValidator), new DefaultInputValidator());
            acDomain.AddService(typeof(IAuditDiscriminator), new DefaultAuditDiscriminator());
            acDomain.AddService(typeof(IPermissionValidator), new DefaultPermissionValidator());
            #endregion
        }
    }
}
