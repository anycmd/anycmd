
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Edi.Abstractions;
    using Events;

    public class TopicAddedEvent : DomainEvent
    {
        public TopicAddedEvent(TopicBase source) : base(source) { }
    }
}
