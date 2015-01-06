
namespace Anycmd.Engine.Host.Edi.MemorySets
{
    using Engine.Edi;
    using Exceptions;
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
    internal sealed class MessageTransferSet : IMessageTransferSet
    {
        public static readonly IMessageTransferSet Empty = new MessageTransferSet(EmptyAcDomain.SingleInstance);

        private readonly Dictionary<Guid, IMessageTransfer> _dic = new Dictionary<Guid, IMessageTransfer>();
        private bool _initialized = false;
        private readonly object _locker = new object();
        private readonly Guid _id = Guid.NewGuid();
        private readonly IAcDomain _host;

        public Guid Id
        {
            get { return _id; }
        }

        internal MessageTransferSet(IAcDomain host)
        {
            if (host == null)
            {
                throw new ArgumentNullException("host");
            }
            this._host = host;
        }

        /// <summary>
        /// 根据发送策略名索引发送策略
        /// </summary>
        /// <param name="transferId">发送策略名</param>
        /// <exception cref="AnycmdException">当给定名称的发送策略不存在时引发</exception>
        /// <returns></returns>
        /// <exception cref="AnycmdException">当转移器标识非法时抛出</exception>
        public IMessageTransfer this[Guid transferId]
        {
            get
            {
                if (!_initialized)
                {
                    Init();
                }
                if (!_dic.ContainsKey(transferId))
                {
                    throw new AnycmdException("意外的转移器标识");
                }

                return _dic[transferId];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transferId"></param>
        /// <param name="sendStrategy"></param>
        /// <returns></returns>
        public bool TryGetTransfer(Guid transferId, out IMessageTransfer sendStrategy)
        {
            if (!_initialized)
            {
                Init();
            }
            return _dic.TryGetValue(transferId, out sendStrategy);
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

        private void Init()
        {
            if (_initialized) return;
            lock (_locker)
            {
                if (_initialized) return;
                foreach (var item in _dic.Values)
                {
                    item.Dispose();
                }
                _dic.Clear();

                var transfers = GetTransfers();
                if (transfers != null)
                {
                    var messageTransfers = transfers as IMessageTransfer[] ?? transfers.ToArray();
                    foreach (var item in messageTransfers)
                    {
                        var item1 = item;
                        _dic.Add(item.Id, messageTransfers.Single(a => a.Id == item1.Id));
                    }
                }
                _initialized = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerator<IMessageTransfer> GetEnumerator()
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

        private IEnumerable<IMessageTransfer> GetTransfers()
        {
            IEnumerable<IMessageTransfer> r = null;
            using (var catalog = new DirectoryCatalog(Path.Combine(_host.GetPluginBaseDirectory(PluginType.MessageTransfer), "Bin")))
            using (var container = new CompositionContainer(catalog))
            {
                var infoValueConverterImport = new MessageTransferImport();
                infoValueConverterImport.ImportsSatisfied += (sender, e) =>
                {
                    r = e.Transfers;
                };
                container.ComposeParts(infoValueConverterImport);
            }
            return r;
        }

        private class MessageTransferImport : IPartImportsSatisfiedNotification
        {
            /// <summary>
            /// 
            /// </summary>
            [ImportMany(typeof(IMessageTransfer), AllowRecomposition = true)]
            private IEnumerable<IMessageTransfer> Transfers { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public event EventHandler<MessageTransferImportEventArgs> ImportsSatisfied;

            /// <summary>
            /// 
            /// </summary>
            public void OnImportsSatisfied()
            {
                if (ImportsSatisfied != null)
                {
                    ImportsSatisfied(this, new MessageTransferImportEventArgs(
                        this.Transfers));
                }
            }
        }

        private class MessageTransferImportEventArgs : EventArgs
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="transfers"></param>
            public MessageTransferImportEventArgs(IEnumerable<IMessageTransfer> transfers)
            {
                this.Transfers = transfers;
            }

            /// <summary>
            /// 
            /// </summary>
            public IEnumerable<IMessageTransfer> Transfers
            {
                get;
                private set;
            }
        }
    }
}