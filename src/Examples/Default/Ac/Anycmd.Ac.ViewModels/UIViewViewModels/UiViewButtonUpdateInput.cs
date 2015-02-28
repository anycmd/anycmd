
namespace Anycmd.Ac.ViewModels.UIViewViewModels
{
    using Engine.Ac.UiViews;
    using Engine.Messages;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class UiViewButtonUpdateInput : IUiViewButtonUpdateIo
    {
        public UiViewButtonUpdateInput()
        {
            HecpOntology = "UiViewButton";
            HecpVerb = "Update";
        }

        public string HecpOntology { get; private set; }

        public string HecpVerb { get; private set; }

        public Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid? FunctionId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int IsEnabled { get; set; }

        public IAnycmdCommand ToCommand(IAcSession acSession)
        {
            return new UpdateUiViewButtonCommand(acSession, this);
        }
    }
}
