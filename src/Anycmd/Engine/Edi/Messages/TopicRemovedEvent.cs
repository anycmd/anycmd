
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Edi.Abstractions;
    using Events;

    public class TopicRemovedEvent : DomainEvent
    {
        public TopicRemovedEvent(TopicBase source) : base(source) { }
    }
}
