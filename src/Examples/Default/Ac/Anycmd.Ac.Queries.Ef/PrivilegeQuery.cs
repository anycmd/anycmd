
namespace Anycmd.Ac.Queries.Ef
{
    using Anycmd.Ef;
    using Engine;
    using Query;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using ViewModels.PrivilegeViewModels;

    /// <summary>
    /// 查询接口实现<see cref="IPrivilegeQuery"/>
    /// </summary>
    public sealed class PrivilegeQuery : QueryBase, IPrivilegeQuery
    {
        public PrivilegeQuery(IAcDomain host)
            : base(host, "AcEntities")
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
                var parameters = new List<SqlParameter>();
                var filterString = " where (a.Name like @key or a.Code like @key or a.LoginName like @key)";
                parameters.Add(new SqlParameter("key", "%" + key + "%"));
                if (!includeDescendants)
                {
                    parameters.Add(new SqlParameter("CatalogCode", catalogCode));
                    filterString += " and a.CatalogCode=@CatalogCode";
                }
                else
                {
                    parameters.Add(new SqlParameter("CatalogCode", catalogCode + "%"));
                    filterString += " and a.CatalogCode like @CatalogCode";
                }
                return new SqlFilter(filterString, parameters.ToArray());
            };

            return base.GetPlist("CatalogAccountTr", filter, paging);
        }
    }
}
