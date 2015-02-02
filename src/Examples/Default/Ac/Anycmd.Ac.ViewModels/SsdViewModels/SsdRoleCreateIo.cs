
namespace Anycmd.Ac.ViewModels.SsdViewModels
{
    using Engine;
    using Engine.Ac.InOuts;
    using Engine.Ac.Messages.Rbac;
    using System;

    /// <summary>
    /// 创建静态职责分离角色时的输入或输出参数类型。
    /// </summary>
    public class SsdRoleCreateIo : EntityCreateInput, ISsdRoleCreateIo
    {
        public SsdRoleCreateIo()
        {
            HecpOntology = "SsdRole";
            HecpVerb = "Create";
        }

        public Guid SsdSetId { get; set; }

        public Guid RoleId { get; set; }

        public override IAnycmdCommand ToCommand(IAcSession userSession)
        {
            return new AddSsdRoleCommand(userSession, this);
        }
    }
}
