
namespace Anycmd.Engine.Ac.Messages
{
    using Abstractions;
    using Events;

    public class PrivilegeRemovedEvent : DomainEvent
    {
        public PrivilegeRemovedEvent(IUserSession userSession, PrivilegeBase source)
            : base(userSession, source)
        {
        }
    }
}
