
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using Abstractions.Rbac;
    using Events;

    public class SsdRoleRemovedEvent : DomainEvent
    {
        public SsdRoleRemovedEvent(IUserSession userSession, SsdRoleBase source)
            : base(userSession, source)
        {
        }
    }
}