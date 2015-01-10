
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
    internal sealed class EntityProviderSet : IEntityProviderSet, IMemorySet
    {
        public static readonly IEntityProviderSet Empty = new EntityProviderSet(EmptyAcDomain.SingleInstance);

        private readonly Dictionary<Guid, IEntityProvider> _dic = new Dictionary<Guid, IEntityProvider>();
        private bool _initialized = false;
        private readonly Guid _id = Guid.NewGuid();
        private readonly IAcDomain _host;

        public Guid Id
        {
            get
            {
                return _id;
            }
        }

        /// <summary>
        /// 构造并接入总线
        /// </summary>
        internal EntityProviderSet(IAcDomain host)
        {
            if (host == null)
            {
                throw new ArgumentNullException("host");
            }
            if (host.Equals(EmptyAcDomain.SingleInstance))
            {
                _initialized = true;
            }
            this._host = host;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="providerId"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public bool TryGetEntityProvider(Guid providerId, out IEntityProvider provider)
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
        public IEnumerator<IEntityProvider> GetEnumerator()
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
            lock (this)
            {
                if (_initialized) return;
                _host.MessageDispatcher.DispatchMessage(new MemorySetInitingEvent(this));
                _dic.Clear();

                var entityProviders = GetEntityProviders();
                if (entityProviders != null)
                {
                    var enumerable = entityProviders as IEntityProvider[] ?? entityProviders.ToArray();
                    foreach (var item in enumerable)
                    {
                        var item1 = item;
                        _dic.Add(item.Id, enumerable.Single(a => a.Id == item1.Id));
                    }
                }
                _initialized = true;
                _host.MessageDispatcher.DispatchMessage(new MemorySetInitializedEvent(this));
            }
        }

        private IEnumerable<IEntityProvider> GetEntityProviders()
        {
            IEnumerable<IEntityProvider> r = null;
            using (var catalog = new DirectoryCatalog(Path.Combine(_host.GetPluginBaseDirectory(PluginType.EntityProvider), "Bin")))
            using (var container = new CompositionContainer(catalog))
            {
                var infoValueConverterImport = new EntityProviderImport();
                infoValueConverterImport.ImportsSatisfied += (sender, e) =>
                {
                    r = e.EntityProviders;
                };
                container.ComposeParts(infoValueConverterImport);
            }
            return r;
        }
        private class EntityProviderImport : IPartImportsSatisfiedNotification
        {
            /// <summary>
            /// 
            /// </summary>
            [ImportMany(typeof(IEntityProvider), AllowRecomposition = true)]
            private IEnumerable<IEntityProvider> EntityProviders { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public event EventHandler<EntityProviderImportEventArgs> ImportsSatisfied;

            /// <summary>
            /// 
            /// </summary>
            public void OnImportsSatisfied()
            {
                if (ImportsSatisfied != null)
                {
                    ImportsSatisfied(this, new EntityProviderImportEventArgs(
                        this.EntityProviders));
                }
            }
        }

        private class EntityProviderImportEventArgs : EventArgs
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="entityProviders"></param>
            public EntityProviderImportEventArgs(
                IEnumerable<IEntityProvider> entityProviders)
            {
                this.EntityProviders = entityProviders;
            }

            /// <summary>
            /// 
            /// </summary>
            public IEnumerable<IEntityProvider> EntityProviders
            {
                get;
                private set;
            }
        }
    }
}
