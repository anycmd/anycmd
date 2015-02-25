
namespace Anycmd.Ac.Queries.Ef
{
    using Anycmd.Ef;
    using Model;
    using Query;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using ViewModels.AccountViewModels;

    /// <summary>
    /// 查询接口实现<see cref="IVisitingLogQuery"/>
    /// </summary>
    public sealed class VisitingLogQuery : QueryBase, IVisitingLogQuery
    {
        public VisitingLogQuery(IAcDomain acDomain)
            : base(acDomain, "IdentityEntities")
        {
        }

        public List<DicReader> GetPlistVisitingLogTrs(string key, DateTime? leftVisitOn, DateTime? rightVisitOn, PagingInput paging)
        {
            paging.Valid();
            if (key != null)
            {
                key = key.Trim();
            }
            Func<SqlFilter> filter = () =>
            {
                var parameters = new List<SqlParameter>();
                var filterString = @" where a.LoginName like @key ";
                parameters.Add(new SqlParameter("key", "%" + key + "%"));
                if (leftVisitOn.HasValue)
                {
                    parameters.Add(new SqlParameter("leftVisitOn", leftVisitOn.Value));
                    filterString += " and a.VisitOn>=@leftVisitOn";
                }
                if (rightVisitOn.HasValue)
                {
                    parameters.Add(new SqlParameter("rightVisitOn", rightVisitOn.Value));
                    filterString += " and a.VisitOn<@rightVisitOn";
                }
                return new SqlFilter(filterString, parameters.ToArray());
            };
            return base.GetPlist("VisitingLog", filter, paging);
        }

        public List<DicReader> GetPlistVisitingLogTrs(Guid accountId, string loginName, DateTime? leftVisitOn, DateTime? rightVisitOn, PagingInput paging)
        {
            paging.Valid();
            loginName = (loginName ?? string.Empty).ToLower();
            Func<SqlFilter> filter = () =>
            {
                var parameters = new List<SqlParameter>();
                var filterString = @" where (a.AccountId=@AccountId or Lower(a.LoginName)=@LoginName) ";
                parameters.Add(new SqlParameter("LoginName", loginName));
                parameters.Add(new SqlParameter("AccountId", accountId));
                if (leftVisitOn.HasValue)
                {
                    parameters.Add(new SqlParameter("leftVisitOn", leftVisitOn.Value));
                    filterString += " and a.VisitOn>=@leftVisitOn";
                }
                if (rightVisitOn.HasValue)
                {
                    parameters.Add(new SqlParameter("rightVisitOn", rightVisitOn.Value));
                    filterString += " and a.VisitOn<@rightVisitOn";
                }
                return new SqlFilter(filterString, parameters.ToArray());
            };
            return base.GetPlist("VisitingLog", filter, paging);
        }
    }
}
