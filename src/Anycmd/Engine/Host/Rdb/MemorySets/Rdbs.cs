
namespace Anycmd.Engine.Host.Rdb.MemorySets
{
    using Engine.Rdb;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// SQLServer数据库上下文
    /// </summary>
    public sealed class Rdbs : IRdbs
    {
        public static readonly IRdbs Empty = new Rdbs(EmptyAcDomain.SingleInstance, MemorySets.DbTables.Empty, MemorySets.DbViews.Empty, MemorySets.DbTableColumns.Empty, MemorySets.DbViewColumns.Empty);
        private static readonly object Locker = new object();

        private readonly Dictionary<Guid, RdbDescriptor> _dicById = new Dictionary<Guid, RdbDescriptor>();
        private bool _initialized;
        private readonly IAcDomain _acDomain;
        private readonly IDbTableColumns _dbTableColumns;
        private readonly IDbTables _dbTables;
        private readonly IDbViewColumns _dbViewColumns;
        private readonly IDbViews _dbViews;

        public Rdbs(IAcDomain acDomain, IDbTables dbTables, IDbViews dbViews, IDbTableColumns dbTableColumns, IDbViewColumns dbViewColumns)
        {
            if (acDomain == null)
            {
                throw new ArgumentNullException("acDomain");
            }
            if (dbTables == null)
            {
                throw new ArgumentNullException("dbTables");
            }
            if (dbViews == null)
            {
                throw new ArgumentNullException("dbViews");
            }
            if (dbTableColumns == null)
            {
                throw new ArgumentNullException("dbTableColumns");
            }
            if (dbViewColumns == null)
            {
                throw new ArgumentNullException("dbViewColumns");
            }
            _acDomain = acDomain;
            _dbTables = dbTables;
            _dbViews = dbViews;
            _dbTableColumns = dbTableColumns;
            _dbViewColumns = dbViewColumns;
        }

        /// <summary>
        /// 关系数据库表列数据集
        /// </summary>
        public IDbTableColumns DbTableColumns
        {
            get { return _dbTableColumns; }
        }

        /// <summary>
        /// 关系数据库表数据集
        /// </summary>
        public IDbTables DbTables
        {
            get { return _dbTables; }
        }

        /// <summary>
        /// 关系数据库视图列数据集
        /// </summary>
        public IDbViewColumns DbViewColumns
        {
            get { return _dbViewColumns; }
        }

        /// <summary>
        /// 关系数据库视图数据集
        /// </summary>
        public IDbViews DbViews
        {
            get { return _dbViews; }
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

        private void Init()
        {
            if (_initialized) return;
            lock (Locker)
            {
                if (_initialized) return;
                _dicById.Clear();
                var list = _acDomain.RetrieveRequiredService<IOriginalHostStateReader>().GetAllRDatabases();
                foreach (var item in list)
                {
                    _dicById.Add(item.Id, new RdbDescriptor(_acDomain, item));
                }
                _initialized = true;
            }
        }
    }
}