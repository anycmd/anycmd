
namespace Anycmd.Engine.Host.Edi.Messages
{
    using Commands;
    using InOuts;
    using Model;

    public class AddNodeOntologyCareCommand: AddEntityCommand<INodeOntologyCareCreateIo>, ISysCommand
    {
        public AddNodeOntologyCareCommand(INodeOntologyCareCreateIo input)
            : base(input)
        {

        }
    }
}
