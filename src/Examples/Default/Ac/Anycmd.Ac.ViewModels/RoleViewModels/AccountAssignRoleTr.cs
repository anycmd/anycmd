
namespace Anycmd.Ac.ViewModels.RoleViewModels
{
    using System;

    /// <summary>
    /// 为账户分配角色时使用的模型
    /// </summary>
    public class AccountAssignRoleTr
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid AccountId { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public Guid RoleId { get; set; }

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

        public string CreateBy { get; set; }

        public Guid? CreateUserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateOn { get; set; }
    }
}
