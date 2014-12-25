
namespace Anycmd.Edi.ViewModels.OntologyViewModels
{
    using Engine.Host.Edi.InOuts;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class InfoGroupUpdateInput : IInfoGroupUpdateIo
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }
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
        public int SortCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
    }
}
