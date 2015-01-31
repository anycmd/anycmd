
namespace Anycmd.Engine.Edi.Abstractions
{
    using Host.Ac;
    using System;

    /// <summary>
    /// 本体目录动作级配置模型
    /// </summary>
    public interface ICatalogAction
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
        /// 目录码
        /// </summary>
        Guid CatalogId { get; }

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
