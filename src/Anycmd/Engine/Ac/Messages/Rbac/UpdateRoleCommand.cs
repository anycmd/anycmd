
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using InOuts;

    public class UpdateRoleCommand : UpdateEntityCommand<IRoleUpdateIo>, IAnycmdCommand
    {
        public UpdateRoleCommand(IUserSession userSession, IRoleUpdateIo input)
            : base(userSession, input)
        {

        }
    }
}
