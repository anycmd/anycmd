
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
    public interface IVisitingLogQuery : IQuery
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="leftVisitOn"></param>
        /// <param name="rightVisitOn"></param>
        /// <param name="paging"></param>
        /// <returns></returns>
        List<DicReader> GetPlistVisitingLogTrs(string key, DateTime? leftVisitOn, DateTime? rightVisitOn, PagingInput paging);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="loginName"></param>
        /// <param name="leftVisitOn"></param>
        /// <param name="rightVisitOn"></param>
        /// <param name="paging"></param>
        /// <returns></returns>
        List<DicReader> GetPlistVisitingLogTrs(Guid accountId, string loginName, DateTime? leftVisitOn, DateTime? rightVisitOn, PagingInput paging);
    }
}
