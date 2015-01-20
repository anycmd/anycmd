
namespace Anycmd.Edi.ViewModels.StateCodeViewModels
{
    using Engine;
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 
    /// </summary>
    public class StateCodeUpdateInput : ManagedPropertyValues
    {
        public StateCodeUpdateInput()
        {
            OntologyCode = "StateCode";
            Verb = "Update";
        }

        public string OntologyCode { get; private set; }

        public string Verb { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string ReasonPhrase { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }

        // TODO:走CommandBus
    }
}
