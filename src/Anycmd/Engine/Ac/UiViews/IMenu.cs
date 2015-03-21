
namespace Anycmd.Engine.Ac.UiViews
{
    using System;

    /// <summary>
    /// 表示该接口 实现类是系统菜单。
    /// <remarks>
    /// 菜单界面元素对应系统内的空间记录。用户通过点击菜单告知系统他/她希望系统将他/她的分神传送到什么空间中去。
    /// </remarks>
    /// </summary>
    public interface IMenu
    {
        /// <summary>
        /// 读取菜单标识
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// 读取该菜单所归属的应用系统的标识。
        /// </summary>
        Guid AppSystemId { get; }

        /// <summary>
        /// 父级菜单的Id
        /// </summary>
        Guid? ParentId { get; }

        /// <summary>
        /// 名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 用于定位空间的逻辑定位符。如“/User/Index”，不一定是定位到本菜单所属系统的内部的空间，
        /// 也可能定位到外部系统的空间，如“http://j.map.baidu.com/u3fgA”。
        /// </summary>
        string Url { get; }
        /// <summary>
        /// 
        /// </summary>
        int SortCode { get; }

        /// <summary>
        /// 图标
        /// </summary>
        string Icon { get; }

        /// <summary>
        /// 读取菜单描述文字
        /// </summary>
        string Description { get; }
    }
}
