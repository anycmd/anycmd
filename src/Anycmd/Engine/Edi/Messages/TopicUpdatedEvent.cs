
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class TopicUpdatedEvent : DomainEvent
    {
        /// <summary>
        /// 
        /// </summary>
        public TopicUpdatedEvent(TopicBase source)
            : base(source)
        {
        }
    }
}