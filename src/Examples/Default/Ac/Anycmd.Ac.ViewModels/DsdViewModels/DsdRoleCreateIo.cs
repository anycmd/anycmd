
namespace Anycmd.Ac.ViewModels.DsdViewModels
{
    using Engine.Ac.Dsd;
    using Engine.InOuts;
    using Engine.Messages;
    using System;

    /// <summary>
    /// 创建动态职责分离角色时的输入或输出参数类型。
    /// </summary>
    public class DsdRoleCreateIo : EntityCreateInput, IDsdRoleCreateIo
    {
        public DsdRoleCreateIo()
        {
            HecpOntology = "DsdRole";
            HecpVerb = "Create";
        }

        public Guid DsdSetId { get; set; }

        public Guid RoleId { get; set; }

        public override IAnycmdCommand ToCommand(IAcSession acSession)
        {
            return new AddDsdRoleCommand(acSession, this);
        }
    }
}
