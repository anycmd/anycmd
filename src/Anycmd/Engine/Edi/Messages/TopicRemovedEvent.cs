
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class TopicRemovedEvent : DomainEvent
    {
        public TopicRemovedEvent(TopicBase source) : base(source) { }
    }
}
