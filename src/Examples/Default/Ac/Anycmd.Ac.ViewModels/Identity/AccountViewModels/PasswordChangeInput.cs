
namespace Anycmd.Ac.ViewModels.Identity.AccountViewModels
{
    using Engine.Ac.InOuts;
    using Engine.Ac.Messages.Identity;

    public class PasswordChangeInput : IPasswordChangeIo
    {
        public PasswordChangeInput()
        {
            OntologyCode = "Account";
            Verb = "ChangePassword";
        }

        public string OntologyCode { get; private set; }

        public string Verb { get; private set; }

        public string LoginName { get; set; }

        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        public ChangePasswordCommand ToCommand(IUserSession userSession)
        {
            return new ChangePasswordCommand(this, userSession);
        }
    }
}
