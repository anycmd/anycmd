
namespace Anycmd.Edi.ViewModels.OntologyViewModels
{
    using Engine.Edi.InOuts;
    using Engine.Edi.Messages;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ActionUpdateInput : IActionUpdateIo
    {
        public ActionUpdateInput()
        {
            OntologyCode = "Action";
            Verb = "Update";
        }

        public string OntologyCode { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }
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

        public UpdateActionCommand ToCommand(IUserSession userSession)
        {
            return new UpdateActionCommand(userSession, this);
        }
    }
}
