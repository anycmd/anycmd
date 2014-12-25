
namespace Anycmd.Engine.Ac.Abstractions.Infra
{
    using System;

    /// <summary>
    /// 表示该接口的实现类是系统字典项。
    /// <remarks>
    /// 字典是没有层级的，有层级的只有组织结构。
    /// </remarks>
    /// </summary>
    public interface IDicItem
    {
        /// <summary>
        /// 
        /// </summary>
        Guid Id { get; }
        /// <summary>
        /// 编码
        /// </summary>
        string Code { get; }
        /// <summary>
        /// 名称
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 所属字典
        /// </summary>
        Guid DicId { get; }
        /// <summary>
        /// 排序
        /// </summary>
        int SortCode { get; }

        int IsEnabled { get; }
    }
}
