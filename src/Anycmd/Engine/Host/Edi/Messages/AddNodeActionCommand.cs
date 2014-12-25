
namespace Anycmd.Engine.Host.Edi.Messages
{
    using Commands;
    using InOuts;
    using Model;

    public class AddNodeActionCommand : AddEntityCommand<INodeActionCreateIo>, ISysCommand
    {
        public AddNodeActionCommand(INodeActionCreateIo input)
            : base(input)
        {

        }
    }
}
