
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public sealed class BatchUpdatedEvent : DomainEvent
    {
        public BatchUpdatedEvent(IAcSession acSession, IBatch source)
            : base(acSession, source)
        {
        }
    }
}
