
namespace Anycmd.Engine.Ac.Roles
{
    using Roles;
    using InOuts;

    public class RoleAddedEvent : EntityAddedEvent<IRoleCreateIo>
    {
        public RoleAddedEvent(IAcSession acSession, RoleBase source, IRoleCreateIo output)
            : base(acSession, source, output)
        {
        }
    }
}
