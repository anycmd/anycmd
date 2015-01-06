
namespace Anycmd.Engine.Ac.Messages.Identity
{
    using InOuts;

    public class AddAccountCommand : AddEntityCommand<IAccountCreateIo>, IAnycmdCommand
    {
        public AddAccountCommand(IAccountCreateIo input)
            : base(input)
        {
        }
    }
}
