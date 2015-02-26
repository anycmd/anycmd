
namespace Anycmd.Engine.Ac.Privileges
{
    using Events;

    public class RoleRolePrivilegeRemovedEvent : DomainEvent
    {
        public RoleRolePrivilegeRemovedEvent(IAcSession acSession, PrivilegeBase source)
            : base(acSession, source)
        {
        }
    }
}
