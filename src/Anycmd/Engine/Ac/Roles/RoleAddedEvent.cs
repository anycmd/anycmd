
namespace Anycmd.Engine.Ac.Roles
{
    using Messages;

    public class RoleAddedEvent : EntityAddedEvent<IRoleCreateIo>
    {
        public RoleAddedEvent(IAcSession acSession, RoleBase source, IRoleCreateIo output)
            : base(acSession, source, output)
        {
        }
    }
}
