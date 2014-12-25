
namespace Anycmd.Ac.ViewModels.PrivilegeViewModels
{
    using ViewModel;

    /// <summary>
    /// 
    /// </summary>
    public class GetPlistAccountOrganizationPrivileges : GetPlistResult
    {
        /// <summary>
        /// 
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OrganizationCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool? IncludeDescendants { get; set; }
    }
}
