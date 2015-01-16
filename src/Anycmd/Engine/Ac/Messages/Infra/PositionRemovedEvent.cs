
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;

    public class PositionRemovedEvent : DomainEvent
    {
        public PositionRemovedEvent(GroupBase source)
            : base(source)
        {
        }
    }
}