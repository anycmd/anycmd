
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using InOuts;

    public class UpdateOrganizationCommand : UpdateEntityCommand<IOrganizationUpdateIo>, IAnycmdCommand
    {
        public UpdateOrganizationCommand(IUserSession userSession, IOrganizationUpdateIo input)
            : base(userSession, input)
        {

        }
    }
}
