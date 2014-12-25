
namespace Anycmd.Ac.ViewModels.Identity.AccountViewModels
{
    using ViewModel;

    /// <summary>
    /// 
    /// </summary>
    public class GetPlistContractors : GetPlistResult
    {
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
