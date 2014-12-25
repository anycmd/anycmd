
namespace Anycmd.Edi.ViewModels.StateCodeViewModels
{
    using Model;
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 
    /// </summary>
    public class StateCodeUpdateInput : ManagedPropertyValues
    {
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
    }
}
