
namespace Anycmd.Engine.Ac.Accounts
{
    using Messages;

    public sealed class AddAccountCommand : AddEntityCommand<IAccountCreateIo>, IAnycmdCommand
    {
        public AddAccountCommand(IAcSession acSession, IAccountCreateIo input)
            : base(acSession, input)
        {
        }
    }
}
