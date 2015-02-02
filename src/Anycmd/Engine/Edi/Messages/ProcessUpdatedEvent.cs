
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class ProcessUpdatedEvent : DomainEvent
    {
        public ProcessUpdatedEvent(IAcSession acSession, ProcessBase source)
            : base(acSession, source)
        {
        }
    }
}
