
namespace Anycmd.Engine.Ac.Abstractions.Infra
{
    using System;

    /// <summary>
    /// 表示该接口的实现类是系统资源类型。
    /// </summary>
    public interface IResourceType
    {
        /// <summary>
        /// 
        /// </summary>
        Guid Id { get; }

        Guid AppSystemId { get; }

        /// <summary>
        /// 名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 编码
        /// </summary>
        string Code { get; }

        /// <summary>
        /// 
        /// </summary>
        string Icon { get; }

        int SortCode { get; }
    }
}
