
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using Abstractions;
    using Events;

    public class RoleRolePrivilegeAddedEvent : DomainEvent
    {
        public RoleRolePrivilegeAddedEvent(PrivilegeBase source)
            : base(source)
        {
        }
    }
}
