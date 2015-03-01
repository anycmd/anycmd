
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public sealed class TopicAddedEvent : DomainEvent
    {
        public TopicAddedEvent(IAcSession acSession, TopicBase source) : base(acSession, source) { }
    }
}
