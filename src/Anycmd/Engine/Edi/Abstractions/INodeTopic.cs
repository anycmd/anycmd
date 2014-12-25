
namespace Anycmd.Engine.Edi.Abstractions {
    using System;

    /// <summary>
    /// 
    /// </summary>
    public interface INodeTopic {
        /// <summary>
        /// 
        /// </summary>
        Guid Id { get; }
        /// <summary>
        /// 
        /// </summary>
        Guid NodeId { get; }
        /// <summary>
        /// 
        /// </summary>
        Guid TopicId { get; }
    }
}
