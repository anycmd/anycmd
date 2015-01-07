
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class BatchUpdatedEvent : DomainEvent
    {
        public BatchUpdatedEvent(IBatch source)
            : base(source)
        {
        }
    }
}
