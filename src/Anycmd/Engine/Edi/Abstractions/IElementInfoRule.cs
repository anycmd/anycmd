
namespace Anycmd.Engine.Edi.Abstractions
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    public interface IElementInfoRule
    {
        /// <summary>
        /// 
        /// </summary>
        Guid Id { get; }
        /// <summary>
        /// 
        /// </summary>
        Guid ElementId { get; }
        /// <summary>
        /// 
        /// </summary>
        Guid InfoRuleId { get; }
        /// <summary>
        /// 
        /// </summary>
        int IsEnabled { get; }
        /// <summary>
        /// 
        /// </summary>
        int SortCode { get; }
        DateTime CreateOn { get; }
    }
}
