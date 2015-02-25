
namespace Anycmd.Ac.ViewModels.AccountViewModels
{
    using Model;
    using Query;
    using System;
    using System.Collections.Generic;
    using ViewModel;

    /// <summary>
    /// 
    /// </summary>
    public interface IAccountQuery : IQuery
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="catalogCode"></param>
        /// <param name="includeDescendants"></param>
        /// <param name="paging"></param>
        /// <returns></returns>
        List<DicReader> GetPlistAccountTrs(List<FilterData> filters, string catalogCode, bool includeDescendants, PagingInput paging);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="roleId"></param>
        /// <param name="paging"></param>
        /// <returns></returns>
        List<DicReader> GetPlistRoleAccountTrs(string key, Guid roleId, PagingInput paging);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="groupId"></param>
        /// <param name="paging"></param>
        /// <returns></returns>
        List<DicReader> GetPlistGroupAccountTrs(string key, Guid groupId, PagingInput paging);
    }
}
