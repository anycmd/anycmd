
namespace Anycmd.Ac.ViewModels.AccountViewModels
{
    using Engine;
    using Engine.Ac.UiViews;
    using Engine.Ac.Accounts;
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

        public IAnycmdCommand ToCommand(IAcSession acSession)
        {
            return new AssignPasswordCommand(acSession, this);
        }
    }
}
