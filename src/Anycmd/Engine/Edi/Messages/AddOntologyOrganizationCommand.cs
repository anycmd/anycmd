
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class AddOntologyOrganizationCommand: AddEntityCommand<IOntologyOrganizationCreateIo>, IAnycmdCommand
    {
        public AddOntologyOrganizationCommand(IUserSession userSession, IOntologyOrganizationCreateIo input)
            : base(userSession, input)
        {

        }
    }
}
