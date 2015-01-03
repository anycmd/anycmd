
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

        public List<DicReader> GetPlistOrganizationAccountTrs(string key, string organizationCode
            , bool includeDescendants, PagingInput paging)
        {
            paging.Valid();
            if (string.IsNullOrEmpty(organizationCode))
            {
                throw new ArgumentNullException("organizationCode");
            }
            Func<SqlFilter> filter = () =>
            {
                var parameters = new List<SqlParameter>();
                var filterString = " where (a.Name like @key or a.Code like @key or a.LoginName like @key)";
                parameters.Add(new SqlParameter("key", "%" + key + "%"));
                if (!includeDescendants)
                {
                    parameters.Add(new SqlParameter("OrganizationCode", organizationCode));
                    filterString += " and a.OrganizationCode=@OrganizationCode";
                }
                else
                {
                    parameters.Add(new SqlParameter("OrganizationCode", organizationCode + "%"));
                    filterString += " and a.OrganizationCode like @OrganizationCode";
                }
                return new SqlFilter(filterString, parameters.ToArray());
            };

            return base.GetPlist("OrganizationAccountTr", filter, paging);
        }
    }
}
