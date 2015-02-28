
namespace Anycmd.Engine.Ac.Accounts
{
    using Messages;

    public sealed class UpdateAccountCommand : UpdateEntityCommand<IAccountUpdateIo>, IAnycmdCommand
    {
        public UpdateAccountCommand(IAcSession acSession, IAccountUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
