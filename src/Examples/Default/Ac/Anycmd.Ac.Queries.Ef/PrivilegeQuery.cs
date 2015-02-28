
namespace Anycmd.Ac.Queries.Ef
{
    using Anycmd.Ef;
    using Model;
    using Query;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using ViewModels.PrivilegeViewModels;

    /// <summary>
    /// 查询接口实现<see cref="IPrivilegeQuery"/>
    /// </summary>
    public sealed class PrivilegeQuery : QueryBase, IPrivilegeQuery
    {
        public PrivilegeQuery(IAcDomain acDomain)
            : base(acDomain, "AcEntities")
        {
        }

        public List<DicReader> GetPlistCatalogAccountTrs(string key, string catalogCode
            , bool includeDescendants, PagingInput paging)
        {
            paging.Valid();
            if (string.IsNullOrEmpty(catalogCode))
            {
                throw new ArgumentNullException("catalogCode");
            }
            Func<SqlFilter> filter = () =>
            {
                var parameters = new List<DbParameter>();
                var filterString = " where (a.Name like @key or a.Code like @key or a.LoginName like @key)";
                parameters.Add(CreateParameter("key", "%" + key + "%", DbType.String));
                if (!includeDescendants)
                {
                    parameters.Add(CreateParameter("CatalogCode", catalogCode, DbType.String));
                    filterString += " and a.CatalogCode=@CatalogCode";
                }
                else
                {
                    parameters.Add(CreateParameter("CatalogCode", catalogCode + "%", DbType.String));
                    filterString += " and a.CatalogCode like @CatalogCode";
                }
                return new SqlFilter(filterString, parameters.ToArray());
            };

            return base.GetPlist("CatalogAccountTr", filter, paging);
        }

        private DbParameter CreateParameter(string parameterName, object value, DbType dbType)
        {
            var p = base.DbContext.Rdb.CreateParameter();
            p.ParameterName = parameterName;
            p.Value = value;
            p.DbType = dbType;

            return p;
        }
    }
}
