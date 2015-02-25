
namespace Anycmd.Ac.ViewModels.Infra.UIViewViewModels
{
    using Engine;
    using Engine.Ac.InOuts;
    using Engine.Ac.Messages.Infra;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class UiViewUpdateInput : IUiViewUpdateIo
    {
        public UiViewUpdateInput()
        {
            HecpOntology = "UiView";
            HecpVerb = "Update";
        }

        public string HecpOntology { get; private set; }

        public string HecpVerb { get; private set; }

        public Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Tooltip { get; set; }

        public IAnycmdCommand ToCommand(IAcSession acSession)
        {
            return new UpdateUiViewCommand(acSession, this);
        }
    }
}
