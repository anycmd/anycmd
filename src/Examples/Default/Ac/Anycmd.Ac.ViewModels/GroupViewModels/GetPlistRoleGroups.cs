
namespace Anycmd.Ac.ViewModels.GroupViewModels
{
    using System;
    using ViewModel;

    /// <summary>
    /// 
    /// </summary>
    public class GetPlistRoleGroups : GetPlistResult
    {
        /// <summary>
        /// 
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid RoleId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool? IsAssigned { get; set; }
    }
}
