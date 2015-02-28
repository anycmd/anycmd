
namespace Anycmd.Edi.ViewModels.ProcessViewModels
{
    using Engine.Edi.InOuts;
    using Engine.Edi.Messages;
    using Engine.Messages;
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using ViewModel;

    /// <summary>
    /// 
    /// </summary>
    public class ProcessUpdateInput : IProcessUpdateIo, IInfoModel
    {
        public ProcessUpdateInput()
        {
            HecpOntology = "Process";
            HecpVerb = "Update";
        }

        public string HecpOntology { get; private set; }

        public string HecpVerb { get; private set; }

        public Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(1)]
        public int IsEnabled { get; set; }

        public IAnycmdCommand ToCommand(IAcSession acSession)
        {
            return new UpdateProcessCommand(acSession, this);
        }
    }
}
