
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class AddNodeOntologyOrganizationCommand : AddEntityCommand<INodeOntologyOrganizationCreateIo>, IAnycmdCommand
    {
        public AddNodeOntologyOrganizationCommand(INodeOntologyOrganizationCreateIo input)
            : base(input)
        {

        }
    }
}
