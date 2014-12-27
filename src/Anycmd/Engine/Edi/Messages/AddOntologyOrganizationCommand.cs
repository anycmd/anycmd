
namespace Anycmd.Engine.Edi.Messages
{
    using Commands;
    using InOuts;
    using Model;

    public class AddOntologyOrganizationCommand: AddEntityCommand<IOntologyOrganizationCreateIo>, IAnycmdCommand
    {
        public AddOntologyOrganizationCommand(IOntologyOrganizationCreateIo input)
            : base(input)
        {

        }
    }
}
