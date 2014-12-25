
namespace Anycmd.Engine.Edi.Abstractions {
    using System;

    /// <summary>
    /// 
    /// </summary>
    public interface INodeElementAction {
        /// <summary>
        /// 
        /// </summary>
        Guid Id { get; }
        /// <summary>
        /// 
        /// </summary>
        Guid ActionId { get; }
        /// <summary>
        /// 
        /// </summary>
        Guid ElementId { get; }
        /// <summary>
        /// 
        /// </summary>
        bool IsAllowed { get; }
        /// <summary>
        /// 
        /// </summary>
        bool IsAudit { get; }
        /// <summary>
        /// 
        /// </summary>
        Guid NodeId { get; }

        /// <summary>
        /// 
        /// </summary>
        DateTime? CreateOn { get; }
    }
}
