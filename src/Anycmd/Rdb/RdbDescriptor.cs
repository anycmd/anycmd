
namespace Anycmd.Rdb
{
    using Exceptions;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net;
    using Util;

    /// <summary>
    /// SQLServer数据库模型
    /// </summary>
    public sealed class RdbDescriptor
    {
        private static readonly object GlobalLocker = new object();

        private readonly Dictionary<string, DataTable> _tableSchemas = new Dictionary<string, DataTable>(StringComparer.OrdinalIgnoreCase);

        private string _connString;
        private DbProviderFactory _dbProviderFactory = null;
        private string _dataSource = null;
        private bool _isLocalhost = false;
        private bool _isLocalhostDetected = false;
        private readonly object _thisLocker = new object();
        private readonly IAcDomain _host;

        #region Ctor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <param name="database"></param>
        public RdbDescriptor(IAcDomain host, IRDatabase database)
        {
            if (database == null)
            {
                throw new ArgumentNullException("database");
            }
            if (host == null)
            {
                throw new ArgumentNullException("host");
            }
            this._host = host;
            RdbmsType rdbmsType;
            if (!database.RdbmsType.TryParse(out rdbmsType))
            {
                throw new AnycmdException("意外的关系数据库类型" + database.RdbmsType);
            }
            this.Database = database;
        }
        #endregion

        #region Public Properties
        public IAcDomain AcDomain { get { return _host; } }

        /// <summary>
        /// 
        /// </summary>
        public IRDatabase Database { get; private set; }

        public IReadOnlyDictionary<string, DbTable> DbTables
        {
            get { return _host.Rdbs.DbTables[this]; }
        }

        public IReadOnlyDictionary<string, DbView> DbViews
        {
            get { return _host.Rdbs.DbViews[this]; }
        }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string ConnString
        {
            get
            {
                if (string.IsNullOrEmpty(_connString))
                {
                    _connString = string.Format(
@"data source={0};initial catalog={1};user id={2};password={3};{4}"
                        , Database.DataSource, Database.CatalogName
                        , Database.UserId, Database.Password, Database.Profile);
                }

                return _connString;
            }
        }

        /// <summary>
        /// 查看本数据库的数据源是否指向本机。返回True表示指向本机。
        /// <remarks>
        /// 如果本数据的DataSource属性值为“.”或“localhost”或以“.\”或“localhost\”
        /// 开头则直接返回True。否则通过Dns系统检测是否是本机。
        /// </remarks>
        /// </summary>
        public bool IsLocalhost
        {
            get
            {
                if (!_isLocalhostDetected || !string.Equals(_dataSource, Database.DataSource, StringComparison.OrdinalIgnoreCase))
                {
                    lock (_thisLocker)
                    {
                        _isLocalhostDetected = true;
                        _dataSource = Database.DataSource;
                        if (string.IsNullOrEmpty(_dataSource))
                        {
                            throw new AnycmdException("数据源为空");
                        }
                        if (_dataSource == "."
                            || _dataSource.Equals("localhost", StringComparison.OrdinalIgnoreCase)
                            || _dataSource.StartsWith(".\\")
                            || _dataSource.StartsWith("localhost\\", StringComparison.OrdinalIgnoreCase)
                            || _dataSource == IPAddress.Loopback.ToString())
                        {
                            _isLocalhost = true;
                        }
                        else
                        {
                            HashSet<string> ips = IpHelper.GetLocalIPs();
                            _isLocalhost = ips.Contains(_dataSource);
                        }
                    }
                }
                return _isLocalhost;
            }
        }
        #endregion

        #region Public Methods

        public bool TryGetDbTable(string dbTableId, out DbTable dbTable)
        {
            return _host.Rdbs.DbTables.TryGetDbTable(this, dbTableId, out dbTable);
        }

        public bool TryGetDbView(string dbViewId, out DbView dbView)
        {
            return _host.Rdbs.DbViews.TryGetDbView(this, dbViewId, out dbView);
        }

        #region GetConnection
        /// <summary>
        /// Returns a new instance of the provider's class that implements the System.Data.Common.DbConnection
        ///     class.
        /// </summary>
        /// <returns></returns>
        public DbConnection GetConnection()
        {
            if (_dbProviderFactory == null)
            {
                _dbProviderFactory = DbProviderFactories.GetFactory(Database.ProviderName);
            }
            var conn = _dbProviderFactory.CreateConnection();
            Debug.Assert(conn != null, "conn != null");
            conn.ConnectionString = ConnString;

            return conn;
        }
        #endregion

        #region NewTable

        /// <summary>
        /// 返回给定的表模式克隆得到的新表
        /// </summary>
        /// <param name="dbTable"></param>
        /// <returns></returns>
        public DataTable NewTable(DbTable dbTable)
        {
            return this.GetTableSchema(dbTable).Clone();
        }
        #endregion

        #region WriteToServer
        /// <summary>
        /// Copies all rows in the supplied System.Data.DataTable to a destination table
        ///     specified by the System.Data.SqlClient.SqlBulkCopy.DestinationTableName property
        ///     of the System.Data.SqlClient.SqlBulkCopy object.
        /// </summary>
        /// <param name="table">A System.Data.DataTable whose rows will be copied to the destination table.</param>
        public void WriteToServer(DataTable table)
        {
            using (var conn = GetConnection())
            {
                if (!(conn is SqlConnection))
                {
                    throw new Exception();
                }
                var bulkCopy = new SqlBulkCopy(conn as SqlConnection)
                {
                    DestinationTableName = table.TableName,
                    BatchSize = table.Rows.Count
                };
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                bulkCopy.WriteToServer(table);
            }
        }
        #endregion

