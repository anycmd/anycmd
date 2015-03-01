
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public sealed class TopicRemovedEvent : DomainEvent
    {
        public TopicRemovedEvent(IAcSession acSession, TopicBase source) : base(acSession, source) { }
    }
}
