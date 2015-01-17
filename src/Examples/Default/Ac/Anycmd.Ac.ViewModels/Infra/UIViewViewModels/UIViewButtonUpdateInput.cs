
namespace Anycmd.Ac.ViewModels.Infra.UIViewViewModels
{
    using Engine.Ac.InOuts;
    using Engine.Ac.Messages.Infra;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class UiViewButtonUpdateInput : IUiViewButtonUpdateIo
    {
        public UiViewButtonUpdateInput()
        {
            OntologyCode = "UiViewButton";
            Verb = "Update";
        }

        public string OntologyCode { get; private set; }

        public string Verb { get; private set; }

        public Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid? FunctionId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int IsEnabled { get; set; }

        public UpdateUiViewButtonCommand ToCommand()
        {
            return new UpdateUiViewButtonCommand(this);
        }
    }
}
