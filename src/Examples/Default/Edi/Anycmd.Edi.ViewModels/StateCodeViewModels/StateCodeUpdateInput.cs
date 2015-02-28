
namespace Anycmd.Edi.ViewModels.StateCodeViewModels
{
    using Engine;
    using Engine.Edi.InOuts;
    using Engine.Edi.Messages;
    using Engine.Messages;
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 
    /// </summary>
    public class StateCodeUpdateInput : ManagedPropertyValues, IStateCodeUpdateInput
    {
        public StateCodeUpdateInput()
        {
            HecpOntology = "StateCode";
            HecpVerb = "Update";
        }

        public string HecpOntology { get; private set; }

        public string HecpVerb { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string ReasonPhrase { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }

        public IAnycmdCommand ToCommand(IAcSession acSession)
        {
            return new UpdateStateCodeCommand(acSession, this);
        }
    }
}
