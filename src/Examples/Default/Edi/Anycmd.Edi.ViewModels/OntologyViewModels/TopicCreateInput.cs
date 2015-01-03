
namespace Anycmd.Edi.ViewModels.OntologyViewModels
{
    using Engine;
    using Engine.Edi.InOuts;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class TopicCreateInput : EntityCreateInput, ITopicCreateIo
    {
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
    }
}
