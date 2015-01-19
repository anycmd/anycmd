
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using InOuts;

    public class AddDsdRoleCommand : AddEntityCommand<IDsdRoleCreateIo>, IAnycmdCommand
    {
        public AddDsdRoleCommand(IUserSession userSession, IDsdRoleCreateIo input)
            : base(userSession, input)
        {

        }
    }
}
