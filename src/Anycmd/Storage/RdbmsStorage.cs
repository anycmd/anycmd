
namespace Anycmd.Storage
{
    using Builders;
    using Model;
    using Specifications;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// 表示一个传统的关系数据仓库。
    /// </summary>
    public abstract class RdbmsStorage : DisposableObject, IStorage
    {
        #region Private Fields
        private volatile bool _committed = true;
        private readonly string _connectionString;
        private readonly IStorageMappingResolver _mappingResolver;
        private readonly Dictionary<Type, object> _whereClauseBuilders = new Dictionary<Type, object>();
        private readonly List<Tuple<Type, PropertyBag>> _newObjectSpecs = new List<Tuple<Type, PropertyBag>>();
        private readonly List<Tuple<Type, object>> _deletedObjectSpecs = new List<Tuple<Type, object>>();
        private readonly List<Tuple<Type, PropertyBag, object>> _modifiedObjectSpecs = new List<Tuple<Type, PropertyBag, object>>();
        #endregion

        #region Ctor
        /// <summary>
        /// 初始化一个 <c>RdbmsStorage</c> 类型的对象。
        /// </summary>
        /// <param name="connectionString">连接字符串。</param>
        /// <param name="mappingResolver">仓库映射转换器。</param>
        protected RdbmsStorage(string connectionString, IStorageMappingResolver mappingResolver)
        {
            this._mappingResolver = mappingResolver;
            this._connectionString = connectionString;
        }
        #endregion

        #region Private Methods
        private MethodInfo MakeGenericInsertMethod(Type genericType)
        {
            var insertMethod = this.GetType().GetMethod("DoInsert", BindingFlags.NonPublic | BindingFlags.Instance);
            if (insertMethod == null)
                throw new InvalidOperationException("Cannot locate the DoInsert method.");
            return insertMethod.MakeGenericMethod(genericType);
        }

        private MethodInfo MakeGenericUpdateMethod(Type genericType)
        {
            var updateMethod = this.GetType().GetMethod("DoUpdate", BindingFlags.NonPublic | BindingFlags.Instance);
            if (updateMethod == null)
                throw new InvalidOperationException("Cannot locate the DoUpdate method.");
            return updateMethod.MakeGenericMethod(genericType);
        }

        private MethodInfo MakeGenericDeleteMethod(Type genericType)
        {
            var deleteMethod = this.GetType().GetMethod("DoDelete", BindingFlags.NonPublic | BindingFlags.Instance);
            if (deleteMethod == null)
                throw new InvalidOperationException("Cannot locate the DoDelete method.");
            return deleteMethod.MakeGenericMethod(genericType);
        }
        #endregion

        #region Protected Properties
        /// <summary>
        /// 读取连接字符串。
        /// </summary>
        protected string ConnectionString
        {
            get { return this._connectionString; }
        }

        /// <summary>
        ///读取仓库映射转换器。
        /// </summary>
        protected IStorageMappingResolver MappingResolver
        {
            get { return this._mappingResolver; }
        }

