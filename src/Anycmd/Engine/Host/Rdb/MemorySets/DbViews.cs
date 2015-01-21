
namespace Anycmd.Engine.Host.Rdb.MemorySets
{
    using Engine.Rdb;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 数据库视图上下文
    /// </summary>
    public sealed class DbViews : IDbViews
    {
        public static readonly IDbViews Empty = new DbViews(EmptyAcDomain.SingleInstance);

        private readonly Dictionary<RdbDescriptor, Dictionary<string, DbView>> _dicById = new Dictionary<RdbDescriptor, Dictionary<string, DbView>>();
        private bool _initialized = false;
        private readonly IAcDomain _host;

        public DbViews(IAcDomain host)
        {
            this._host = host;
        }

        /// <summary>
        /// 根据数据库索引该库的所有视图
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public IReadOnlyDictionary<string, DbView> this[RdbDescriptor db]
        {
            get
            {
                if (!_initialized)
                {
                    Init();
                }
                if (!_dicById.ContainsKey(db))
                {
                    return new Dictionary<string, DbView>(StringComparer.OrdinalIgnoreCase);
                }
                return _dicById[db];
            }
        }

        public bool TryGetDbView(RdbDescriptor db, string dbViewId, out DbView dbView)
        {
            if (!_initialized)
            {
                Init();
            }
            if (!_dicById.ContainsKey(db))
            {
                dbView = null;
                return false;
            }
            return _dicById[db].TryGetValue(dbViewId, out dbView);
        }

        private void Init()
        {
            if (_initialized) return;
            lock (this)
            {
                if (_initialized) return;
                _dicById.Clear();
                foreach (var db in _host.Rdbs)
                {
                    _dicById.Add(db, new Dictionary<string, DbView>(StringComparer.OrdinalIgnoreCase));
                    var views = _host.RetrieveRequiredService<IOriginalHostStateReader>().GetDbViews(db);
                    foreach (var item in views)
                    {
                        _dicById[db].Add(item.Id, item);
                    }
                }
                _initialized = true;
            }
        }
    }
}