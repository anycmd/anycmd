
namespace Anycmd.Engine.Host.Edi.Messages
{
    using Commands;
    using InOuts;
    using Model;

    public class AddNodeCommand : AddEntityCommand<INodeCreateIo>, ISysCommand
    {
        public AddNodeCommand(INodeCreateIo input)
            : base(input)
        {

        }
    }
}
