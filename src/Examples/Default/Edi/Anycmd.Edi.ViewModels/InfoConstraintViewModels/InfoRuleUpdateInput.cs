
namespace Anycmd.Edi.ViewModels.InfoConstraintViewModels
{
    using Engine;
    using Engine.Edi.InOuts;
    using Engine.Edi.Messages;
    using Engine.Messages;
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
            HecpOntology = "InfoRule";
            HecpVerb = "Update";
        }

        public string HecpOntology { get; private set; }

        public string HecpVerb { get; private set; }

        public Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(1)]
        public int IsEnabled { get; set; }

        public IAnycmdCommand ToCommand(IAcSession acSession)
        {
            return new UpdateInfoRuleCommand(acSession, this);
        }
    }
}
