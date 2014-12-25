
namespace Anycmd.Edi.ViewModels.ElementViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using ViewModel;

    /// <summary>
    /// 
    /// </summary>
    public class GetPlistElements : GetPlistResult
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Guid? OntologyId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid? GroupId { get; set; }
    }
}
