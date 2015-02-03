
namespace Anycmd.Edi.Application
{
    using DataContracts;
    using Engine.Edi;
    using Funq;
    using MessageServices;
    using ServiceStack;
    using System;
    using Util;

    /// <summary>
    /// Create your ServiceSelfHost web service application with a singleton ServiceSelfHost.
    /// </summary>
    public sealed class ServiceSelfHost : AppHostHttpListenerBase
    {
        private readonly ProcessDescriptor _process;
        private readonly IAcDomain _acDomain;

        public ServiceSelfHost(Anycmd.IAcDomain acDomain, ProcessDescriptor process)
            : base("Self-Host", typeof(MessageService).Assembly)
        {
            if (acDomain == null)
            {
                throw new ArgumentNullException("acDomain");
            }
            if (process == null)
            {
                throw new ArgumentNullException("process");
            }
            this._acDomain = acDomain;
            this._process = process;
            this.ServiceName = process.ProcessType.ToName() + " - " + process.Process.Name;
        }

        public override ServiceStackHost Init()
        {
            var acDomain = base.Init();
            acDomain.Start(_process.WebApiBaseAddress);
            return acDomain;
        }

        public override void Configure(Container container)
        {
            var adapter = new ServiceContainerAdapter(_acDomain);
            container.Adapter = adapter;

            SetConfig(new HostConfig
            {
                DebugMode = true,
                WsdlServiceNamespace = Consts.Namespace,
                EnableFeatures = Feature.Metadata | Feature.Json | Feature.Jsv | Feature.Html
            });
        }
    }

}
