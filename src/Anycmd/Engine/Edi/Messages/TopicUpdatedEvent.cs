
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public sealed class TopicUpdatedEvent : DomainEvent
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