
namespace Anycmd.Engine.Ac.Messages
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
