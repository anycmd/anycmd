
namespace Anycmd.Rdb
{
    using Exceptions;
    using Util;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics.Contracts;
    using System.Text;

    /// <summary>
    /// 数据库元数据提供程序。提供访问数据库表、视图、列元数据的方法。
    /// </summary>
    public sealed class SqlServerMetaDataService : IRdbMetaDataService
    {
        /// <summary>
        /// 数据库连接字符串引导库连接字符串
        /// </summary>
        private readonly string _bootConnString = ConfigurationManager.AppSettings["BootDbConnString"];

        /// <summary>
        /// 数据库连接字符串引导库连接字符串
        /// </summary>
        public string BootConnString { get { return _bootConnString; } }

        private readonly IAcDomain _host;

        public SqlServerMetaDataService(IAcDomain host)
        {
            this._host = host;
        }

        #region GetTableSpaces
        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="sortField"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        public IList<DbTableSpace> GetTableSpaces(RdbDescriptor db, string sortField, string sortOrder)
        {
            if (string.IsNullOrEmpty(sortField))
            {
                sortField = "Rows";
            }
            if (string.IsNullOrEmpty(sortOrder))
            {
                sortOrder = "asc";
            }
            Contract.Requires(sortOrder == "asc" || sortOrder == "desc");
            string sql =
@"CREATE TABLE #T
    (
      Name VARCHAR(255) ,
      [Rows] BIGINT ,
      Reserved VARCHAR(20) ,
      Data VARCHAR(20) ,
      IndexSize VARCHAR(20) ,
      UnUsed VARCHAR(20)
    )
EXEC SP_MSFOREACHTABLE ""INSERT INTO #T EXEC SP_SPACEUSED '?'""
SELECT  *
FROM    #T
ORDER BY " + sortField + " " + sortOrder +
@" DROP TABLE #T";
            var spaces = new List<DbTableSpace>();
            using (var reader = db.ExecuteReader(sql, null))
            {
                while (reader.Read())
                {
                    spaces.Add(DbTableSpace.Create(reader));
                }
                reader.Close();
            }

            return spaces;
        }
        #endregion

