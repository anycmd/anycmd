
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Commands;
    using InOuts;
    using Model;

    public class UpdateOrganizationCommand : UpdateEntityCommand<IOrganizationUpdateIo>, ISysCommand
    {
        public UpdateOrganizationCommand(IOrganizationUpdateIo input)
            : base(input)
        {

        }
    }
}
