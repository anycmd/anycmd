
namespace Anycmd.Edi.ViewModels.InfoConstraintViewModels
{
    using Engine;
    using Engine.Edi.InOuts;
    using Engine.Edi.Messages;
    using System;
    using System.ComponentModel;
    using ViewModel;

    /// <summary>
    /// 
    /// </summary>
    public class InfoRuleUpdateInput : ManagedPropertyValues, IInfoRuleUpdateIo, IInfoModel
    {
        public InfoRuleUpdateInput()
        {
            OntologyCode = "InfoRule";
            Verb = "Update";
        }

        public string OntologyCode { get; private set; }

        public string Verb { get; private set; }

        public Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(1)]
        public int IsEnabled { get; set; }

        public UpdateInfoRuleCommand ToCommand(IUserSession userSession)
        {
            return new UpdateInfoRuleCommand(userSession, this);
        }
    }
}
