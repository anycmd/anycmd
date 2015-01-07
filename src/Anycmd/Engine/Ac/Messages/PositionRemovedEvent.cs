
namespace Anycmd.Engine.Ac.Messages
{
    using Abstractions;
    using Events;

    public class PositionRemovedEvent : DomainEvent
    {
        public PositionRemovedEvent(GroupBase source)
            : base(source)
        {
        }
    }
}