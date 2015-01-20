
namespace Anycmd.Ac.ViewModels.Identity.AccountViewModels
{
    using Engine;
    using Engine.Ac.InOuts;
    using Engine.Ac.Messages.Identity;

    public class PasswordChangeInput : IPasswordChangeIo
    {
        public PasswordChangeInput()
        {
            HecpOntology = "Account";
            HecpVerb = "ChangePassword";
        }

        public string HecpOntology { get; private set; }

        public string HecpVerb { get; private set; }

        public string LoginName { get; set; }

        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        public IAnycmdCommand ToCommand(IUserSession userSession)
        {
            return new ChangePasswordCommand(userSession, this);
        }
    }
}
