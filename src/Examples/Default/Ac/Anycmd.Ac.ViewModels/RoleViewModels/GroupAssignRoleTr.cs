
namespace Anycmd.Ac.ViewModels.RoleViewModels
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class GroupAssignRoleTr
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid GroupId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid RoleId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsAssigned { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CategoryCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int SortCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int IsEnabled { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid? CreateUserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CreateBy { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateOn { get; set; }
    }
}
