
namespace Anycmd.Ac.ViewModels.Identity.AccountViewModels
{
    using Engine.Ac.InOuts;

    public class PasswordChangeInput : IPasswordChangeIo
    {
        public string LoginName { get; set; }

        public string OldPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
