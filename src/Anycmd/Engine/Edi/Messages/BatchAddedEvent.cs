
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public sealed class BatchAddedEvent : DomainEvent
    {
        public BatchAddedEvent(IAcSession acSession, IBatch source)
            : base(acSession, source)
        {
        }
    }
}
