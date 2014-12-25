
namespace Anycmd.Engine.Edi.Abstractions
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    public interface ITopic
    {
        Guid Id { get; }
        /// <summary>
        /// 本体标识
        /// </summary>
        Guid OntologyId { get; }

        /// <summary>
        /// 动作码
        /// </summary>
        string Code { get; }

        /// <summary>
        /// 动作名
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 
        /// </summary>
        bool IsAllowed { get; }
        /// <summary>
        /// 
        /// </summary>
        string Description { get; }

        DateTime? CreateOn { get; }
    }
}
