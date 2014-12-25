
namespace Anycmd.Edi.ViewModels.ElementViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using ViewModel;

    /// <summary>
    /// 
    /// </summary>
    public class GetNodeElementCares : GetPlistResult
    {
        /// <summary>
        /// 
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Guid NodeId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Guid OntologyId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool? IsAssigned { get; set; }
    }
}
