
namespace Anycmd.Engine.Ac.Messages.Identity
{
    using InOuts;

    public class AddAccountCommand : AddEntityCommand<IAccountCreateIo>, IAnycmdCommand
    {
        public AddAccountCommand(IAcSession userSession, IAccountCreateIo input)
            : base(userSession, input)
        {
        }
    }
}
