
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
	/// 查询接口实现<see cref="IAccountQuery"/>
	/// </summary>
	public sealed class AccountQuery : QueryBase, IAccountQuery
	{
		public AccountQuery(IAcDomain acDomain)
			: base(acDomain, "IdentityEntities")
		{
		}

		#region GetPlistAccountTrs
		public List<DicReader> GetPlistAccountTrs(List<FilterData> filters, string catalogCode, bool includeDescendants, PagingInput paging)
		{
			paging.Valid();
			bool byOrgCode = !string.IsNullOrEmpty(catalogCode);
			Func<SqlFilter> filter = () =>
			{
				List<DbParameter> parameters;
				var filterString = new SqlFilterStringBuilder().FilterString(base.DbContext.Rdb, filters, "a", out parameters);
				if (!string.IsNullOrEmpty(filterString))
				{
					filterString = " where 1=1 and " + filterString;
				}
				else
				{
					filterString = " where 1=1 ";
				}
				if (!includeDescendants)
				{
					if (byOrgCode)
					{
						if (!string.IsNullOrEmpty(catalogCode))
						{
							parameters.Add(this.CreateParameter("CatalogCode", catalogCode, DbType.String));
							filterString += "and a.CatalogCode=@CatalogCode ";
						}
					}
				}
				else
				{
					if (byOrgCode)
					{
						if (!string.IsNullOrEmpty(catalogCode))
						{
							parameters.Add(CreateParameter("CatalogCode", catalogCode + "%", DbType.String));
							filterString += "and a.CatalogCode like @CatalogCode ";
						}
					}
				}
				return new SqlFilter(filterString, parameters.ToArray());
			};
			return base.GetPlist("AccountTr", filter, paging);
		}
		#endregion

		#region GetPlistRoleAccountTrs
		public List<DicReader> GetPlistRoleAccountTrs(string key, Guid roleId, PagingInput paging)
		{
			paging.Valid();
			Func<SqlFilter> filter = () =>
			{
				var parameters = new List<DbParameter>();
				const string filterString = @" where (a.Name like @key
	or a.Code like @key
	or a.LoginName like @key) and a.RoleId=@RoleId";
				parameters.Add(CreateParameter("key", "%" + key + "%", DbType.String));
				parameters.Add(CreateParameter("RoleId", roleId, DbType.Guid));
				return new SqlFilter(filterString, parameters.ToArray());
			};
			return base.GetPlist("RoleAccountTr", filter, paging);
		}
		#endregion

		#region GetPlistGroupAccountTrs
		public List<DicReader> GetPlistGroupAccountTrs(string key, Guid groupId, PagingInput paging)
		{
			paging.Valid();
			Func<SqlFilter> filter = () =>
			{
				var parameters = new List<DbParameter>();
				const string filterString = @" where (a.Name like @key
	or a.Code like @key
	or a.LoginName like @key) and a.GroupId=@GroupId";
				parameters.Add(CreateParameter("key", "%" + key + "%", DbType.String));
				parameters.Add(CreateParameter("GroupId", groupId, DbType.Guid));
				return new SqlFilter(filterString, parameters.ToArray());
			};
			return base.GetPlist("GroupAccountTr", filter, paging);
		}
		#endregion

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
