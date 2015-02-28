
namespace Anycmd.Ac.ViewModels.PrivilegeViewModels
{
    using Engine.Ac.Privileges;
    using Engine.InOuts;
    using Engine.Messages;
    using System;

    /// <summary>
    /// 创建权限二元组时的输入或输出参数类型。
    /// </summary>
    public class PrivilegeCreateIo : EntityCreateInput, IPrivilegeCreateIo
    {
        public PrivilegeCreateIo()
        {
            HecpOntology = "Privilege";
            HecpVerb = "Create";
        }

        public string SubjectType { get; set; }

        public Guid SubjectInstanceId { get; set; }

        public string ObjectType { get; set; }

        public Guid ObjectInstanceId { get; set; }

        public string AcContent { get; set; }

        public string AcContentType { get; set; }

        public override IAnycmdCommand ToCommand(IAcSession acSession)
        {
            return new AddPrivilegeCommand(acSession, this);
        }
    }
}
