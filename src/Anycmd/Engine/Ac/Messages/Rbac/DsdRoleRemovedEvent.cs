
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using Abstractions.Rbac;
    using Events;

    public class DsdRoleRemovedEvent : DomainEvent
    {
        public DsdRoleRemovedEvent(IUserSession userSession, DsdRoleBase source)
            : base(userSession, source)
        {
        }
    }
}