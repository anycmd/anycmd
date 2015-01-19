
namespace Anycmd.Edi.ViewModels.ProcessViewModels
{
    using Engine.Edi.InOuts;
    using Engine.Edi.Messages;
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
            OntologyCode = "Process";
            Verb = "Update";
        }

        public string OntologyCode { get; private set; }

        public string Verb { get; private set; }

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

        public UpdateProcessCommand ToCommand(IUserSession userSession)
        {
            return new UpdateProcessCommand(userSession, this);
        }
    }
}
