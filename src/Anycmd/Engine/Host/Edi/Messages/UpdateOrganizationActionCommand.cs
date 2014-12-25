
namespace Anycmd.Engine.Host.Edi.Messages
{
    using Commands;
    using InOuts;
    using Model;

    public class UpdateOrganizationActionCommand : UpdateEntityCommand<IOrganizationActionUpdateIo>, ISysCommand
    {
        public UpdateOrganizationActionCommand(IOrganizationActionUpdateIo input)
            : base(input)
        {

        }
    }
}
