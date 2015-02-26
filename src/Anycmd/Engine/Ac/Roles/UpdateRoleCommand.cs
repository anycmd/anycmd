
namespace Anycmd.Engine.Ac.Roles
{

    public class UpdateRoleCommand : UpdateEntityCommand<IRoleUpdateIo>, IAnycmdCommand
    {
        public UpdateRoleCommand(IAcSession acSession, IRoleUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
