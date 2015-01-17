
namespace Anycmd.Ac.ViewModels.Infra.UIViewViewModels
{
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
            OntologyCode = "UiView";
            Verb = "Update";
        }

        public string OntologyCode { get; private set; }

        public string Verb { get; private set; }

        public Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Tooltip { get; set; }

        public UpdateUiViewCommand ToCommand()
        {
            return new UpdateUiViewCommand(this);
        }
    }
}
