
namespace Anycmd.Engine.Host.Edi.Messages
{
    using Commands;
    using InOuts;
    using Model;

    public class AddOntologyOrganizationCommand: AddEntityCommand<IOntologyOrganizationCreateIo>, ISysCommand
    {
        public AddOntologyOrganizationCommand(IOntologyOrganizationCreateIo input)
            : base(input)
        {

        }
    }
}
