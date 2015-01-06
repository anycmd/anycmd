
namespace Anycmd.Engine.Edi.Messages
{
    using InOuts;

    public class UpdateOrganizationActionCommand : UpdateEntityCommand<IOrganizationActionUpdateIo>, IAnycmdCommand
    {
        public UpdateOrganizationActionCommand(IOrganizationActionUpdateIo input)
            : base(input)
        {

        }
    }
}
