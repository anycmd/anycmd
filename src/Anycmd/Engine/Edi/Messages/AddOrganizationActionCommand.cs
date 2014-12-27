
namespace Anycmd.Engine.Edi.Messages
{
    using Commands;
    using InOuts;
    using Model;

    public class AddOrganizationActionCommand: AddEntityCommand<IOrganizationActionCreateIo>, IAnycmdCommand
    {
        public AddOrganizationActionCommand(IOrganizationActionCreateIo input)
            : base(input)
        {

        }
    }
}
