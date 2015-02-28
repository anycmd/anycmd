
namespace Anycmd.Edi.ViewModels.ArchiveViewModels
{
    using Engine.Edi.InOuts;
    using Engine.Edi.Messages;
    using Engine.Messages;
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 
    /// </summary>
    public class ArchiveUpdateInput : IArchiveUpdateIo
    {
        public ArchiveUpdateInput()
        {
            HecpOntology = "Archive";
            HecpVerb = "Update";
        }

        public string HecpOntology { get; private set; }

        public string HecpVerb { get; private set; }

        public Guid Id { get; set; }
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
        [StringLength(50)]
        [DisplayName(@"备注")]
        public string Description { get; set; }

        public IAnycmdCommand ToCommand(IAcSession acSession)
        {
            return new UpdateArchiveCommand(acSession, this);
        }
    }
}
