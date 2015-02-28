
namespace Anycmd.Edi.ViewModels.OntologyViewModels
{
    using Engine.Edi.InOuts;
    using Engine.Edi.Messages;
    using Engine.InOuts;
    using Engine.Messages;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class TopicCreateInput : EntityCreateInput, ITopicCreateIo
    {
        public TopicCreateInput()
        {
            HecpOntology = "Topic";
            HecpVerb = "Create";
        }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Guid OntologyId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }

        public bool IsAllowed { get; set; }

        public override IAnycmdCommand ToCommand(IAcSession acSession)
        {
            return new AddTopicCommand(acSession, this);
        }
    }
}
