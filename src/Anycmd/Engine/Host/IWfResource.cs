
namespace Anycmd.Engine.Host
{
    using System;

    /// <summary>
    /// 资源
    /// </summary>
    public interface IWfResource
    {
        /// <summary>
        /// 资源标识
        /// </summary>
        Guid Id { get; }
        /// <summary>
        /// 资源名称
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 资源类型
        /// </summary>
        BuiltInResourceKind BuiltInResourceKind { get; }
        /// <summary>
        /// 描述
        /// </summary>
        string Description { get; }
    }
}
