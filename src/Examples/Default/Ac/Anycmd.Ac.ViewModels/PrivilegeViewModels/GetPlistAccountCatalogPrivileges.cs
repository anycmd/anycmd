
namespace Anycmd.Ac.ViewModels.PrivilegeViewModels
{
    using ViewModel;

    /// <summary>
    /// 
    /// </summary>
    public class GetPlistAccountCatalogPrivileges : GetPlistResult
    {
        /// <summary>
        /// 
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CatalogCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool? IncludeDescendants { get; set; }
    }
}
