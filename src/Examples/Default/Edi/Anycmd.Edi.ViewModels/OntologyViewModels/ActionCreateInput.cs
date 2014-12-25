
namespace Anycmd.Edi.ViewModels.OntologyViewModels
{
    using Engine.Host.Edi.InOuts;
    using Model;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ActionCreateInput : EntityCreateInput, IActionCreateIo
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Verb { get; set; }
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
        [Required]
        public string IsAllowed { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string IsAudit { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public bool IsPersist { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int SortCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
    }
}
