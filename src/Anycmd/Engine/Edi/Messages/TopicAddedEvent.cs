
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class TopicAddedEvent : DomainEvent
    {
        public TopicAddedEvent(IUserSession userSession, TopicBase source) : base(userSession, source) { }
    }
}
