
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class TopicRemovedEvent : DomainEvent
    {
        public TopicRemovedEvent(IAcSession userSession, TopicBase source) : base(userSession, source) { }
    }
}
