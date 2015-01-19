
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using InOuts;

    public class AddRoleCommand : AddEntityCommand<IRoleCreateIo>, IAnycmdCommand
    {
        public AddRoleCommand(IUserSession userSession, IRoleCreateIo input)
            : base(userSession, input)
        {

        }
    }
}
