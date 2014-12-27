
namespace Anycmd.Engine.Edi.Messages
{
    using Commands;
    using InOuts;
    using Model;

    public class UpdateOrganizationActionCommand : UpdateEntityCommand<IOrganizationActionUpdateIo>, IAnycmdCommand
    {
        public UpdateOrganizationActionCommand(IOrganizationActionUpdateIo input)
            : base(input)
        {

        }
    }
}
