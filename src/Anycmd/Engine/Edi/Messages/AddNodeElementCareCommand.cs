
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class AddNodeElementCareCommand : AddEntityCommand<INodeElementCareCreateIo>, IAnycmdCommand
    {
        public AddNodeElementCareCommand(IUserSession userSession, INodeElementCareCreateIo input)
            : base(userSession, input)
        {

        }
    }
}
