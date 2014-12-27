
namespace Anycmd.Edi.ViewModels.ArchiveViewModels
{
    using Engine.Edi.InOuts;
    using Model;
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 
    /// </summary>
    public class ArchiveCreateInput : EntityCreateInput, IInputModel, IArchiveCreateIo
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(200)]
        [DisplayName(@"标题")]
        public string Title { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Guid OntologyId { get; set; }

        public string RdbmsType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(50)]
        [DisplayName(@"备注")]
        public string Description { get; set; }
    }
}
