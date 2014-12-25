
namespace Anycmd.Ac.ViewModels.RoleViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using ViewModel;

    public class GetPlistSsdSetRoles : GetPlistResult
    {
        /// <summary>
        /// 
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Guid SsdSetId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool? IsAssigned { get; set; }
    }
}
