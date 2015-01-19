
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using Abstractions;
    using Events;

    public class RoleRolePrivilegeAddedEvent : DomainEvent
    {
        public RoleRolePrivilegeAddedEvent(IUserSession userSession, PrivilegeBase source)
            : base(userSession, source)
        {
        }
    }
}
