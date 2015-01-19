
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class AddNodeCommand : AddEntityCommand<INodeCreateIo>, IAnycmdCommand
    {
        public AddNodeCommand(IUserSession userSession, INodeCreateIo input)
            : base(userSession, input)
        {

        }
    }
}
