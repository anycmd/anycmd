
namespace Anycmd.Edi.ViewModels.BatchViewModels
{
    using Engine.Edi.InOuts;
    using Engine.Edi.Messages;
    using Engine.InOuts;
    using Engine.Messages;
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 
    /// </summary>
    public class BatchCreateInput : EntityCreateInput, IBatchCreateIo
    {
        public BatchCreateInput()
        {
            HecpOntology = "Batch";
            HecpVerb = "Create";
        }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Guid OntologyId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Guid NodeId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CatalogCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool? IncludeDescendants { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }

        public override IAnycmdCommand ToCommand(IAcSession acSession)
        {
            return new AddBatchCommand(acSession, this);
        }
    }
}
