
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using Abstractions.Rbac;
    using Events;

    public class RoleRemovingEvent: DomainEvent
    {
        public RoleRemovingEvent(IUserSession userSession, RoleBase source)
            : base(userSession, source)
        {
        }
    }
}