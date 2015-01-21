
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
    public sealed class DbTableColumns : IDbTableColumns
    {
        public static readonly IDbTableColumns Empty = new DbTableColumns(EmptyAcDomain.SingleInstance);

        private readonly Dictionary<RdbDescriptor, Dictionary<DbTable, Dictionary<string, DbTableColumn>>>
            _dic = new Dictionary<RdbDescriptor, Dictionary<DbTable, Dictionary<string, DbTableColumn>>>();
        private readonly Dictionary<RdbDescriptor, Dictionary<string, DbTableColumn>> _dicById = new Dictionary<RdbDescriptor, Dictionary<string, DbTableColumn>>();
        private bool _initialized = false;
        private readonly IAcDomain _host;

        public DbTableColumns(IAcDomain host)
        {
            this._host = host;
            // TODO:接入总线
        }

        public bool TryGetDbTableColumns(RdbDescriptor database, DbTable table, out IReadOnlyDictionary<string, DbTableColumn> dbTableColumns)
        {
            if (!_initialized)
            {
                Init();
            }
            if (!_dic.ContainsKey(database))
            {
                dbTableColumns = new Dictionary<string, DbTableColumn>(StringComparer.OrdinalIgnoreCase);
                return false;
            }
            Dictionary<string, DbTableColumn> outDic;
            var r = _dic[database].TryGetValue(table, out outDic);
            dbTableColumns = outDic;
            return r;
        }

        public bool TryGetDbTableColumn(RdbDescriptor database, string tableColumnId, out DbTableColumn dbTableColumn)
        {
            if (!_initialized)
            {
                Init();
            }
            if (!_dicById.ContainsKey(database))
            {
                dbTableColumn = null;
                return false;
            }
            return _dicById[database].TryGetValue(tableColumnId, out dbTableColumn);
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
                            var columns = _host.RetrieveRequiredService<IOriginalHostStateReader>().GetTableColumns(database);
                            _dic.Add(database, new Dictionary<DbTable, Dictionary<string, DbTableColumn>>());
                            _dicById.Add(database, new Dictionary<string, DbTableColumn>(StringComparer.OrdinalIgnoreCase));
                            foreach (var table in database.DbTables.Values)
                            {
                                if (_dic[database].ContainsKey(table))
                                {
                                    // 不计划支持Schema
                                    throw new AnycmdException("重名的数据库表" + database.Database.CatalogName + "." + table.SchemaName + "." + table.Name);
                                }
                                _dic[database].Add(table, new Dictionary<string, DbTableColumn>(StringComparer.OrdinalIgnoreCase));
                                foreach (var tableCol in columns.Where(a => a.TableName == table.Name && a.SchemaName == table.SchemaName))
                                {
                                    _dic[database][table].Add(tableCol.Name, tableCol);
                                    _dicById[database].Add(tableCol.Id, tableCol);
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