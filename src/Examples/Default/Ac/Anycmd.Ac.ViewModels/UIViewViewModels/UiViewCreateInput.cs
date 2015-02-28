
namespace Anycmd.Ac.ViewModels.UIViewViewModels
{
    using Engine.Ac.UiViews;
    using Engine.InOuts;
    using Engine.Messages;

    /// <summary>
    /// 
    /// </summary>
    public class UiViewCreateInput : EntityCreateInput, IUiViewCreateIo
    {
        public UiViewCreateInput()
        {
            HecpOntology = "UiView";
            HecpVerb = "Create";
        }

        /// <summary>
        /// 
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Tooltip { get; set; }

        public override IAnycmdCommand ToCommand(IAcSession acSession)
        {
            return new AddUiViewCommand(acSession, this);
        }
    }
}
