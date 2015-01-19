
namespace Anycmd.Ac.ViewModels.Infra.UIViewViewModels
{
    using Engine;
    using Engine.Ac.InOuts;
    using Engine.Ac.Messages.Infra;

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

        public AddUiViewCommand ToCommand(IUserSession userSession)
        {
            return new AddUiViewCommand(userSession, this);
        }
    }
}
