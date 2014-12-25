
namespace Anycmd.Ac.ViewModels.RoleViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using ViewModel;

    public class GetPlistDsdSetRoles : GetPlistResult
    {
        /// <summary>
        /// 
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Guid DsdSetId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool? IsAssigned { get; set; }
    }
}
