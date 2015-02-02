
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class BatchAddedEvent : DomainEvent
    {
        public BatchAddedEvent(IAcSession acSession, IBatch source)
            : base(acSession, source)
        {
        }
    }
}
