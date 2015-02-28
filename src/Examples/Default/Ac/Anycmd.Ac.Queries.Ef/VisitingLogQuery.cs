
namespace Anycmd.Ac.Queries.Ef
{
    using Anycmd.Ef;
    using Model;
    using Query;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
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
                var parameters = new List<DbParameter>();
                var filterString = @" where a.LoginName like @key ";
                parameters.Add(CreateParameter("key", "%" + key + "%", DbType.String));
                if (leftVisitOn.HasValue)
                {
                    parameters.Add(CreateParameter("leftVisitOn", leftVisitOn.Value, DbType.DateTime));
                    filterString += " and a.VisitOn>=@leftVisitOn";
                }
                if (rightVisitOn.HasValue)
                {
                    parameters.Add(CreateParameter("rightVisitOn", rightVisitOn.Value, DbType.DateTime));
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
                var parameters = new List<DbParameter>();
                var filterString = @" where (a.AccountId=@AccountId or Lower(a.LoginName)=@LoginName) ";
                parameters.Add(CreateParameter("LoginName", loginName, DbType.String));
                parameters.Add(CreateParameter("AccountId", accountId, DbType.Guid));
                if (leftVisitOn.HasValue)
                {
                    parameters.Add(CreateParameter("leftVisitOn", leftVisitOn.Value, DbType.DateTime));
                    filterString += " and a.VisitOn>=@leftVisitOn";
                }
                if (rightVisitOn.HasValue)
                {
                    parameters.Add(CreateParameter("rightVisitOn", rightVisitOn.Value, DbType.DateTime));
                    filterString += " and a.VisitOn<@rightVisitOn";
                }
                return new SqlFilter(filterString, parameters.ToArray());
            };
            return base.GetPlist("VisitingLog", filter, paging);
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
