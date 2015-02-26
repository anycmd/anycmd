
namespace Anycmd.Engine.Ac.Accounts
{

    public class UpdateAccountCommand : UpdateEntityCommand<IAccountUpdateIo>, IAnycmdCommand
    {
        public UpdateAccountCommand(IAcSession acSession, IAccountUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
