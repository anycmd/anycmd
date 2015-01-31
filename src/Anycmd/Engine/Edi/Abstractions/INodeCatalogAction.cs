using System;

namespace Anycmd.Engine.Edi.Abstractions
{
    using Host.Ac;

    public interface INodeCatalogAction
    {
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
        Guid ActionId { get; }
        /// <summary>
        /// 
        /// </summary>
        Guid CatalogId { get; }
        /// <summary>
        /// 
        /// </summary>
        string IsAllowed { get; }
        /// <summary>
        /// 
        /// </summary>
        string IsAudit { get; }

        /// <summary>
        /// 
        /// </summary>
        DateTime? CreateOn { get; }

        /// <summary>
        /// 是否允许
        /// </summary>
        AllowType AllowType { get; }

        /// <summary>
        /// 是否需要审核
        /// </summary>
        AuditType AuditType { get; }
    }
}
