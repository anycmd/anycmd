
namespace Anycmd.Ac.ViewModels.PrivilegeViewModels
{
    using Engine.Ac.InOuts;
    using Engine.Ac.Messages;
    using System;

    /// <summary>
    /// 更新权限二元组时的输入或输出参数类型。
    /// </summary>
    public class PrivilegeUpdateIo : IPrivilegeUpdateIo
    {
        public PrivilegeUpdateIo()
        {
            OntologyCode = "Privilege";
            Verb = "Update";
        }

        public string OntologyCode { get; private set; }

        public string Verb { get; private set; }

        public Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AcContent { get; set; }

        public UpdatePrivilegeCommand ToCommand(IUserSession userSession)
        {
            return new UpdatePrivilegeCommand(userSession, this);
        }
    }
}
