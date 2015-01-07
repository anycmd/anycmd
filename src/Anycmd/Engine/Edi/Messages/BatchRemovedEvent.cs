
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class BatchRemovedEvent : DomainEvent
    {
        public BatchRemovedEvent(IBatch source)
            : base(source)
        {
        }
    }
}
