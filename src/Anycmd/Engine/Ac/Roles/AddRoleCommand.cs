
namespace Anycmd.Engine.Ac.Roles
{

    public class AddRoleCommand : AddEntityCommand<IRoleCreateIo>, IAnycmdCommand
    {
        public AddRoleCommand(IAcSession acSession, IRoleCreateIo input)
            : base(acSession, input)
        {

        }
    }
}
