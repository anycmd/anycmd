
namespace Anycmd.Engine.Edi.Messages
{
    using Commands;
    using InOuts;
    using Model;

    public class AddNodeElementCareCommand : AddEntityCommand<INodeElementCareCreateIo>, ISysCommand
    {
        public AddNodeElementCareCommand(INodeElementCareCreateIo input)
            : base(input)
        {

        }
    }
}
