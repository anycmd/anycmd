
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

        public UpdateUiViewCommand ToCommand(IUserSession userSession)
        {
            return new UpdateUiViewCommand(userSession, this);
        }
    }
}
