
namespace Anycmd.Engine.Ac.Messages
{
    using Abstractions;
    using Events;

    public class PrivilegeRemovedEvent : DomainEvent
    {
        public PrivilegeRemovedEvent(IAcSession userSession, PrivilegeBase source)
            : base(userSession, source)
        {
        }
    }
}
