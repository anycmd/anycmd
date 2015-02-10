
namespace Anycmd.Engine.Host.Edi.MemorySets
{
    using Engine.Edi.Abstractions;
    using Handlers;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// 
    /// </summary>
    internal sealed class MessageProviderSet : IMessageProviderSet, IMemorySet
    {
        public static readonly IMessageProviderSet Empty = new MessageProviderSet(EmptyAcDomain.SingleInstance);

        private readonly Dictionary<Guid, IMessageProvider> _dic = new Dictionary<Guid, IMessageProvider>();
        private bool _initialized = false;
        private static readonly object Locker = new object();
        private readonly Guid _id = Guid.NewGuid();
        private readonly IAcDomain _acDomain;

        public Guid Id
        {
            get { return _id; }
        }

        /// <summary>
        /// 构造并接入总线
        /// </summary>
        internal MessageProviderSet(IAcDomain acDomain)
        {
            if (acDomain == null)
            {
                throw new ArgumentNullException("acDomain");
            }
            if (acDomain.Equals(EmptyAcDomain.SingleInstance))
            {
                _initialized = true;
            }
            this._acDomain = acDomain;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="providerId"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public bool TryGetMessageProvider(Guid providerId, out IMessageProvider provider)
        {
            if (!_initialized)
            {
                Init();
            }
            return _dic.TryGetValue(providerId, out provider);
        }

        /// <summary>
        /// 
        /// </summary>
        internal void Refresh()
        {
            if (_initialized)
            {
                _initialized = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerator<IMessageProvider> GetEnumerator()
        {
            if (!_initialized)
            {
                Init();
            }
            return _dic.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (!_initialized)
            {
                Init();
            }
            return _dic.Values.GetEnumerator();
        }

        private void Init()
        {
            if (_initialized) return;
            lock (Locker)
            {
                if (_initialized) return;
                _acDomain.MessageDispatcher.DispatchMessage(new MemorySetInitingEvent(this));
                _dic.Clear();
                var messageProviders = GetMessageProviders();
                if (messageProviders != null)
                {
                    var enumerable = messageProviders as IMessageProvider[] ?? messageProviders.ToArray();
                    foreach (var item in enumerable)
                    {
                        var item1 = item;
                        _dic.Add(item.Id, enumerable.Single(a => a.Id == item1.Id));
                    }
                }
                _initialized = true;
                _acDomain.MessageDispatcher.DispatchMessage(new MemorySetInitializedEvent(this));
            }
        }

        private IEnumerable<IMessageProvider> GetMessageProviders()
        {
            IEnumerable<IMessageProvider> r = null;
            using (var catalog = new DirectoryCatalog(Path.Combine(_acDomain.GetPluginBaseDirectory(PluginType.MessageProvider), "Bin")))
            using (var container = new CompositionContainer(catalog))
            {
                var infoValueConverterImport = new MessageProviderImport();

                infoValueConverterImport.ImportsSatisfied += (sender, e) =>
                {
                    r = e.MessageProviders;
                };
                container.ComposeParts(infoValueConverterImport);
            }
            return r;
        }

        private class MessageProviderImport : IPartImportsSatisfiedNotification
        {
            [ImportMany(typeof(IMessageProvider), AllowRecomposition = true)]
            private IEnumerable<IMessageProvider> MessageProviders { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public event EventHandler<MessageProviderImportEventArgs> ImportsSatisfied;

            /// <summary>
            /// 
            /// </summary>
            public void OnImportsSatisfied()
            {
                if (ImportsSatisfied != null)
                {
                    ImportsSatisfied(this, new MessageProviderImportEventArgs(
                        this.MessageProviders));
                }
            }
        }

        private class MessageProviderImportEventArgs : EventArgs
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="messageProviders"></param>
            public MessageProviderImportEventArgs(
                IEnumerable<IMessageProvider> messageProviders)
            {
                this.MessageProviders = messageProviders;
            }

            /// <summary>
            /// 
            /// </summary>
            public IEnumerable<IMessageProvider> MessageProviders
            {
                get;
                private set;
            }
        }
    }
}