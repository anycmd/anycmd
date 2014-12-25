
namespace Anycmd.Engine.Host.Ac
{
    using System.ComponentModel;

    /// <summary>
    /// 审核类型
    /// </summary>
    public enum AuditType : byte
    {
        /// <summary>
        /// 非法的审核类型
        /// </summary>
        [Description("非法的审核类型")]
        Invalid = 0,
        /// <summary>
        /// 显式审核
        /// </summary>
        [Description("显式审核")]
        ExplicitAudit = 1,
        /// <summary>
        /// 隐式审核
        /// </summary>
        [Description("隐式审核")]
        ImplicitAudit = 2,
        /// <summary>
        /// 显式不审核
        /// </summary>
        [Description("显式不审核")]
        NotAudit = 3
    }
}
