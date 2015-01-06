
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class AddNodeElementCareCommand : AddEntityCommand<INodeElementCareCreateIo>, IAnycmdCommand
    {
        public AddNodeElementCareCommand(INodeElementCareCreateIo input)
            : base(input)
        {

        }
    }
}
