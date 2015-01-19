
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using Abstractions;
    using Events;

    public class RoleRolePrivilegeRemovedEvent : DomainEvent
    {
        public RoleRolePrivilegeRemovedEvent(IUserSession userSession, PrivilegeBase source)
            : base(userSession, source)
        {
        }
    }
}
