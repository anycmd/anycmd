
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;

    public class UpdateOrganizationCommand : UpdateEntityCommand<IOrganizationUpdateIo>, IAnycmdCommand
    {
        public UpdateOrganizationCommand(IOrganizationUpdateIo input)
            : base(input)
        {

        }
    }
}
