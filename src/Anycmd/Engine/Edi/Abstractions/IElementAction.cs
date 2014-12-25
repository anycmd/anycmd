
namespace Anycmd.Engine.Edi.Abstractions
{
    using System;

    /// <summary>
    /// 本体元素级动作。将本体元素与动作的关系视作实体。
    /// </summary>
    public interface IElementAction
    {
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
        string IsAllowed { get; }
        /// <summary>
        /// 
        /// </summary>
        string IsAudit { get; }
    }
}
