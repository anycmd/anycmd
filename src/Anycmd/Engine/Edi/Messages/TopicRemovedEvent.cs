
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class TopicRemovedEvent : DomainEvent
    {
        public TopicRemovedEvent(IUserSession userSession, TopicBase source) : base(userSession, source) { }
    }
}
