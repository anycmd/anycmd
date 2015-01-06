
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class AddNodeActionCommand : AddEntityCommand<INodeActionCreateIo>, IAnycmdCommand
    {
        public AddNodeActionCommand(INodeActionCreateIo input)
            : base(input)
        {

        }
    }
}
