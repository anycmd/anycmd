
namespace Anycmd.Engine.Edi.Abstractions {
    using System;

    /// <summary>
    /// 本体目录级配置模型
    /// </summary>
    public interface IOntologyOrganization {
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
        Guid OrganizationId { get; }
        /// <summary>
        /// 
        /// </summary>
        string Actions { get; }
    }
}
