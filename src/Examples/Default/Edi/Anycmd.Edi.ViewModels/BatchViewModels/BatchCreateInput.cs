
namespace Anycmd.Edi.ViewModels.BatchViewModels
{
    using Engine.Host.Edi.InOuts;
    using Model;
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 
    /// </summary>
    public class BatchCreateInput : EntityCreateInput, IInputModel, IBatchCreateIo
    {
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
        public string OrganizationCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool? IncludeDescendants { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
    }
}