        #region ExecuteNonQuery
        /// <summary>
        /// Executes a SQLText statement against GetConnection().
        /// </summary>
        /// <param name="sqlText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sqlText, params SqlParameter[] parameters)
        {
            return ExecuteNonQuery(sqlText, CommandType.Text, parameters);
        }

        /// <summary>
        /// Executes a SQL statement against GetConnection().
        /// </summary>
        /// <param name="sqlText"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sqlText, CommandType commandType, params object[] parameters)
        {
            using (var conn = GetConnection())
            {
                DbCommand cmd = conn.CreateCommand();
                cmd.CommandType = commandType;
                cmd.CommandText = sqlText;
                if (parameters != null && parameters.Length > 0)
                {
                    foreach (var item in parameters)
                    {
                        cmd.Parameters.Add(item);
                    }
                }
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                return cmd.ExecuteNonQuery();
            }
        }
        #endregion

        #region ExecuteReader
        /// <summary>
        /// <remarks>在执行该命令时，如果关闭关联的 DataReader 对象，则关联的 Connection 对象也将关闭。</remarks>
        /// </summary>
        /// <param name="sqlText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DbDataReader ExecuteReader(string sqlText, params object[] parameters)
        {
            return ExecuteReader(sqlText, CommandType.Text, parameters);
        }

        /// <summary>
        /// Executes the System.Data.Common.DbCommand.CommandText against the System.Data.Common.DbCommand.Connection,
        ///     and returns an System.Data.Common.DbDataReader using one of the System.Data.CommandBehavior
        /// </summary>
        /// <remarks>CommandBehavior.CloseConnection关闭Reader则自动关闭连接</remarks>
        /// <param name="sqlText"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DbDataReader ExecuteReader(string sqlText, CommandType commandType, params object[] parameters)
        {
            var conn = GetConnection();
            DbCommand cmd = conn.CreateCommand();
            cmd.CommandText = sqlText;
            cmd.CommandType = commandType;
            if (parameters != null && parameters.Length > 0)
            {
                foreach (var item in parameters)
                {
                    cmd.Parameters.Add(item);
                }
            }
            if (conn.State != System.Data.ConnectionState.Open)
            {
                conn.Open();
            }
            DbDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            return reader;
        }
        #endregion

        #region ExecuteScalar
        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列。所有其他的列和行将被忽略。
        /// </summary>
        /// <param name="sqlText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public object ExecuteScalar(string sqlText, params object[] parameters)
        {
            return ExecuteScalar(sqlText, CommandType.Text, parameters);
        }

        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列。所有其他的列和行将被忽略。
        /// </summary>
        /// <param name="sqlText"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public object ExecuteScalar(string sqlText, CommandType commandType, params object[] parameters)
        {
            using (var conn = GetConnection())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = sqlText;
                cmd.CommandType = commandType;
                if (parameters != null && parameters.Length > 0)
                {
                    foreach (var item in parameters)
                    {
                        cmd.Parameters.Add(item);
                    }
                }
                if (conn.State != System.Data.ConnectionState.Open)
                {
                    conn.Open();
                }
                var result = cmd.ExecuteScalar();

                return result;
            }
        }
        #endregion

        #region Create
        /// <summary>
        /// 创建数据库
        /// <param name="useDb">使用这个数据库创建本数据库</param>
        /// <param name="filePath">将本数据库创建到磁盘的路径</param>
        /// </summary>
        public void Create(RdbDescriptor useDb, string filePath = null)
        {
            using (var conn = useDb.GetConnection())
            {
                var cmd = conn.CreateCommand();
                var config = string.Empty;
                if (!string.IsNullOrEmpty(filePath))
                {
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    var fileName = Path.Combine(filePath, Database.CatalogName + ".mdf");
                    config =
@" ON PRIMARY ( NAME = N'" + Database.CatalogName + @"', FILENAME = N'" + fileName + "')";
                }
                cmd.CommandText =
@"if DB_ID('" + Database.CatalogName + "') IS NULL CREATE DATABASE " + Database.CatalogName + config;
                cmd.CommandType = CommandType.Text;
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                cmd.ExecuteNonQuery();
            }
        }
        #endregion

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            var o = obj as RdbDescriptor;
            if (o == null)
            {
                return false;
            }

            return Database == o.Database || Database.Id == o.Database.Id;
        }

        public override int GetHashCode()
        {
            return Database.Id.GetHashCode();
        }
        #endregion

        #region GetTableSchema
        /// <summary>
        /// 获取给定表的表模式
        /// <remarks>表模式是一个ADO.NET表<see cref="DataTable"/></remarks>
        /// </summary>
        /// <returns></returns>
        private DataTable GetTableSchema(DbTable dbTable)
        {
            if (dbTable == null)
            {
                throw new ArgumentNullException("dbTable");
            }
            if (!_tableSchemas.ContainsKey(dbTable.Id))
            {
                lock (GlobalLocker)
                {
                    if (_tableSchemas.ContainsKey(dbTable.Id)) return _tableSchemas[dbTable.Id];
                    IReadOnlyDictionary<string, DbTableColumn> dbTableColumns;
                    if (!_host.Rdbs.DbTableColumns.TryGetDbTableColumns(this, dbTable, out dbTableColumns))
                    {
                        throw new AnycmdException("意外的数据库表");
                    }
                    var dataTable = new DataTable(dbTable.Name);
                    foreach (var col in dbTableColumns.Select(a => a.Value).OrderBy(a => a.Ordinal))
                    {
                        dataTable.Columns.Add(col.ToDataColumn());
                    }
                    _tableSchemas.Add(dbTable.Id, dataTable);
                }
            }
            return _tableSchemas[dbTable.Id];
        }
        #endregion
    }
}
