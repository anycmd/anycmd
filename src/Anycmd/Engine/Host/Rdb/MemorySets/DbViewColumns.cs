
namespace Anycmd.Engine.Host.Rdb.MemorySets
{
    using Engine.Rdb;
    using Exceptions;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// 表列上下文
    /// </summary>
    public sealed class DbViewColumns : IDbViewColumns
    {
        public static readonly IDbViewColumns Empty = new DbViewColumns(EmptyAcDomain.SingleInstance);

        private readonly Dictionary<RdbDescriptor, Dictionary<DbView, Dictionary<string, DbViewColumn>>>
            _dic = new Dictionary<RdbDescriptor, Dictionary<DbView, Dictionary<string, DbViewColumn>>>();
        private readonly Dictionary<RdbDescriptor, Dictionary<string, DbViewColumn>> _dicById = new Dictionary<RdbDescriptor, Dictionary<string, DbViewColumn>>();
        private bool _initialized = false;
        private readonly IAcDomain _host;

        public DbViewColumns(IAcDomain host)
        {
            this._host = host;
            // TODO:接入总线
        }

        public bool TryGetDbViewColumns(RdbDescriptor database, DbView view, out IReadOnlyDictionary<string, DbViewColumn> dbViewColumns)
        {
            if (!_initialized)
            {
                Init();
            }
            if (!_dic.ContainsKey(database))
            {
                dbViewColumns = new Dictionary<string, DbViewColumn>(StringComparer.OrdinalIgnoreCase);
                return false;
            }
            Dictionary<string, DbViewColumn> outDic;
            var r = _dic[database].TryGetValue(view, out outDic);
            dbViewColumns = outDic;
            return r;
        }

        public bool TryGetDbViewColumn(RdbDescriptor database, string viewColumnId, out DbViewColumn dbViewColumn)
        {
            if (!_initialized)
            {
                Init();
            }
            if (!_dicById.ContainsKey(database))
            {
                dbViewColumn = null;
                return false;
            }
            return _dicById[database].TryGetValue(viewColumnId, out dbViewColumn);
        }

        private void Init()
        {
            if (!_initialized)
            {
                lock (this)
                {
                    if (!_initialized)
                    {
                        _dic.Clear();
                        _dicById.Clear();
                        foreach (var database in _host.Rdbs)
                        {
                            _dic.Add(database, new Dictionary<DbView, Dictionary<string, DbViewColumn>>());
                            _dicById.Add(database, new Dictionary<string, DbViewColumn>(StringComparer.OrdinalIgnoreCase));
                            var columns = _host.RetrieveRequiredService<IOriginalHostStateReader>().GetViewColumns(database);
                            foreach (var view in database.DbViews.Values)
                            {
                                if (_dic[database].ContainsKey(view))
                                {
                                    // TODO:暂不支持Schema
                                    throw new AnycmdException("重名的数据库视图" + database.Database.CatalogName + "." + view.SchemaName + "." + view.Name);
                                }
                                _dic[database].Add(view, new Dictionary<string, DbViewColumn>(StringComparer.OrdinalIgnoreCase));
                                foreach (var viewCol in columns.Where(a => a.ViewName == view.Name && a.SchemaName == view.SchemaName))
                                {
                                    _dic[database][view].Add(viewCol.Name, viewCol);
                                    _dicById[database].Add(viewCol.Id, viewCol);
                                }
                            }
                        }
                        _initialized = true;
                    }
                }
            }
        }
    }
}