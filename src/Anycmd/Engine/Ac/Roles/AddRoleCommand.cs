
namespace Anycmd.Engine.Ac.Roles
{
    using Messages;

    public class AddRoleCommand : AddEntityCommand<IRoleCreateIo>, IAnycmdCommand
    {
        public AddRoleCommand(IAcSession acSession, IRoleCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
