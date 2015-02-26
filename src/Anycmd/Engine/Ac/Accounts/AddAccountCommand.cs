
namespace Anycmd.Engine.Ac.Accounts
{

    public class AddAccountCommand : AddEntityCommand<IAccountCreateIo>, IAnycmdCommand
    {
        public AddAccountCommand(IAcSession acSession, IAccountCreateIo input)
            : base(acSession, input)
        {
        }
    }
}
