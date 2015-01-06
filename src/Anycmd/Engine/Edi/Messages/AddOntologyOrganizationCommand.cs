
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class AddOntologyOrganizationCommand: AddEntityCommand<IOntologyOrganizationCreateIo>, IAnycmdCommand
    {
        public AddOntologyOrganizationCommand(IOntologyOrganizationCreateIo input)
            : base(input)
        {

        }
    }
}
