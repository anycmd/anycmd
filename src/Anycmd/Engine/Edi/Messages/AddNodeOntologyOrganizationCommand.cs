
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class AddNodeOntologyOrganizationCommand : AddEntityCommand<INodeOntologyOrganizationCreateIo>, IAnycmdCommand
    {
        public AddNodeOntologyOrganizationCommand(IUserSession userSession, INodeOntologyOrganizationCreateIo input)
            : base(userSession, input)
        {

        }
    }
}