        /// <summary>
        /// 读取事务锁行为 <see cref="System.Data.IsolationLevel"/> 值。
        /// </summary>
        protected virtual IsolationLevel IsolationLevel
        {
            get
            {
                return IsolationLevel.Unspecified;
            }
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// 创建一个给定对象类型的where子句建造器。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <returns>where子句建造器实例。</returns>
        protected abstract WhereClauseBuilder<T> CreateWhereClauseBuilder<T>() where T : class, new();

        /// <summary>
        /// 获取给定对象类型的where子句建造器。
        /// </summary>
        /// <typeparam name="T">给定的对象类型。</typeparam>
        /// <returns>where子句建造器实例。</returns>
        protected WhereClauseBuilder<T> GetWhereClauseBuilder<T>() where T : class, new()
        {
            WhereClauseBuilder<T> whereClauseBuilder = null;
            if (!_whereClauseBuilders.ContainsKey(typeof(T)))
            {
                whereClauseBuilder = CreateWhereClauseBuilder<T>();
                _whereClauseBuilders.Add(typeof(T), whereClauseBuilder);
            }
            else
            {
                whereClauseBuilder = _whereClauseBuilders[typeof(T)] as WhereClauseBuilder<T>;
            }
            return whereClauseBuilder;
        }

        /// <summary>
        /// 创建数据库连接对象。
        /// </summary>
        /// <returns>The <see cref="System.Data.Common.DbConnection"/> instance which represents
        /// the open connection to the relational database.</returns>
        protected abstract DbConnection CreateDatabaseConnection();

        /// <summary>
        /// Creates a database parameter object.
        /// </summary>
        /// <returns>The instance of database parameter object.</returns>
        protected abstract DbParameter CreateParameter();

        /// <summary>
        /// Creates a instance of the command object.
        /// </summary>
        /// <param name="sql">The SQL statement used for creating the command object.</param>
        /// <param name="connection">The <see cref="System.Data.Common.DbConnection"/> which represents
        /// the database connection.</param>
        /// <returns>The instance of the command object.</returns>
        protected abstract DbCommand CreateCommand(string sql, DbConnection connection);

        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="disposing">A <see cref="System.Boolean"/> value which indicates whether
        /// the object should be disposed explicitly.</param>
        protected override void Dispose(bool disposing) { }

        #region Select Utilities
        /// <summary>
        /// Gets a list of database parameters for constructing the selection criteria clause.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="parameterValues">The <c>Dictionary&lt;string, object&gt;</c> instance which contains the criteria.</param>
        /// <returns>The list of database parameters.</returns>
        protected virtual List<DbParameter> GetSelectCriteriaDbParameterList<T>(Dictionary<string, object> parameterValues)
            where T : class, new()
        {
            var collection = new List<DbParameter>();
            foreach (var kvp in parameterValues)
            {
                var param = CreateParameter();
                param.ParameterName = kvp.Key;
                param.Value = kvp.Value;
                collection.Add(param);
            }
            return collection;
        }
        /// <summary>
        /// Gets the field list for sorting.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="orders">The <c>PropertyBag</c> instance which contains the fields for sorting.</param>
        /// <returns>The sorting field list, commonly, the ORDER BY clause.</returns>
        protected virtual string GetOrderByFieldList<T>(PropertyBag orders)
            where T : class, new()
        {
            return GetFieldNameList<T>(orders);
        }
        #endregion

        #region Table Utility
        /// <summary>
        /// Gets the table name.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <returns>The table name.</returns>
        protected virtual string GetTableName<T>()
            where T : class, new()
        {
            return _mappingResolver.ResolveTableName<T>();
        }
        #endregion

        #region Field And Parameter Utility
        /// <summary>
        /// Gets the name of the field by using the specified property name.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="propertyName">The property name.</param>
        /// <returns>The field name in relational database tables.</returns>
        protected virtual string GetFieldName<T>(string propertyName)
            where T : class, new()
        {
            return _mappingResolver.ResolveFieldName<T>(propertyName);
        }
        /// <summary>
        /// Gets the list of the field names, separated by the comma character.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <returns>The field names, separated by the comma character.</returns>
        protected virtual string GetFieldNameList<T>()
            where T : class, new()
        {
            return string.Join(", ", typeof(T).GetProperties(PropertyBag.PropertyBagBindingFlags).Select(p => _mappingResolver.ResolveFieldName<T>(p.Name)).ToArray());
        }
        /// <summary>
        /// Gets the list of the field names, separated by the comma character, by using the
        /// specified fields.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="allFields">The <c>PropertyBag</c> object that contains the specified fields.</param>
        /// <returns>The field names, separated by the comma character.</returns>
        protected virtual string GetFieldNameList<T>(PropertyBag allFields)
            where T : class, new()
        {
            return string.Join(", ", allFields.Select(p => _mappingResolver.ResolveFieldName<T>(p.Key)).ToArray());
        }
        #endregion

        #region Insert Utility
        /// <summary>
        /// Gets a comma separated list of the field names for INSERT operation, by using the specified
        /// fields.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="fields">The <c>PropertyBag</c> instance which contains the fields.</param>
        /// <returns>The comma separated list of the field names for INSERT operation.</returns>
        /// <remarks>The auto-generated identity fields will be omitted.</remarks>
        protected virtual string GetInsertFieldNameList<T>(PropertyBag fields)
            where T : class, new()
        {
            return string.Join(", ", fields.Where(kvp => !_mappingResolver.IsAutoIdentityField<T>(kvp.Key)).Select(kvp => _mappingResolver.ResolveFieldName<T>(kvp.Key)).ToArray());
        }
        /// <summary>
        /// Gets a comma separated list of the field parameter names for INSERT operation, by using the
        /// specified fields.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="fields">The <c>PropertyBag</c> object which contains the fields.</param>
        /// <returns>The comma separated list of the field parameter names for INSERT operation.</returns>
        /// <remarks>The auto-generated identity fields will be omitted.</remarks>
        protected virtual string GetInsertParameterNameList<T>(PropertyBag fields)
            where T : class, new()
        {
            return string.Join(", ", fields.Where(kvp => !_mappingResolver.IsAutoIdentityField<T>(kvp.Key)).Select(kvp => string.Format("{0}{1}", GetWhereClauseBuilder<T>().ParameterChar, _mappingResolver.ResolveFieldName<T>(kvp.Key).ToLower())).ToArray());
        }
        /// <summary>
        /// Gets a list of database parameters used by the INSERT operation, by using the specified
        /// fields.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="fields">The <c>PropertyBag</c> object which contains the fields.</param>
        /// <returns>The list of database parameters.</returns>
        protected virtual List<DbParameter> GetInsertDbParameterList<T>(PropertyBag fields)
            where T : class, new()
        {
            var parameters = new List<DbParameter>();
            fields.ToList().ForEach(kvp =>
            {
                if (_mappingResolver.IsAutoIdentityField<T>(kvp.Key)) return;
                var param = CreateParameter();
                param.ParameterName = string.Format("{0}{1}",
                    GetWhereClauseBuilder<T>().ParameterChar,
                    _mappingResolver.ResolveFieldName<T>(kvp.Key).ToLower());
                param.Value = kvp.Value;
                parameters.Add(param);
            }
            );
            return parameters;
        }
        #endregion

        #region Update Utility
        /// <summary>
        /// Creates the <see cref="System.Data.Common.DbParameter"/> list which contains the parameter
        /// definitions to be used in the UPDATE WHERE clause.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="parameterValues">The parameter values.</param>
        /// <returns>A list of <see cref="System.Data.Common.DbParameter"/> objects.</returns>
        protected virtual List<DbParameter> GetUpdateCriteriaDbParameterList<T>(Dictionary<string, object> parameterValues)
            where T : class, new()
        {
            var parameters = new List<DbParameter>();
            parameterValues.ToList().ForEach(kvp =>
            {
                var p = CreateParameter();
                p.ParameterName = kvp.Key;
                p.Value = kvp.Value;
                parameters.Add(p);
            });
            return parameters;
        }
        /// <summary>
        /// Gets a comma separated <see cref="System.String"/> value which represents the fields
        /// that needs to be updated to the new values.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="newValues">The <c>PropertyBag</c> object which contains the fields and their
        /// new values to be updated.</param>
        /// <returns>A comma separated <see cref="System.String"/> value which represents the fields.</returns>
        protected virtual string GetUpdateFieldList<T>(PropertyBag newValues)
            where T : class, new()
        {
            return string.Join(", ", newValues.Where(kvp => !_mappingResolver.IsAutoIdentityField<T>(kvp.Key)).Select(p => string.Format("{0}={1}u_{2}", _mappingResolver.ResolveFieldName<T>(p.Key), GetWhereClauseBuilder<T>().ParameterChar, _mappingResolver.ResolveFieldName<T>(p.Key).ToLower())).ToArray());
        }
        /// <summary>
        /// Gets the database parameters that are used for update the fields in UPDATE clause.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="newValues">The <c>PropertyBag</c> object which contains the fields and their
        /// new values to be updated.</param>
        /// <returns>The database parameters.</returns>
        protected virtual List<DbParameter> GetUpdateDbParameterList<T>(PropertyBag newValues)
            where T : class, new()
        {
            var collection = new List<DbParameter>();
            foreach (var kvp in newValues)
            {
                if (_mappingResolver.IsAutoIdentityField<T>(kvp.Key))
                    continue;
                var param = CreateParameter();
                param.ParameterName = string.Format("{0}u_{1}", GetWhereClauseBuilder<T>().ParameterChar, _mappingResolver.ResolveFieldName<T>(kvp.Key).ToLower());
                param.Value = kvp.Value;
                collection.Add(param);
            }
            return collection;
        }
        #endregion

        #region Delete Utility
        /// <summary>
        /// Creates the <see cref="System.Data.Common.DbParameter"/> list which contains the parameter
        /// definitions to be used in the DELETE WHERE clause.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="parameterValues">The parameter values.</param>
        /// <returns>A list of <see cref="System.Data.Common.DbParameter"/> objects.</returns>
        protected virtual List<DbParameter> GetDeleteDbParameterList<T>(Dictionary<string, object> parameterValues)
            where T : class, new()
        {
            var parameters = new List<DbParameter>();
            parameterValues.ToList().ForEach(kvp =>
            {
                var p = CreateParameter();
                p.ParameterName = kvp.Key;
                p.Value = kvp.Value;
                parameters.Add(p);
            });
            return parameters;
        }
        #endregion

        #region CreateFromReader Utility
        /// <summary>
        /// Creates the data object instance from a <see cref="System.Data.Common.DbDataReader"/> instance.
        /// </summary>
        /// <typeparam name="T">The type of the object to be created.</typeparam>
        /// <param name="reader">The <see cref="System.Data.Common.DbDataReader"/> instance.</param>
        /// <returns>The data object instance.</returns>
        protected virtual T CreateFromReader<T>(DbDataReader reader)
            where T : class, new()
        {
            var t = new T();

            typeof(T)
                .GetProperties(PropertyBag.PropertyBagBindingFlags)
                .ToList()
                .ForEach(pi =>
                {
                    // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
                    if (pi.PropertyType == typeof(Guid))
                        pi.SetValue(t, new Guid(Convert.ToString(reader[_mappingResolver.ResolveFieldName<T>(pi.Name)])), null);
                    else
                        pi.SetValue(t, reader[_mappingResolver.ResolveFieldName<T>(pi.Name)], null);
                });

            return t;
        }
        #endregion

        #region Delegation Methods
        /// <summary>
        /// Gets the number of records existing in the current storage.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="specification">The instance of <see cref="Anycmd.Specifications.ISpecification&lt;T&gt;"/>
        /// which represents the filter criteria.</param>
        /// <param name="connection">The instance of <see cref="System.Data.Common.DbConnection"/> which represents
        /// the connection to a database.</param>
        /// <param name="transaction">The instance of <see cref="System.Data.Common.DbTransaction"/> which represents
        /// the database transaction.</param>
        /// <returns>The number of records.</returns>
        protected virtual int DoGetRecordCount<T>(ISpecification<T> specification, DbConnection connection, DbTransaction transaction = null)
            where T : class, new()
        {
            int result = 0;
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if (connection.State != ConnectionState.Open)
            {
                throw new ArgumentException(@"The database connection is not in an Open state.", "connection");
            }
            WhereClauseBuildResult whereBuildResult = null;
            var sql = string.Format("SELECT COUNT(*) FROM {0}", GetTableName<T>());
            if (specification != null)
            {
                var expression = specification.GetExpression();
                whereBuildResult = GetWhereClauseBuilder<T>().BuildWhereClause(expression);
                sql += " WHERE " + whereBuildResult.WhereClause;
            }
            using (var command = this.CreateCommand(sql, connection))
            {
                if (transaction != null)
                    command.Transaction = transaction;
                if (specification != null)
                {
                    command.Parameters.Clear();
                    var parameters = GetSelectCriteriaDbParameterList<T>(whereBuildResult.ParameterValues);
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.Add(parameter);
                    }
                }
                result = Convert.ToInt32(command.ExecuteScalar());
            }
            return result;
        }
        /// <summary>
        /// Selects the first-only object from the current storage.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="specification">The instance of <see cref="Anycmd.Specifications.ISpecification&lt;T&gt;"/>
        /// which represents the filter criteria.</param>
        /// <param name="connection">The instance of <see cref="System.Data.Common.DbConnection"/> which represents
        /// the connection to a database.</param>
        /// <param name="transaction">The instance of <see cref="System.Data.Common.DbTransaction"/> which represents
        /// the database transaction.</param>
        /// <returns>The first-only object that exists in the current storage.</returns>
        protected virtual T DoSelectFirstOnly<T>(ISpecification<T> specification, DbConnection connection, DbTransaction transaction = null)
            where T : class, new()
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            if (connection.State != ConnectionState.Open)
            {
                throw new ArgumentException(@"The database connection is not in an Open state.", "connection");
            }
            WhereClauseBuildResult whereBuildResult = null;
            var sql = string.Format("SELECT {0} FROM {1}",
                GetFieldNameList<T>(), GetTableName<T>());
            if (specification != null)
            {
                var expression = specification.GetExpression();
                whereBuildResult = GetWhereClauseBuilder<T>().BuildWhereClause(expression);
                sql += " WHERE " + whereBuildResult.WhereClause;
            }
            using (var command = CreateCommand(sql, connection))
            {
                if (transaction != null)
                    command.Transaction = transaction;
                if (specification != null)
                {
                    command.Parameters.Clear();
                    var parameters = GetSelectCriteriaDbParameterList<T>(whereBuildResult.ParameterValues);
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.Add(parameter);
                    }
                }
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    var ret = CreateFromReader<T>(reader);
                    reader.Close(); // Very important: reader MUST be closed !!!
                    return ret;
                }
                reader.Close(); // Very important: reader MUST be closed !!!
                return null;
            }
        }
        /// <summary>
        /// Selects all the object from the current storage.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="specification">The instance of <see cref="Anycmd.Specifications.ISpecification&lt;T&gt;"/>
        /// which represents the filter criteria.</param>
        /// <param name="orders">The <see cref="Anycmd.Storage.PropertyBag"/> which represents the fields for sorting.</param>
        /// <param name="sortOrder">The <see cref="Anycmd.Storage.SortOrder"/> value which represents the sort order.</param>
        /// <param name="connection">The instance of <see cref="System.Data.Common.DbConnection"/> which represents
        /// the connection to a database.</param>
        /// <param name="transaction">The instance of <see cref="System.Data.Common.DbTransaction"/> which represents
        /// the database transaction.</param>
        /// <returns>All the objects selected.</returns>
        protected virtual IEnumerable<T> DoSelect<T>(ISpecification<T> specification, PropertyBag orders, SortOrder sortOrder,
            DbConnection connection, DbTransaction transaction = null)
            where T : class, new()
        {
            if (connection == null)
                throw new ArgumentNullException("connection");
            if (connection.State != ConnectionState.Open)
                throw new ArgumentException(@"The database connection is not in an Open state.", "connection");
            WhereClauseBuildResult whereBuildResult = null;
            var sql = string.Format("SELECT {0} FROM {1}",
                GetFieldNameList<T>(), GetTableName<T>());
            if (specification != null)
            {
                var expression = specification.GetExpression();
                whereBuildResult = GetWhereClauseBuilder<T>().BuildWhereClause(expression);
                sql += " WHERE " + whereBuildResult.WhereClause;
            }
            if (orders != null && sortOrder != SortOrder.Unspecified)
            {
                sql += " ORDER BY " + GetOrderByFieldList<T>(orders);
                switch (sortOrder)
                {
                    case SortOrder.Ascending:
                        sql += " ASC";
                        break;
                    case SortOrder.Descending:
                        sql += " DESC";
                        break;
                    default: break;
                }
            }
            using (var command = CreateCommand(sql, connection))
            {
                if (transaction != null)
                    command.Transaction = transaction;
                if (specification != null)
                {
                    command.Parameters.Clear();
                    var parameters = GetSelectCriteriaDbParameterList<T>(whereBuildResult.ParameterValues);
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.Add(parameter);
                    }
                }
                var reader = command.ExecuteReader();
                var ret = new List<T>();
                while (reader.Read())
                {
                    ret.Add(CreateFromReader<T>(reader));
                }
                reader.Close(); // Very important: reader MUST be closed !!!
                return ret;
            }
        }
        /// <summary>
        /// Inserts an object into current storage.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="allFields">The fields to be inserted.</param>
        /// <param name="connection">The instance of <see cref="System.Data.Common.DbConnection"/> which represents
        /// the connection to a database.</param>
        /// <param name="transaction">The instance of <see cref="System.Data.Common.DbTransaction"/> which represents
        /// the database transaction.</param>
        protected virtual void DoInsert<T>(PropertyBag allFields, DbConnection connection, DbTransaction transaction = null)
            where T : class, new()
        {
            if (connection == null)
                throw new ArgumentNullException("connection");
            if (connection.State != ConnectionState.Open)
                throw new ArgumentException(@"The database connection is not in an Open state.", "connection");

            var sql = string.Format("INSERT INTO {0} ({1}) VALUES ({2})",
                GetTableName<T>(), GetInsertFieldNameList<T>(allFields), GetInsertParameterNameList<T>(allFields));
            using (var command = CreateCommand(sql, connection))
            {
                if (transaction != null)
                    command.Transaction = transaction;
                command.Parameters.Clear();
                var parameters = GetInsertDbParameterList<T>(allFields);
                foreach (var param in parameters)
                {
                    command.Parameters.Add(param);
                }
                command.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Deletes an object from current storage.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="specification">The instance of <see cref="Anycmd.Specifications.ISpecification&lt;T&gt;"/>
        /// which represents the filter criteria.</param>
        /// <param name="connection">The instance of <see cref="System.Data.Common.DbConnection"/> which represents
        /// the connection to a database.</param>
        /// <param name="transaction">The instance of <see cref="System.Data.Common.DbTransaction"/> which represents
        /// the database transaction.</param>
        protected virtual void DoDelete<T>(ISpecification<T> specification, DbConnection connection, DbTransaction transaction = null)
            where T : class, new()
        {
            if (connection == null)
                throw new ArgumentNullException("connection");
            if (connection.State != ConnectionState.Open)
                throw new ArgumentException(@"The database connection is not in an Open state.", "connection");
            WhereClauseBuildResult whereBuildResult = null;
            var sql = string.Format("DELETE FROM {0}",
                GetTableName<T>());
            if (specification != null)
            {
                var expression = specification.GetExpression();
                whereBuildResult = GetWhereClauseBuilder<T>().BuildWhereClause(expression);
                sql += " WHERE " + whereBuildResult.WhereClause;
            }
            using (var command = CreateCommand(sql, connection))
            {
                if (transaction != null)
                    command.Transaction = transaction;
                if (specification != null)
                {
                    command.Parameters.Clear();
                    var parameters = GetDeleteDbParameterList<T>(whereBuildResult.ParameterValues);
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.Add(parameter);
                    }
                }
                command.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Updates the object with new values.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="newValues">The <see cref="Anycmd.Storage.PropertyBag"/> which contains the new values.</param>
        /// <param name="specification">The instance of <see cref="Anycmd.Specifications.ISpecification&lt;T&gt;"/>
        /// which represents the filter criteria.</param>
        /// <param name="connection">The instance of <see cref="System.Data.Common.DbConnection"/> which represents
        /// the connection to a database.</param>
        /// <param name="transaction">The instance of <see cref="System.Data.Common.DbTransaction"/> which represents
        /// the database transaction.</param>
        protected virtual void DoUpdate<T>(PropertyBag newValues, ISpecification<T> specification, DbConnection connection, DbTransaction transaction = null)
            where T : class, new()
        {
            if (connection == null)
                throw new ArgumentNullException("connection");
            if (connection.State != ConnectionState.Open)
                throw new ArgumentException(@"The database connection is not in an Open state.", "connection");
            WhereClauseBuildResult whereBuildResult = null;
            var sql = string.Format("UPDATE {0} SET {1}",
                GetTableName<T>(), GetUpdateFieldList<T>(newValues));
            if (specification != null)
            {
                var expression = specification.GetExpression();
                whereBuildResult = GetWhereClauseBuilder<T>().BuildWhereClause(expression);
                sql += " WHERE " + whereBuildResult.WhereClause;
            }
            using (var command = CreateCommand(sql, connection))
            {
                if (transaction != null)
                    command.Transaction = transaction;
                command.Parameters.Clear();
                var updateParams = GetUpdateDbParameterList<T>(newValues);
                foreach (var updateParam in updateParams)
                {
                    command.Parameters.Add(updateParam);
                }
                if (specification != null)
                {
                    var criParams = GetUpdateCriteriaDbParameterList<T>(whereBuildResult.ParameterValues);
                    foreach (var criParam in criParams)
                    {
                        command.Parameters.Add(criParam);
                    }
                }
                command.ExecuteNonQuery();
            }
        }
        #endregion

        #endregion

        #region IStorage Members
        /// <summary>
        /// Gets the first only object from storage.
        /// </summary>
        /// <typeparam name="T">The type of the object to get.</typeparam>
        /// <returns>The instance of the object.</returns>
        public T SelectFirstOnly<T>()
            where T : class, new()
        {
            return SelectFirstOnly<T>(specification: null);
        }
        /// <summary>
        /// Gets the first-only object from the storage by given specification.
        /// </summary>
        /// <typeparam name="T">The type of the object to get.</typeparam>
        /// <param name="specification">The specification.</param>
        /// <returns>The instance of the object.</returns>
        public T SelectFirstOnly<T>(ISpecification<T> specification)
            where T : class, new()
        {
            T result = null;
            using (var connection = this.CreateDatabaseConnection())
            {
                connection.Open();
                result = DoSelectFirstOnly<T>(specification, connection);
                connection.Close();
            }
            return result;
        }
        /// <summary>
        /// Gets the number of records in the storage.
        /// </summary>
        /// <typeparam name="T">The type of the object to get.</typeparam>
        /// <returns>The number of records in the storage.</returns>
        public int GetRecordCount<T>()
            where T : class, new()
        {
            return this.GetRecordCount<T>(specification: null);
        }
        /// <summary>
        /// Gets the number of records in the storage.
        /// </summary>
        /// <typeparam name="T">The type of the object to get.</typeparam>
        /// <param name="specification">The specification.</param>
        /// <returns>The number of records in the storage.</returns>
        public int GetRecordCount<T>(ISpecification<T> specification)
            where T : class, new()
        {
            var ret = 0;
            using (var connection = this.CreateDatabaseConnection())
            {
                connection.Open();
                ret = this.DoGetRecordCount<T>(specification, connection);
                connection.Close();
            }
            return ret;
        }
        /// <summary>
        /// Gets a list of all objects from storage.
        /// </summary>
        /// <typeparam name="T">The type of the object to get.</typeparam>
        /// <returns>A list of the objects.</returns>
        public IEnumerable<T> Select<T>()
            where T : class, new()
        {
            return Select<T>(specification: null);
        }
        /// <summary>
        /// Gets a list of objects from storage by given specification.
        /// </summary>
        /// <typeparam name="T">The type of the object to get.</typeparam>
        /// <param name="specification">The specification.</param>
        /// <returns>A list of the objects.</returns>
        public IEnumerable<T> Select<T>(ISpecification<T> specification)
            where T : class, new()
        {

            return Select<T>(specification, null, SortOrder.Unspecified);
        }
        /// <summary>
        /// Gets a list of ordered objects from storage by given specification.
        /// </summary>
        /// <typeparam name="T">The type of the object to get.</typeparam>
        /// <param name="specification">The specification.</param>
        /// <param name="orders">The <c>PropertyBag</c> instance which contains the ordering fields.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <returns>A list of ordered objects.</returns>
        public IEnumerable<T> Select<T>(ISpecification<T> specification, PropertyBag orders, Storage.SortOrder sortOrder)
            where T : class, new()
        {
            IEnumerable<T> result = null;
            using (var connection = this.CreateDatabaseConnection())
            {
                connection.Open();
                result = DoSelect<T>(specification, orders, sortOrder, connection);
                connection.Close();
            }
            return result;
        }

        /// <summary>
        /// Inserts the object into the storage.
        /// </summary>
        /// <typeparam name="T">The type of the object to be inserted.</typeparam>
        /// <param name="allFields">The <c>PropertyBag</c> instance which contains the properties and property values
        /// to be inserted.</param>
        public void Insert<T>(PropertyBag allFields)
            where T : class, new()
        {
            //newObjectSpecs.Add(typeof(T), allFields);
            _newObjectSpecs.Add(Tuple.Create<Type, PropertyBag>(typeof(T), allFields));
            _committed = false;
        }
        /// <summary>
        /// Deletes all objects from storage.
        /// </summary>
        /// <typeparam name="T">The type of the object to be deleted.</typeparam>
        public void Delete<T>()
            where T : class, new()
        {
            Delete<T>(specification: null);
            _committed = false;
        }
        /// <summary>
        /// Deletes specified objects from storage.
        /// </summary>
        /// <typeparam name="T">The type of the object to be deleted.</typeparam>
        /// <param name="specification">The specification.</param>
        public void Delete<T>(ISpecification<T> specification)
            where T : class, new()
        {
            //deletedObjectSpecs.Add(typeof(T), specification);
            _deletedObjectSpecs.Add(Tuple.Create<Type, object>(typeof(T), specification));
            _committed = false;
        }

        /// <summary>
        /// Updates all the objects in storage with the given values.
        /// </summary>
        /// <typeparam name="T">The type of the object to be updated.</typeparam>
        /// <param name="newValues">The <c>PropertyBag</c> instance which contains the properties and property values
        /// to be updated.</param>
        public void Update<T>(PropertyBag newValues)
            where T : class, new()
        {
            Update<T>(newValues, specification: null);
        }
        /// <summary>
        /// Updates all the objects in storage with the given values and the specification.
        /// </summary>
        /// <typeparam name="T">The type of the object to be updated.</typeparam>
        /// <param name="newValues">The <c>PropertyBag</c> instance which contains the properties and property values
        /// to be updated.</param>
        /// <param name="specification">The specification.</param>
        public void Update<T>(PropertyBag newValues, ISpecification<T> specification)
            where T : class, new()
        {
            //var spec = Tuple.Create<PropertyBag, object>(newValues, specification);
            //modifiedObjectSpecs.Add(typeof(T), spec);
            _modifiedObjectSpecs.Add(Tuple.Create<Type, PropertyBag, object>(typeof(T), newValues, specification));
            _committed = false;
        }
        #endregion

        #region IUnitOfWork Members
        /// <summary>
        /// Gets a <see cref="System.Boolean"/> value which indicates
        /// whether the Unit of Work supports MS-DTC.
        /// </summary>
        public abstract bool DistributedTransactionSupported { get; }
        /// <summary>
        /// Gets a <see cref="System.Boolean"/> value which indicates
        /// whether the Unit of Work was successfully committed.
        /// </summary>
        public bool Committed
        {
            get { return this._committed; }
        }
        /// <summary>
        /// Commits the transaction.
        /// </summary>
        public virtual void Commit()
        {
            using (var connection = this.CreateDatabaseConnection())
            {
                try
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction(this.IsolationLevel))
                    {
                        try
                        {
                            // insert...
                            foreach (var item in _newObjectSpecs)
                            {
                                var type = item.Item1;
                                var propertyBag = item.Item2;
                                var insertMethod = MakeGenericInsertMethod(type);
                                insertMethod.Invoke(this, new object[] { propertyBag, connection, transaction });
                            }
                            // update...
                            foreach (var item in _modifiedObjectSpecs)
                            {
                                var type = item.Item1;
                                var propertyBag = item.Item2;
                                var specification = item.Item3;
                                var updateMethod = MakeGenericUpdateMethod(type);
                                updateMethod.Invoke(this, new object[] { propertyBag, specification, connection, transaction });
                            }
                            // delete...
                            foreach (var item in _deletedObjectSpecs)
                            {
                                var type = item.Item1;
                                var specification = item.Item2;
                                var deleteMethod = MakeGenericDeleteMethod(type);
                                deleteMethod.Invoke(this, new object[] { specification, connection, transaction });
                            }
                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
            _newObjectSpecs.Clear();
            _modifiedObjectSpecs.Clear();
            _deletedObjectSpecs.Clear();
            _committed = true;
        }
        /// <summary>
        /// Rollback the transaction.
        /// </summary>
        public virtual void Rollback()
        {
            _committed = false;
        }
        #endregion
    }
}
