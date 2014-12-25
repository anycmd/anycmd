
namespace Anycmd.Engine.Host.Edi.Handlers
{
    using Ac.Infra;

    /// <summary>
    /// 鉴别器。鉴别给定的命令是否需要审核。
    /// <remarks>
    /// 审计鉴别步骤处在合法性鉴别和权限鉴别之后。
    /// </remarks>
    /// </summary>
    public interface IAuditDiscriminator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        DiscriminateResult IsNeedAudit(MessageContext context);
    }
}
