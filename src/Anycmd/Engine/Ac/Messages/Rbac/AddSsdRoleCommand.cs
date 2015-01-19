
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using InOuts;

    public class AddSsdRoleCommand : AddEntityCommand<ISsdRoleCreateIo>, IAnycmdCommand
    {
        public AddSsdRoleCommand(IUserSession userSession, ISsdRoleCreateIo input)
            : base(userSession, input)
        {

        }
    }
}
