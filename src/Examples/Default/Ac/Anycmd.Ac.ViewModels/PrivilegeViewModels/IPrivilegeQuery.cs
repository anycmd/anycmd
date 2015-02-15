
namespace Anycmd.Ac.ViewModels.PrivilegeViewModels
{
    using Model;
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
        /// <param name="catalogCode"></param>
        /// <param name="includeDescendants"></param>
        /// <param name="paging"></param>
        /// <returns></returns>
        List<DicReader> GetPlistCatalogAccountTrs(string key, string catalogCode, bool includeDescendants, PagingInput paging);
    }
}
