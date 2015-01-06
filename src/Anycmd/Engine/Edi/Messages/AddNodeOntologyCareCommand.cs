
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class AddNodeOntologyCareCommand: AddEntityCommand<INodeOntologyCareCreateIo>, IAnycmdCommand
    {
        public AddNodeOntologyCareCommand(INodeOntologyCareCreateIo input)
            : base(input)
        {

        }
    }
}
