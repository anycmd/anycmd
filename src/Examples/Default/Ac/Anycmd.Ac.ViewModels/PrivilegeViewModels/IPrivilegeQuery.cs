
namespace Anycmd.Ac.ViewModels.PrivilegeViewModels
{
    using Engine;
    using Query;
    using System.Collections.Generic;
    using ViewModel;

    /// <summary>
    /// 
    /// </summary>
    public interface IPrivilegeQuery : IQuery
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="organizationCode"></param>
        /// <param name="includeDescendants"></param>
        /// <param name="paging"></param>
        /// <returns></returns>
        List<DicReader> GetPlistOrganizationAccountTrs(string key, string organizationCode, bool includeDescendants, PagingInput paging);
    }
}
