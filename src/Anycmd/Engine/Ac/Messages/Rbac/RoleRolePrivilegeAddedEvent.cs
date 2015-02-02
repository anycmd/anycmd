
namespace Anycmd.Engine.Ac.Messages.Rbac
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
