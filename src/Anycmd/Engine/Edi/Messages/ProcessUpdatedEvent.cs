
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public sealed class ProcessUpdatedEvent : DomainEvent
    {
        public ProcessUpdatedEvent(IAcSession acSession, ProcessBase source)
            : base(acSession, source)
        {
        }
    }
}
