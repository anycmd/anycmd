
namespace Anycmd.Ac.ViewModels.RoleViewModels
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class MenuAssignRoleTr
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid MenuId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid RoleId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsAssigned { get; set; }

        public string AcContentType { get; set; }

        public string AcContent { get; set; }

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

        public Guid? CreateUserId { get; set; }

        public string CreateBy { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateOn { get; set; }
    }
}