        #region GetDatabase
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RDatabase GetDatabase(Guid id)
        {
            var sql = "select * from [RDatabase] where Id=@Id";
            RDatabase database = null;
            using (var conn = new SqlConnection(this.BootConnString))
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("Id", id));
                if (conn.State != System.Data.ConnectionState.Open)
                {
                    conn.Open();
                }
                using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (reader.Read())
                    {
                        database = PopulateRDatabase(reader);
                    }
                }
            }

            return database;
        }
        #endregion

        #region GetDatabases
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IList<RDatabase> GetDatabases()
        {
            var list = new List<RDatabase>();
            const string sql = "select * from [RDatabase] order by CatalogName";
            using (var conn = new SqlConnection(this.BootConnString))
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandType = System.Data.CommandType.Text;
                if (conn.State != System.Data.ConnectionState.Open)
                {
                    conn.Open();
                }
                using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                    {
                        list.Add(PopulateRDatabase(reader));
                    }
                }
            }

            return list;
        }
        #endregion

        #region UpdateDatabase
        // TODO:如果DataSource或UserID或Password或Profile有变化则需要刷新数据库上下文
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dataSource"></param>
        /// <param name="description"></param>
        public void UpdateDatabase(Guid id, string dataSource, string description)
        {
            const string sql = "update [RDatabase] set DataSource=@DataSource,Description=@Description where Id=@Id";
            using (var conn = new SqlConnection(this.BootConnString))
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("Id", id));
                cmd.Parameters.Add(new SqlParameter("DataSource", dataSource));
                cmd.Parameters.Add(new SqlParameter("Description", description));
                if (conn.State != System.Data.ConnectionState.Open)
                {
                    conn.Open();
                }
                cmd.ExecuteNonQuery();
            }
        }

        #endregion

        #region GetViewDefinition
        public string GetViewDefinition(RdbDescriptor db, DbView view)
        {
            const string sql = "sp_helptext";
            var sb = new StringBuilder();
            using (var reader = db.ExecuteReader(sql, CommandType.StoredProcedure, new SqlParameter("objname", view.Name)))
            {
                while (reader.Read())
                {
                    sb.Append(reader.GetString(reader.GetOrdinal("Text")));
                }
            }
            return sb.ToString();
        }
        #endregion

        #region GetDbTables
        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public IList<DbTable> GetDbTables(RdbDescriptor db)
        {
            var sql = _host.Config.SqlServerTablesSelect;
            var tables = new List<DbTable>();
            using (var reader = db.ExecuteReader(sql, null))
            {
                while (reader.Read())
                {
                    tables.Add(new DbTable(db.Database.Id, reader));
                }
                reader.Close();
            }

            return tables;
        }
        #endregion

        #region GetDbViews
        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public IList<DbView> GetDbViews(RdbDescriptor db)
        {
            var queryString = _host.Config.SqlServerViewsSelect;
            var views = new List<DbView>();
            using (var reader = db.ExecuteReader(queryString, null))
            {
                while (reader.Read())
                {
                    views.Add(new DbView(db.Database.Id, reader));
                }
                reader.Close();
            }

            return views;
        }
        #endregion

        #region GetTableColumns
        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public IList<DbTableColumn> GetTableColumns(RdbDescriptor db)
        {
            var sql = _host.Config.SqlServerTableColumnsSelect;
            IList<DbTableColumn> list = new List<DbTableColumn>();
            using (var reader = db.ExecuteReader(sql, null))
            {
                while (reader.Read())
                {
                    list.Add(DbTableColumn.Create(db.Database.Id, reader));
                }
                reader.Close();
            }

            return list;
        }
        #endregion

        #region GetViewColumns
        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public IList<DbViewColumn> GetViewColumns(RdbDescriptor db)
        {
            var sql = _host.Config.SqlServerViewColumnsSelect;
            IList<DbViewColumn> list = new List<DbViewColumn>();
            using (var reader = db.ExecuteReader(sql, null))
            {
                while (reader.Read())
                {
                    list.Add(DbViewColumn.Create(db.Database.Id, reader));
                }
                reader.Close();
                return list;
            }
        }
        #endregion

        #region CrudDescription
        /*
         * sys.sp_addextendedproperty @name = NULL, -- sysname
                @value = NULL, -- sql_variant
                @level0type = '', -- varchar(128)
                @level0name = NULL, -- sysname
                @level1type = '', -- varchar(128)
                @level1name = NULL, -- sysname
                @level2type = '', -- varchar(128)
                @level2name = NULL -- sysname

            sys.sp_updateextendedproperty @name = NULL, -- sysname
                @value = NULL, -- sql_variant
                @level0type = '', -- varchar(128)
                @level0name = NULL, -- sysname
                @level1type = '', -- varchar(128)
                @level1name = NULL, -- sysname
                @level2type = '', -- varchar(128)
                @level2name = NULL -- sysname

            sys.sp_dropextendedproperty @name = NULL, -- sysname
                @level0type = '', -- varchar(128)
                @level0name = NULL, -- sysname
                @level1type = '', -- varchar(128)
                @level1name = NULL, -- sysname
                @level2type = '', -- varchar(128)
                @level2name = NULL -- sysname
         */
        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="id"></param>
        /// <param name="description"></param>
        /// <param name="metaDataType"></param>
        public void CrudDescription(RdbDescriptor db, RDbMetaDataType metaDataType, string id, string description)
        {
            const string addProcName = "sys.sp_addextendedproperty";
            const string updateProcName = "sys.sp_updateextendedproperty";
            const string dropProcName = "sys.sp_dropextendedproperty";
            const string propertyName = "MS_Description";
            switch (metaDataType)
            {
                case RDbMetaDataType.Database:
                    break;
                case RDbMetaDataType.Table:
                    #region Table
                    DbTable table;
                    if (!db.AcDomain.DbTables.TryGetDbTable(db, id, out table))
                    {
                        throw new AnycmdException("意外的数据库表" + id);
                    }
                    if (description == null && table.Description != null)
                    {
                        db.ExecuteNonQuery(dropProcName, CommandType.StoredProcedure,
                            new SqlParameter("name", propertyName),
                            new SqlParameter("level0type", "SCHEMA"),
                            new SqlParameter("level0name", table.SchemaName),
                            new SqlParameter("level1type", "TABLE"),
                            new SqlParameter("level1name", table.Name));
                    }
                    else if (table.Description == null)
                    {
                        db.ExecuteNonQuery(addProcName, CommandType.StoredProcedure,
                            new SqlParameter("name", propertyName),
                            new SqlParameter("value", description),
                            new SqlParameter("level0type", "SCHEMA"),
                            new SqlParameter("level0name", table.SchemaName),
                            new SqlParameter("level1type", "TABLE"),
                            new SqlParameter("level1name", table.Name));
                    }
                    else
                    {
                        db.ExecuteNonQuery(updateProcName, CommandType.StoredProcedure,
                            new SqlParameter("name", propertyName),
                            new SqlParameter("value", description),
                            new SqlParameter("level0type", "SCHEMA"),
                            new SqlParameter("level0name", table.SchemaName),
                            new SqlParameter("level1type", "TABLE"),
                            new SqlParameter("level1name", table.Name));
                    }
                    table.Description = description;
                    #endregion
                    break;
                case RDbMetaDataType.View:
                    #region View
                    DbView view;
                    if (!db.AcDomain.DbViews.TryGetDbView(db, id, out view))
                    {
                        throw new AnycmdException("意外的数据库视图" + id);
                    }
                    if (description == null && view.Description != null)
                    {
                        db.ExecuteNonQuery(dropProcName, CommandType.StoredProcedure,
                            new SqlParameter("name", propertyName),
                            new SqlParameter("level0type", "SCHEMA"),
                            new SqlParameter("level0name", view.SchemaName),
                            new SqlParameter("level1type", "VIEW"),
                            new SqlParameter("level1name", view.Name));
                    }
                    else if (view.Description == null)
                    {
                        db.ExecuteNonQuery(addProcName, CommandType.StoredProcedure,
                            new SqlParameter("name", propertyName),
                            new SqlParameter("value", description),
                            new SqlParameter("level0type", "SCHEMA"),
                            new SqlParameter("level0name", view.SchemaName),
                            new SqlParameter("level1type", "VIEW"),
                            new SqlParameter("level1name", view.Name));
                    }
                    else
                    {
                        db.ExecuteNonQuery(updateProcName, CommandType.StoredProcedure,
                            new SqlParameter("name", propertyName),
                            new SqlParameter("value", description),
                            new SqlParameter("level0type", "SCHEMA"),
                            new SqlParameter("level0name", view.SchemaName),
                            new SqlParameter("level1type", "VIEW"),
                            new SqlParameter("level1name", view.Name));
                    }
                    view.Description = description;
                    #endregion
                    break;
                case RDbMetaDataType.TableColumn:
                    #region TableColumn
                    DbTableColumn tableColumn;
                    if (!db.AcDomain.DbTableColumns.TryGetDbTableColumn(db, id, out tableColumn))
                    {
                        throw new AnycmdException("意外的数据库表列标识" + id);
                    }
                    if (description == null && tableColumn.Description != null)
                    {
                        db.ExecuteNonQuery(dropProcName, CommandType.StoredProcedure,
                            new SqlParameter("name", propertyName),
                            new SqlParameter("level0type", "SCHEMA"),
                            new SqlParameter("level0name", tableColumn.SchemaName),
                            new SqlParameter("level1type", "TABLE"),
                            new SqlParameter("level1name", tableColumn.TableName),
                            new SqlParameter("level2type", "COLUMN"),
                            new SqlParameter("level2name", tableColumn.Name));
                    }
                    else if (tableColumn.Description == null)
                    {
                        db.ExecuteNonQuery(addProcName, CommandType.StoredProcedure,
                            new SqlParameter("name", propertyName),
                            new SqlParameter("value", description),
                            new SqlParameter("level0type", "SCHEMA"),
                            new SqlParameter("level0name", tableColumn.SchemaName),
                            new SqlParameter("level1type", "TABLE"),
                            new SqlParameter("level1name", tableColumn.TableName),
                            new SqlParameter("level2type", "COLUMN"),
                            new SqlParameter("level2name", tableColumn.Name));
                    }
                    else
                    {
                        db.ExecuteNonQuery(updateProcName, CommandType.StoredProcedure,
                            new SqlParameter("name", propertyName),
                            new SqlParameter("value", description),
                            new SqlParameter("level0type", "SCHEMA"),
                            new SqlParameter("level0name", tableColumn.SchemaName),
                            new SqlParameter("level1type", "TABLE"),
                            new SqlParameter("level1name", tableColumn.TableName),
                            new SqlParameter("level2type", "COLUMN"),
                            new SqlParameter("level2name", tableColumn.Name));
                    }
                    tableColumn.Description = description;
                    #endregion
                    break;
                case RDbMetaDataType.ViewColumn:
                    #region ViewColumn
                    DbViewColumn viewColumn;
                    if (!db.AcDomain.DbViewColumns.TryGetDbViewColumn(db, id, out viewColumn))
                    {
                        throw new AnycmdException("意外的数据库视图列标识" + id);
                    }
                    if (description == null && viewColumn.Description != null)
                    {
                        db.ExecuteNonQuery(dropProcName, CommandType.StoredProcedure,
                            new SqlParameter("name", propertyName),
                            new SqlParameter("level0type", "SCHEMA"),
                            new SqlParameter("level0name", viewColumn.SchemaName),
                            new SqlParameter("level1type", "VIEW"),
                            new SqlParameter("level1name", viewColumn.ViewName),
                            new SqlParameter("level2type", "COLUMN"),
                            new SqlParameter("level2name", viewColumn.Name));
                    }
                    else if (viewColumn.Description == null)
                    {
                        db.ExecuteNonQuery(addProcName, CommandType.StoredProcedure,
                            new SqlParameter("name", propertyName),
                            new SqlParameter("value", description),
                            new SqlParameter("level0type", "SCHEMA"),
                            new SqlParameter("level0name", viewColumn.SchemaName),
                            new SqlParameter("level1type", "VIEW"),
                            new SqlParameter("level1name", viewColumn.ViewName),
                            new SqlParameter("level2type", "COLUMN"),
                            new SqlParameter("level2name", viewColumn.Name));
                    }
                    else
                    {
                        db.ExecuteNonQuery(updateProcName, CommandType.StoredProcedure,
                            new SqlParameter("name", propertyName),
                            new SqlParameter("value", description),
                            new SqlParameter("level0type", "SCHEMA"),
                            new SqlParameter("level0name", viewColumn.SchemaName),
                            new SqlParameter("level1type", "VIEW"),
                            new SqlParameter("level1name", viewColumn.ViewName),
                            new SqlParameter("level2type", "COLUMN"),
                            new SqlParameter("level2name", viewColumn.Name));
                    }
                    viewColumn.Description = description;
                    #endregion
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region private methods

        #region PopulateRDatabase
        private RDatabase PopulateRDatabase(IDataReader reader)
        {
            return new RDatabase
            {
                UserId = reader.GetString(reader.GetOrdinal("UserId")),
                RdbmsType = reader.GetString(reader.GetOrdinal("RdbmsType")),
                ProviderName = reader.GetString(reader.GetOrdinal("ProviderName")),
                Profile = reader.GetNullableString("Profile"),
                Password = reader.GetString(reader.GetOrdinal("Password")),
                IsTemplate = reader.GetBoolean(reader.GetOrdinal("IsTemplate")),
                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                Description = reader.GetNullableString("Description"),
                DataSource = reader.GetString(reader.GetOrdinal("DataSource")),
                CatalogName = reader.GetString(reader.GetOrdinal("CatalogName")),
                CreateUserId = reader.GetNullableGuid(reader.GetOrdinal("CreateUserId")),
                CreateBy = reader.GetNullableString(reader.GetOrdinal("CreateBy")),
                CreateOn = reader.GetNullableDateTime(reader.GetOrdinal("CreateOn"))
            };
        }
        #endregion

        #endregion
    }
}
