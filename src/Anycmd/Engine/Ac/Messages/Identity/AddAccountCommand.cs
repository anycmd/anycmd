
namespace Anycmd.Engine.Ac.Messages.Identity
{
    using InOuts;

    public class AddAccountCommand : AddEntityCommand<IAccountCreateIo>, IAnycmdCommand
    {
        public AddAccountCommand(IAcSession acSession, IAccountCreateIo input)
            : base(acSession, input)
        {
        }
    }
}
