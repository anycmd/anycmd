
namespace Anycmd.Ac.ViewModels.AccountViewModels
{
    using Engine;
    using Engine.Ac.Accounts;

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

        public IAnycmdCommand ToCommand(IAcSession acSession)
        {
            return new ChangePasswordCommand(acSession, this);
        }
    }
}
