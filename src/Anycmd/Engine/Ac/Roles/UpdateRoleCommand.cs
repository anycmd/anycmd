
namespace Anycmd.Engine.Ac.Roles
{
    using Messages;

    public sealed class UpdateRoleCommand : UpdateEntityCommand<IRoleUpdateIo>, IAnycmdCommand
    {
        public UpdateRoleCommand(IAcSession acSession, IRoleUpdateIo input)
            : base(acSession, input)
        {

        }
    }
}
