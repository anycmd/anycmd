
namespace Anycmd.Engine.Edi.Abstractions {
    using System;

    /// <summary>
    /// 
    /// </summary>
    public interface IInfoGroup {
        /// <summary>
        /// 
        /// </summary>
        Guid Id { get; }
        /// <summary>
        /// 
        /// </summary>
        string Code { get; }
        /// <summary>
        /// 
        /// </summary>
        string Description { get; }
        /// <summary>
        /// 
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 
        /// </summary>
        Guid OntologyId { get; }
        /// <summary>
        /// 
        /// </summary>
        int SortCode { get; }
    }
}
