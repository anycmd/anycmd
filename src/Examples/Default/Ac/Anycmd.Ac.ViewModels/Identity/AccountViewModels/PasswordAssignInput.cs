
namespace Anycmd.Ac.ViewModels.Identity.AccountViewModels
{
    using Engine;
    using Engine.Ac.InOuts;
    using Engine.Ac.Messages.Identity;
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 
    /// </summary>
    public class PasswordAssignInput : IPasswordAssignIo
    {
        public PasswordAssignInput()
        {
            HecpOntology = "Account";
            HecpVerb = "AssignPassword";
        }

        public string HecpOntology { get; private set; }

        public string HecpVerb { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string LoginName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Password { get; set; }

        public IAnycmdCommand ToCommand(IUserSession userSession)
        {
            return new AssignPasswordCommand(userSession, this);
        }
    }
}
