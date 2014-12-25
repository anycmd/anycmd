
namespace Anycmd.Ac.ViewModels.RoleViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using ViewModel;

    /// <summary>
    /// 
    /// </summary>
    public class GetPlistAccountRoles : GetPlistResult
    {
        /// <summary>
        /// 
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Guid AccountId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool? IsAssigned { get; set; }
    }
}
