
namespace Anycmd.Engine.Ac.Privileges
{
    using Events;

    public class RoleRolePrivilegeAddedEvent : DomainEvent
    {
        public RoleRolePrivilegeAddedEvent(IAcSession acSession, PrivilegeBase source)
            : base(acSession, source)
        {
        }
    }
}
