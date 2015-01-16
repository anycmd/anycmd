
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using Abstractions;
    using Events;

    public class RoleRolePrivilegeRemovedEvent : DomainEvent
    {
        public RoleRolePrivilegeRemovedEvent(PrivilegeBase source)
            : base(source)
        {
        }
    }
}
