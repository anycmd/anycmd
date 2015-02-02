
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class AddNodeElementCareCommand : AddEntityCommand<INodeElementCareCreateIo>, IAnycmdCommand
    {
        public AddNodeElementCareCommand(IAcSession userSession, INodeElementCareCreateIo input)
            : base(userSession, input)
        {

        }
    }
}
