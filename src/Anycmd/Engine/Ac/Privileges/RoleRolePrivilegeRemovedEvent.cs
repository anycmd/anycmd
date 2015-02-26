
namespace Anycmd.Engine.Ac.Privileges
{
    using Abstractions;
    using Events;

    public class RoleRolePrivilegeRemovedEvent : DomainEvent
    {
        public RoleRolePrivilegeRemovedEvent(IAcSession acSession, PrivilegeBase source)
            : base(acSession, source)
        {
        }
    }
}
