
namespace Anycmd.Ac.ViewModels.PrivilegeViewModels
{
    using Engine.Ac.Privileges;
    using Engine.Messages;
    using System;

    /// <summary>
    /// 更新权限二元组时的输入或输出参数类型。
    /// </summary>
    public class PrivilegeUpdateIo : IPrivilegeUpdateIo
    {
        public PrivilegeUpdateIo()
        {
            HecpOntology = "Privilege";
            HecpVerb = "Update";
        }

        public string HecpOntology { get; private set; }

        public string HecpVerb { get; private set; }

        public Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AcContent { get; set; }

        public IAnycmdCommand ToCommand(IAcSession acSession)
        {
            return new UpdatePrivilegeCommand(acSession, this);
        }
    }
}
