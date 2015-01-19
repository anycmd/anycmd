
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class AddNodeActionCommand : AddEntityCommand<INodeActionCreateIo>, IAnycmdCommand
    {
        public AddNodeActionCommand(IUserSession userSession, INodeActionCreateIo input)
            : base(userSession, input)
        {

        }
    }
}
