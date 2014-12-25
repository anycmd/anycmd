
namespace Anycmd.Edi.ViewModels.BatchViewModels
{
    using Engine.Host.Edi.InOuts;
    using Model;
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 
    /// </summary>
    public class BatchUpdateInput : IInputModel, IBatchUpdateIo
    {
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
    }
}
