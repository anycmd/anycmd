
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Edi.Abstractions;
    using Events;

    public class BatchRemovedEvent : DomainEvent
    {
        public BatchRemovedEvent(IBatch source)
            : base(source)
        {
        }
    }
}
