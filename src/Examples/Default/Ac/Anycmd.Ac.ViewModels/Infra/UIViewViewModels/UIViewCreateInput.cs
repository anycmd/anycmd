
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
            OntologyCode = "UiView";
            Verb = "Create";
        }

        public string OntologyCode { get; private set; }

        public string Verb { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Tooltip { get; set; }

        public AddUiViewCommand ToCommand()
        {
            return new AddUiViewCommand(this);
        }
    }
}
