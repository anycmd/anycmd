
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using Abstractions.Rbac;
    using Events;

    public class RoleRemovedEvent : DomainEvent
    {
        public RoleRemovedEvent(IUserSession userSession, RoleBase source)
            : base(userSession, source)
        {
        }
    }
}