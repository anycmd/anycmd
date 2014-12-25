
namespace Anycmd.Ef
{
    using Engine.Ac;
    using Exceptions;
    using Model;
    using Query;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.SqlClient;
    using System.Linq;
    using ViewModel;

    /// <summary>
    /// 
    /// </summary>
    public abstract class QueryBase : IQuery
    {
        private IEfFilterStringBuilder _filterStringBuilder;

        private readonly string _efDbContextName;
        private readonly IAcDomain _host;

        protected QueryBase(IAcDomain host, string efDbContextName)
        {
            this._host = host;
            this._efDbContextName = efDbContextName;
        }

        /// <summary>
        /// 
        /// </summary>
        protected DbContext DbContext
        {
            get
            {
                var repositoryContext = EfContext.Storage.GetRepositoryContext(this._efDbContextName);
                if (repositoryContext == null)
                {
                    repositoryContext = new EfRepositoryContext(_host, this._efDbContextName);
                    EfContext.Storage.SetRepositoryContext(repositoryContext);
                }
                return repositoryContext.DbContext;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity">实体模型类型参数</typeparam>
        /// <returns></returns>
        public IQueryable<TEntity> Query<TEntity>() where TEntity : class
        {
            return DbContext.Set<TEntity>();
        }

        /// <summary>
        /// 
        /// </summary>
        protected IEfFilterStringBuilder FilterStringBuilder
        {
            get
            {
                if (_filterStringBuilder == null)
                {
                    _filterStringBuilder = _host.GetRequiredService<IEfFilterStringBuilder>();
                }
                return _filterStringBuilder;
            }
        }

        public DicReader Get(string tableOrViewName, Guid id)
        {
            var sql = "select * from " + tableOrViewName + " as a where Id=@Id";
            using (var conn = DbContext.Database.Connection)
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.Add(new SqlParameter("Id", id));
                if (conn.State != System.Data.ConnectionState.Open)
                {
                    conn.Open();
                }
                using (var reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                {
                    if (!reader.Read()) return null;
                    var dic = new DicReader(_host);
                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        dic.Add(reader.GetName(i), reader.GetValue(i));
                    }
                    return dic;
                }
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filterCallback"></param>
        /// <param name="paging"></param>
        /// <returns></returns>
        protected IList<T> GetPlist<T>(Func<ObjectFilter> filterCallback, PagingInput paging) where T : class, IEntity
        {
            paging.Valid();
            string setName = typeof(T).Name + "s";
            ObjectFilter filter = ObjectFilter.Empty;
            if (filterCallback != null)
            {
                filter = filterCallback();
            }
            var queryString =
@"select value a from " + setName + " as a " + filter.FilterString + " order by a." + paging.SortField + " " + paging.SortOrder;
            var countQs =
@"select value a from " + setName + " as a " + filter.FilterString;

            IQueryable<T> countQuery;
            IQueryable<T> query;
            if (filter.Parameters != null)
            {
                countQuery = this.DbContext.CreateQuery<T>(countQs, filter.Parameters);
                query = this.DbContext.CreateQuery<T>(queryString, filter.Parameters)
                    .Skip(paging.SkipCount).Take(paging.PageSize);
            }
            else
            {
                countQuery = this.DbContext.CreateQuery<T>(countQs);
                query = this.DbContext.CreateQuery<T>(queryString)
                    .Skip(paging.SkipCount).Take(paging.PageSize);
            }

            paging.Total = countQuery.Count();

            return query.ToList<T>();
        }

        public List<DicReader> GetPlist(string tableOrViewName, Func<SqlFilter> filterCallback, PagingInput paging)
        {
            var filter = SqlFilter.Empty;
            if (filterCallback != null)
            {
                filter = filterCallback();
            }
            string sql =
@"SELECT TOP " + paging.PageSize + " * FROM (SELECT ROW_NUMBER() OVER(ORDER BY " + paging.SortField + " " + paging.SortOrder + ") AS RowNumber,* FROM " + tableOrViewName + " as a " + filter.FilterString + " ) a WHERE a.RowNumber > " + paging.PageIndex * paging.PageSize;
            string countSql =
@"SELECT count(1) FROM " + tableOrViewName + " as a " + filter.FilterString;
            var list = new List<DicReader>();
            using (var conn = DbContext.Database.Connection)
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                if (filter.Parameters != null)
                {
                    foreach (var item in filter.Parameters)
                    {
                        cmd.Parameters.Add(item);
                    }
                }
                if (conn.State != System.Data.ConnectionState.Open)
                {
                    conn.Open();
                }
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var dic = new DicReader(_host);
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            dic.Add(reader.GetName(i), reader.GetValue(i));
                        }
                        list.Add(dic);
                    }
                    reader.Close();
                    cmd.CommandText = countSql;
                    paging.Total = (int)cmd.ExecuteScalar();
                    conn.Close();
                }
                return list;
            }
        }

        public List<DicReader> GetPlist(EntityTypeState entityType, Func<SqlFilter> filterCallback, PagingInput paging)
        {
            if (string.IsNullOrEmpty(entityType.TableName))
            {
                throw new CoreException(entityType.Name + "未配置对应的数据库表");
            }
            return this.GetPlist(string.Format("[{0}]", entityType.TableName), filterCallback, paging);
        }
    }
}
