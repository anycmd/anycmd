
namespace Anycmd.Edi.ViewModels.ArchiveViewModels
{
    using Engine;
    using Engine.Edi.InOuts;
    using Engine.Edi.Messages;
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 
    /// </summary>
    public class ArchiveCreateInput : EntityCreateInput, IArchiveCreateIo
    {
        public ArchiveCreateInput()
        {
            OntologyCode = "Archive";
            Verb = "Create";
        }

        public string OntologyCode { get; private set; }

        public string Verb { get; private set; }

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

        public AddArchiveCommand ToCommand(IUserSession userSession)
        {
            return new AddArchiveCommand(userSession, this);
        }
    }
}
