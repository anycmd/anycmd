
namespace Anycmd.Engine.Host.Edi.Messages
{
    using Engine.Edi.Abstractions;
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