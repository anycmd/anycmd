using System;

namespace Anycmd.Ac.ViewModels.RoleViewModels
{
    public class SsdSetAssignRoleTr
    {
        /// <summary>
        /// SsdRole记录的标识
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// SsdSet记录的标识
        /// </summary>
        public Guid SsdSetId { get; set; }


        /// <summary>
        /// Role记录的标识
        /// </summary>
        public Guid RoleId { get; set; }

        /// <summary>
        /// 表示是否将已将当前Role加入当前SsdSet中。
        /// </summary>
        public bool IsAssigned { get; set; }

        /// <summary>
        /// 表示Role记录的名字。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 表示Role记录的图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 表示Role记录的类别，SsdRole记录没有CatagoryCode字段。
        /// </summary>
        public string CategoryCode { get; set; }

        /// <summary>
        /// 表示Role记录在Role记录集中的排序，SsdRole没有SortCode字段。
        /// </summary>
        public int SortCode { get; set; }

        /// <summary>
        /// 角色的有效状态。
        /// <remarks>SsdRole上没有IsEnabled字段，Role上有IsEnabled字段，这是Role上的IsEnabled字段。</remarks>
        /// </summary>
        public int IsEnabled { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateOn { get; set; }
    }
}
