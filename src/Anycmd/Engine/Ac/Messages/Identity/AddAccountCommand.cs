
namespace Anycmd.Engine.Ac.Messages.Identity
{
    using InOuts;

    public class AddAccountCommand : AddEntityCommand<IAccountCreateIo>, IAnycmdCommand
    {
        public AddAccountCommand(IUserSession userSession, IAccountCreateIo input)
            : base(userSession, input)
        {
        }
    }
}
