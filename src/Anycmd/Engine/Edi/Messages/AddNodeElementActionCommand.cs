
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class AddNodeElementActionCommand : AddEntityCommand<INodeElementActionCreateIo>, IAnycmdCommand
    {
        public AddNodeElementActionCommand(INodeElementActionCreateIo input)
            : base(input)
        {

        }
    }
}
