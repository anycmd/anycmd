
namespace Anycmd.Engine.Ac.Messages
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
