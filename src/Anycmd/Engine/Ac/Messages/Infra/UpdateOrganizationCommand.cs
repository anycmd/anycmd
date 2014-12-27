
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Commands;
    using InOuts;
    using Model;

    public class UpdateOrganizationCommand : UpdateEntityCommand<IOrganizationUpdateIo>, IAnycmdCommand
    {
        public UpdateOrganizationCommand(IOrganizationUpdateIo input)
            : base(input)
        {

        }
    }
}
