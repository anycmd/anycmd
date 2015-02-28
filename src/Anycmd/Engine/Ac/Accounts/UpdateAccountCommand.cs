
namespace Anycmd.Engine.Ac.Accounts
{
    using Messages;

    public class UpdateAccountCommand : UpdateEntityCommand<IAccountUpdateIo>, IAnycmdCommand
    {
        public UpdateAccountCommand(IAcSession acSession, IAccountUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
