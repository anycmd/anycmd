
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class BatchUpdatedEvent : DomainEvent
    {
        public BatchUpdatedEvent(IAcSession acSession, IBatch source)
            : base(acSession, source)
        {
        }
    }
}
