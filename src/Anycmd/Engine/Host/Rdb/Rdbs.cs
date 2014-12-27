
namespace Anycmd.Engine.Host.Rdb
{
    using Anycmd.Rdb;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// SQLServer数据库上下文
    /// </summary>
    public sealed class Rdbs : IRdbs
    {
        public static readonly IRdbs Empty = new Rdbs(EmptyAcDomain.SingleInstance);

        private readonly Dictionary<Guid, RdbDescriptor> _dicById = new Dictionary<Guid, RdbDescriptor>();
        private bool _initialized = false;
        private readonly IAcDomain _host;

        public Rdbs(IAcDomain host)
        {
            this._host = host;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbId"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        public bool TryDb(Guid dbId, out RdbDescriptor database)
        {
            if (!_initialized)
            {
                Init();
            }
            return _dicById.TryGetValue(dbId, out database);
        }

        public bool ContainsDb(Guid dbId)
        {
            if (!_initialized)
            {
                Init();
            }
            return _dicById.ContainsKey(dbId);
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
            if (!_initialized)
            {
                lock (this)
                {
                    if (!_initialized)
                    {
                        _dicById.Clear();
                        var list = _host.RetrieveRequiredService<IOriginalHostStateReader>().GetAllRDatabases();
                        foreach (var item in list)
                        {
                            _dicById.Add(item.Id, new RdbDescriptor(_host, item));
                        }
                        _initialized = true;
                    }
                }
            }
        }

        public IEnumerator<RdbDescriptor> GetEnumerator()
        {
            if (!_initialized)
            {
                Init();
            }
            return _dicById.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (!_initialized)
            {
                Init();
            }
            return _dicById.Values.GetEnumerator();
        }
    }
}