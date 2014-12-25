
namespace Anycmd.Engine.Ac.Abstractions.Infra
{
    using System;

    /// <summary>
    /// 表示该接口 实现类是系统菜单。
    /// </summary>
    public interface IMenu
    {
        /// <summary>
        /// 
        /// </summary>
        Guid Id { get; }

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
        /// 菜单对应的Url
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

        string Description { get; }
    }
}
