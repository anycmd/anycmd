
namespace Anycmd.Engine.Host.Ac.Messages
{
    using Engine.Ac.Abstractions;
    using Events;

    public class PositionRemovedEvent : DomainEvent
    {
        public PositionRemovedEvent(GroupBase source)
            : base(source)
        {
        }
    }
}