
namespace Anycmd.Ac.ViewModels.RoleViewModels
{
    using System;
    using ViewModel;

    /// <summary>
    /// 
    /// </summary>
    public class GetPlistGroupRoles : GetPlistResult
    {
        /// <summary>
        /// 
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid GroupId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool? IsAssigned { get; set; }
    }
}
