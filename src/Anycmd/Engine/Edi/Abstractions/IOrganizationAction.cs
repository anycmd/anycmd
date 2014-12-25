
namespace Anycmd.Engine.Edi.Abstractions
{
    using Engine.Host.Ac;
    using System;

    /// <summary>
    /// 本体组织结构动作级配置模型
    /// </summary>
    public interface IOrganizationAction
    {
        Guid Id { get; }
        /// <summary>
        /// 本体动作标识
        /// </summary>
        Guid ActionId { get; }
        /// <summary>
        /// 是否审核
        /// </summary>
        string IsAudit { get; }
        /// <summary>
        /// 是否“不允许”。True表示不允许，False表示允许。
        /// </summary>
        string IsAllowed { get; }
        /// <summary>
        /// 组织结构码
        /// </summary>
        Guid OrganizationId { get; }

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
