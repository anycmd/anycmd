
namespace Anycmd.Engine.Edi.Abstractions {
    using System;

    /// <summary>
    /// 节点关心本体元素。
    /// <remarks>
    /// 如此简单的模型为什么是接口？使用接口将其约束为不可变模型，从而使插件开发者不能使用正常手段修改它。
    /// </remarks>
    /// </summary>
    public interface INodeElementCare {
        /// <summary>
        /// 
        /// </summary>
        Guid Id { get; }
        /// <summary>
        /// 本体元素标识
        /// </summary>
        Guid ElementId { get; }

        /// <summary>
        /// 节点标识
        /// </summary>
        Guid NodeId { get; }
        /// <summary>
        /// 是否是信息标识本体元素
        /// </summary>
        bool IsInfoIdItem { get; }
    }
}
