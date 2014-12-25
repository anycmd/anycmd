
namespace Anycmd.Engine.Ac.Abstractions.Infra
{
    using System;

    /// <summary>
    /// 表示该接口的实现类是系统字典
    /// <remarks>
    /// 字典是没有层级的，有层级的只有组织结构。
    /// </remarks>
    /// </summary>
    public interface IDic
    {
        /// <summary>
        /// 
        /// </summary>
        Guid Id { get; }
        /// <summary>
        /// 字典码
        /// </summary>
        string Code { get; }
        /// <summary>
        /// 字典名
        /// </summary>
        string Name { get; }

        int IsEnabled { get; }

        int SortCode { get; }
    }
}
