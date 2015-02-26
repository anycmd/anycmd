
namespace Anycmd.Engine.Ac.Privileges
{
    using Abstractions;
    using Events;

    public class RoleRolePrivilegeAddedEvent : DomainEvent
    {
        public RoleRolePrivilegeAddedEvent(IAcSession acSession, PrivilegeBase source)
            : base(acSession, source)
        {
        }
    }
}
