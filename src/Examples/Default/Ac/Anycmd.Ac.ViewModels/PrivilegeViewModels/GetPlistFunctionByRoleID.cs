
namespace Anycmd.Ac.ViewModels.PrivilegeViewModels
{
    using System;
    using ViewModel;

    /// <summary>
    /// 
    /// </summary>
    public class GetPlistFunctionByRoleId : GetPlistResult
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
        /// <summary>
        /// 
        /// </summary>
        public Guid AppSystemId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid? ResourceTypeId { get; set; }
    }
}
