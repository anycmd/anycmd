using System;

namespace Anycmd.Ac.ViewModels.RoleViewModels
{
    public class DsdSetAssignRoleTr
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid DsdSetId { get; set; }


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
        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateOn { get; set; }
    }
}
