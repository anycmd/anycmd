
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class TopicUpdatedEvent : DomainEvent
    {
        /// <summary>
        /// 
        /// </summary>
        public TopicUpdatedEvent(IAcSession acSession, TopicBase source)
            : base(acSession, source)
        {
        }
    }
}