
namespace Anycmd.Engine.Ac.Messages
{
    using Abstractions;
    using Events;

    public class PrivilegeRemovedEvent : DomainEvent
    {
        public PrivilegeRemovedEvent(PrivilegeBase source)
            : base(source)
        {
        }
    }
}
