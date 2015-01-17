
namespace Anycmd.Ac.ViewModels.Infra.UIViewViewModels
{
    using Engine;
    using Engine.Ac.InOuts;
    using Engine.Ac.Messages.Infra;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class UiViewButtonCreateInput : EntityCreateInput, IUiViewButtonCreateIo
    {
        public UiViewButtonCreateInput()
        {
            OntologyCode = "UiViewButton";
            Verb = "Create";
        }

        public string OntologyCode { get; private set; }

        public string Verb { get; private set; }

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

        public AddUiViewButtonCommand ToCommand()
        {
            return new AddUiViewButtonCommand(this);
        }
    }
}
