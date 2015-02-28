
namespace Anycmd.Ac.ViewModels.UIViewViewModels
{
    using Engine.Ac.UiViews;
    using Engine.InOuts;
    using Engine.Messages;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class UiViewButtonCreateInput : EntityCreateInput, IUiViewButtonCreateIo
    {
        public UiViewButtonCreateInput()
        {
            HecpOntology = "UiViewButton";
            HecpVerb = "Create";
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid UiViewId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid ButtonId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid? FunctionId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int IsEnabled { get; set; }

        public override IAnycmdCommand ToCommand(IAcSession acSession)
        {
            return new AddUiViewButtonCommand(acSession, this);
        }
    }
}
