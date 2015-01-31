
namespace Anycmd.Engine.Edi.Abstractions
{
    using System;

    /// <summary>
    /// 本体目录级配置模型
    /// </summary>
    public interface IOntologyCatalog
    {
        /// <summary>
        /// 
        /// </summary>
        Guid Id { get; }
        /// <summary>
        /// 
        /// </summary>
        Guid OntologyId { get; }
        /// <summary>
        /// 
        /// </summary>
        Guid CatalogId { get; }
        /// <summary>
        /// 
        /// </summary>
        string Actions { get; }
    }
}
