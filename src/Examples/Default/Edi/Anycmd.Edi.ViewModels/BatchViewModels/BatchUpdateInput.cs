
namespace Anycmd.Edi.ViewModels.BatchViewModels
{
    using Engine.Edi.InOuts;
    using Engine.Edi.Messages;
    using Engine.Messages;
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 
    /// </summary>
    public class BatchUpdateInput : IBatchUpdateIo
    {
        public BatchUpdateInput()
        {
            HecpOntology = "Batch";
            HecpVerb = "Update";
        }

        public string HecpOntology { get; private set; }

        public string HecpVerb { get; private set; }

        public Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }

        public IAnycmdCommand ToCommand(IAcSession acSession)
        {
            return new UpdateBatchCommand(acSession, this);
        }
    }
}
