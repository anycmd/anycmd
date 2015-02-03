
namespace Anycmd.Engine.Host.Edi.MemorySets
{
    using Engine.Edi.Abstractions;
    using Exceptions;
    using Info;
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
    internal sealed class InfoStringConverterSet : IInfoStringConverterSet, IMemorySet
    {
        public static readonly IInfoStringConverterSet Empty = new InfoStringConverterSet(EmptyAcDomain.SingleInstance);

        private readonly Dictionary<string, IInfoStringConverter>
            _dic = new Dictionary<string, IInfoStringConverter>(StringComparer.OrdinalIgnoreCase);
        private bool _initialized = false;
        private readonly object _locker = new object();
        private readonly Guid _id = Guid.NewGuid();
        private readonly IAcDomain _acDomain;

        public Guid Id
        {
            get { return _id; }
        }

        /// <summary>
        /// 构造并接入总线
        /// </summary>
        internal InfoStringConverterSet(IAcDomain acDomain)
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
        /// <param name="infoFormat"></param>
        /// <param name="converter"></param>
        /// <returns></returns>
        public bool TryGetInfoStringConverter(string infoFormat, out IInfoStringConverter converter)
        {
            if (!_initialized)
            {
                Init();
            }
            if (infoFormat == null)
            {
                converter = null;
                return false;
            }
            if (!_dic.ContainsKey(infoFormat))
            {
                converter = null;
                return false;
            }
            return _dic.TryGetValue(infoFormat, out converter);
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
        public IEnumerator<IInfoStringConverter> GetEnumerator()
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
            lock (_locker)
            {
                if (_initialized) return;
                _acDomain.MessageDispatcher.DispatchMessage(new MemorySetInitingEvent(this));
                _dic.Clear();

                var convertors = GetInfoStringConverters();
                if (convertors != null)
                {
                    var infoStringConverters = convertors as IInfoStringConverter[] ?? convertors.ToArray();
                    foreach (var item in infoStringConverters)
                    {
                        if (_dic.ContainsKey(item.InfoFormat))
                        {
                            throw new AnycmdException("信息格式转化器暂不支持优先级策略，每种格式只允许映射一个转化器");
                        }
                        var item1 = item;
                        _dic.Add(item.InfoFormat, infoStringConverters.Single(a => a.Id == item1.Id));
                    }
                }
                _initialized = true;
                _acDomain.MessageDispatcher.DispatchMessage(new MemorySetInitializedEvent(this));
            }
        }

        private IEnumerable<IInfoStringConverter> GetInfoStringConverters()
        {
            IEnumerable<IInfoStringConverter> r = null;
            using (var catalog = new DirectoryCatalog(Path.Combine(_acDomain.GetPluginBaseDirectory(PluginType.InfoStringConverter), "Bin")))
            using (var container = new CompositionContainer(catalog))
            {
                var infoValueConverterImport = new InfoStringConverterImport();
                infoValueConverterImport.ImportsSatisfied += (sender, e) =>
                {
                    r = e.InfoStringConverters;
                };
                container.ComposeParts(infoValueConverterImport);
            }

            return r;
        }

        private class InfoStringConverterImport : IPartImportsSatisfiedNotification
        {
            /// <summary>
            /// 
            /// </summary>
            [ImportMany(typeof(IInfoStringConverter), AllowRecomposition = true)]
            private IEnumerable<IInfoStringConverter> InfoStringConverters { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public event EventHandler<InfoStringConverterImportEventArgs> ImportsSatisfied;

            /// <summary>
            /// 
            /// </summary>
            public void OnImportsSatisfied()
            {
                if (ImportsSatisfied != null)
                {
                    ImportsSatisfied(this, new InfoStringConverterImportEventArgs(
                        this.InfoStringConverters));
                }
            }
        }

        private class InfoStringConverterImportEventArgs : EventArgs
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="infoValueConverters"></param>
            public InfoStringConverterImportEventArgs(
                IEnumerable<IInfoStringConverter> infoValueConverters)
            {
                this.InfoStringConverters = infoValueConverters;
            }

            /// <summary>
            /// 
            /// </summary>
            public IEnumerable<IInfoStringConverter> InfoStringConverters
            {
                get;
                private set;
            }
        }
    }
}