
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class TopicAddedEvent : DomainEvent
    {
        public TopicAddedEvent(IAcSession acSession, TopicBase source) : base(acSession, source) { }
    }
}
