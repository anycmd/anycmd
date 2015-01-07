
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class BatchAddedEvent : DomainEvent
    {
        public BatchAddedEvent(IBatch source)
            : base(source)
        {
        }
    }
}
