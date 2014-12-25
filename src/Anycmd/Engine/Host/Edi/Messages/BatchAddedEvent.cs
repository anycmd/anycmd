
namespace Anycmd.Engine.Host.Edi.Messages
{
    using Engine.Edi.Abstractions;
    using Events;

    public class BatchAddedEvent : DomainEvent
    {
        public BatchAddedEvent(IBatch source)
            : base(source)
        {
        }
    }
}
